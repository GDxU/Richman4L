﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using WenceyWang . Richman4L . Logics . Buffs . StockBuffs ;
using WenceyWang . Richman4L . Logics . Calendars ;
using WenceyWang . Richman4L . Logics . Players ;
using WenceyWang . Richman4L . Logics . Players . PayReasons ;

namespace WenceyWang . Richman4L . Logics . Stocks
{

	/// <summary>
	///     表示股票市场
	/// </summary>
	public class StockMarket : WithAssetObject
	{

		private GameDate _movementChanging = Game . Current . Calendar . Today ;

		[Own]
		public List <Stock> Stocks { get ; } = new List <Stock> ( ) ;

		public StockPriceMovement Movement { get ; internal set ; }

		[Own]
		public decimal DelegateFeeRate { get ; set ; }

		//todo:event

		[Own]
		public List <StockBuff> Buffs { get ; } = new List <StockBuff> ( ) ;

		/// <summary>
		///     市场的总值
		/// </summary>
		public StockPrice SynthesizePrice
		{
			get
			{
				return new StockPrice ( Stocks . Sum ( stock => stock . Price . OpenPrice ) ,
										Stocks . Sum ( stock => stock . Price . CurrentPrice ) ,
										Stocks . Sum ( stock => stock . Price . TodaysHigh ) ,
										Stocks . Sum ( stock => stock . Price . TodaysLow ) ,
										Stocks . Sum ( stock => stock . Price . BuyVolume ) ,
										Stocks . Sum ( stock => stock . Price . SellVolume ) ) ;
			}
		}

		[NotNull]
		private Dictionary <Stock , List <BuyStockDelegate>> BuyDelegateList { get ; set ; }

		[NotNull]
		private Dictionary <Stock , List <SellStockDelegate>> SellDelegateList { get ; set ; }

		public StockMarketState State { get ; set ; }

		public override IEnumerable <IAsset> Assets { get ; }


		private StockMarket ( [NotNull] XDocument document ) : this ( )
		{
			if ( document == null )
			{
				throw new ArgumentNullException ( nameof(document) ) ;
			}

			try
			{
				foreach ( XElement stock in document . Root . Elements ( ) )
				{
					Stocks . Add ( new Stock ( stock ) ) ;
				}
			}
			catch ( NullReferenceException e )
			{
				throw new ArgumentException ( $"{nameof(document)} has wrong data or lack of data" , e ) ;
			}

			//ToDo LoadStocks
		}

		public StockMarket ( ) { }

		public override void EndToday ( )
		{
			//	1. Let stock end it's day

			foreach ( Stock item in Stocks )
			{
				item . EndToday ( ) ;
			}


			// 2. Process every delegate

			List <Stock> toProcess = BuyDelegateList . Keys . Union ( SellDelegateList . Keys ) . ToList ( ) ;

			foreach ( Stock stock in toProcess )
			{
				List <SellStockDelegate> sellDelegates = SellDelegateList [ stock ] ;
				List <BuyStockDelegate> buyDelegates = BuyDelegateList [ stock ] ;

				if ( stock . TransactToday )
				{
					sellDelegates . Sort ( ( x , y ) => x . Price . CompareTo ( y . Price ) ) ;
					buyDelegates . Sort ( ( x , y ) => y . Price . CompareTo ( x . Price ) ) ;

					decimal minSellPrice = sellDelegates . Min ( dele => dele . Price ) ;
					if ( minSellPrice < stock . Price . TodaysLow )
					{
						stock . LowerTodaysLow ( sellDelegates . Min ( dele => dele . Price ) ) ;
					}

					decimal maxBuyPrice = buyDelegates . Max ( dele => dele . Price ) ;
					if ( maxBuyPrice > stock . Price . TodaysHigh )
					{
						stock . HigherTodaysHigh ( maxBuyPrice ) ;
					}

					if ( ! stock . IsBlockSell ( ) )
					{
						int buyVolume = stock . Price . BuyVolume ;

						if ( ! stock . IsBlockBuy ( ) )
						{
							buyVolume += BuyDelegateList [ stock ] .
								Where ( dele => dele . Price > minSellPrice ) .
								Sum ( dele => dele . Number ) ;
						}

						foreach ( SellStockDelegate dele in sellDelegates )
						{
							if ( dele . Price <= stock . Price . TodaysHigh )
							{
								if ( buyVolume > dele . Number )
								{
									buyVolume -= dele . Number ;

									//dele . Player . TakeAwayStock ( stock , dele . Number ) ;
									dele . State = SellStockDelegateState . Completed ;
								}
								else
								{
									buyVolume -= buyVolume ;

									//dele . Player . TakeAwayStock ( stock , buyVolume ) ;
									dele . State = SellStockDelegateState . VolumeNotEnough ;
									break ;
								}
							}
							else
							{
								dele . State = SellStockDelegateState . PriceNotSuit ;
							}
						}
					}

					if ( ! stock . IsBlockBuy ( ) )
					{
						int sellVolume = stock . Price . BuyVolume ;
						if ( ! stock . IsBlockSell ( ) )
						{
							sellVolume += BuyDelegateList [ stock ] . Sum ( dele => dele . Number ) ;
						}
						foreach ( BuyStockDelegate dele in buyDelegates )
						{
							if ( dele . Price >= stock . Price . TodaysLow )
							{
								if ( sellVolume > dele . Number )
								{
									sellVolume -= dele . Number ;

									//todo
									//dele . Player . PayForBuyStock ( stock , dele . Number , dele . Number * dele . Price . ToLongFloor ( ) ) ;

									dele . State = BuyStockDelegateState . Completed ;
								}
								else
								{
									sellVolume -= sellVolume ;

									//dele . Player . PayForBuyStock ( stock , sellVolume , sellVolume * dele . Price . ToLongFloor ( ) ) ;
									dele . Player . ReceiveStock ( stock , sellVolume ) ;
									dele . State = BuyStockDelegateState . ValueNotEnough ;
									break ;
								}
							}
						}
					}
				}
				else
				{
					foreach ( SellStockDelegate deles in sellDelegates )
					{
						deles . State = SellStockDelegateState . StockCannotSell ;
					}
					foreach ( BuyStockDelegate deles in buyDelegates )
					{
						deles . State = BuyStockDelegateState . StockCannotBuy ;
					}
				}

				foreach ( SellStockDelegate deles in sellDelegates )
				{
					PayMoneyForStockDelegateReason reason =
						new PayMoneyForStockDelegateReason ( deles ,
															( deles . Price * deles . Number * DelegateFeeRate ) . ToLongCelling ( ) ,
															this ) ;

					//deles.Player.ReceivePayReason()
					//todo
					//deles . Player . PayForStockDelegate ( deles ,
					//( deles . Price * deles . Number * DelegateFeeRate ) . ToLongCelling ( ) ) ;
				}
				foreach ( BuyStockDelegate deles in buyDelegates )
				{
					//deles . Player . PayForStockDelegate ( deles ,
					//( deles . Price * deles . Number * DelegateFeeRate ) . ToLongCelling ( ) ) ;
				}
			}
		}

		public override void StartDay ( GameDate date )
		{
			if ( date == _movementChanging )
			{
				switch ( GameRandom . Current . Next ( 0 , 3 ) )
				{
					case 1 :
					{
						Movement = StockPriceMovement . Rise ;
						break ;
					}
					case 2 :
					{
						Movement = StockPriceMovement . Keep ;
						break ;
					}
					case 3 :
					{
						Movement = StockPriceMovement . Fall ;
						break ;
					}
				}

				_movementChanging += GameRandom . Current . Next ( 20 , 30 ) ;
			}

			foreach ( Stock item in Stocks )
			{
				item . StartDay ( date ) ;
			}
		}

		/// <summary>
		///     提交购买股票的委托
		/// </summary>
		/// <param name="buyDelegate">要提交的委托</param>
		public void BuyStock ( [NotNull] BuyStockDelegate buyDelegate )
		{
			if ( buyDelegate == null )
			{
				throw new ArgumentNullException ( nameof(buyDelegate) ) ;
			}

			if ( ! BuyDelegateList . ContainsKey ( buyDelegate . Stock ) )
			{
				BuyDelegateList . Add ( buyDelegate . Stock , new List <BuyStockDelegate> ( ) ) ;
			}
			BuyDelegateList [ buyDelegate . Stock ] . Add ( buyDelegate ) ;
		}

		/// <summary>
		///     提交销售股票的委托
		/// </summary>
		/// <param name="sellDelegate">要提交的委托</param>
		public void SellStock ( [NotNull] SellStockDelegate sellDelegate )
		{
			if ( sellDelegate == null )
			{
				throw new ArgumentNullException ( nameof(sellDelegate) ) ;
			}

			if ( ! SellDelegateList . ContainsKey ( sellDelegate . Stock ) )
			{
				SellDelegateList . Add ( sellDelegate . Stock , new List <SellStockDelegate> ( ) ) ;
			}
			SellDelegateList [ sellDelegate . Stock ] . Add ( sellDelegate ) ;
		}

		public override string ToString ( )
		{
			return $"StockMarket with {Stocks . Count} stock" ;

			//Todo:Compele this;			
		}

		public override void ReceiveTransactionRequest ( AssetTransactionAgreement request )
		{
			throw new NotImplementedException ( ) ;
		}

		public override void RequestPay ( WithAssetObject source , PayMoneyReason reason )
		{
			throw new NotImplementedException ( ) ;
		}

		public override void RequestAsset ( WithAssetObject source , IAsset asset , PayMoneyReason reason )
		{
			throw new NotImplementedException ( ) ;
		}

		public override void ReceiveCash ( WithAssetObject source , decimal amount , PayMoneyReason reason )
		{
			throw new NotImplementedException ( ) ;
		}

		public override void ReceiveCheck ( WithAssetObject source , decimal amount , PayMoneyReason reason )
		{
			throw new NotImplementedException ( ) ;
		}

		public override void ReceiveTransfer ( WithAssetObject source , decimal amount , PayMoneyReason reason )
		{
			throw new NotImplementedException ( ) ;
		}

		public override void ReceivePayReason ( PayMoneyReason reason ) { throw new NotImplementedException ( ) ; }

	}

	public enum StockMarketState
	{

		Running ,

		NotRunning ,

		Restricted

	}

}
