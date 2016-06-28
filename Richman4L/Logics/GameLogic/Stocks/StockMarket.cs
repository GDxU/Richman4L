﻿using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . IO;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Xml . Linq;

using WenceyWang . Richman4L . Buffs . StockBuffs;

namespace WenceyWang . Richman4L . Stocks
{
	/// <summary>
	/// 表示股票市场
	/// </summary>
	public class StockMarket : GameObject
	{

		public List<Stock> Stocks { get; private set; } = new List<Stock> ( );

		public StockPriceMovement Movement { get; internal set; }

		public List<StockBuff> Buffs { get; private set; } = new List<StockBuff> ( );

		private Calendars . GameDate _movementChanging = Game . Current . Calendar . Today;

		public StockPrice SynthesizePrice
		{
			get
			{
				return new StockPrice ( Stocks . Sum ( ( stock ) => stock . CurrentPrice . OpenPrice ) ,
										Stocks . Sum ( ( stock ) => stock . CurrentPrice . CurrentPrice ) ,
										Stocks . Sum ( ( stock ) => stock . CurrentPrice . TodaysHigh ) ,
										Stocks . Sum ( ( stock ) => stock . CurrentPrice . TodaysLow ) ,
										Stocks . Sum ( ( stock ) => stock . CurrentPrice . BuyValue ) ,
										Stocks . Sum ( ( stock ) => stock . CurrentPrice . SellValue ) );
			}
		}

		public override void EndToday ( )
		{
			CheckDisposed ( );

			foreach ( Stock item in Stocks )
			{
				item . EndToday ( );
			}
		}

		public override void StartDay ( Calendars . GameDate date )
		{
			if ( DisposedValue )
			{
				throw new ObjectDisposedException ( nameof ( StockMarket ) );
			}

			if ( date == _movementChanging )
			{
				switch ( GameRandom . Current . Next ( 0 , 3 ) )
				{
					case 1:
						{
							Movement = StockPriceMovement . Rise;
							break;
						}
					case 2:
						{
							Movement = StockPriceMovement . Keep;
							break;
						}
					case 3:
						{
							Movement = StockPriceMovement . Fall;
							break;
						}
				}

				_movementChanging += GameRandom . Current . Next ( 20 , 30 );
			}

			foreach ( Stock item in Stocks )
			{
				item . StartDay ( date );
			}
		}

		public event EventHandler MovementChanged;

		private List<BuyStockDelegate> BuyDelegates { get; set; }

		public void BuyStock ( BuyStockDelegate @delegate ) { BuyDelegates . Add ( @delegate ); }

		/// <summary>
		/// 加载资源
		/// </summary>
		/// <param name="flieName"></param>
		public StockMarket ( string flieName )
			: this ( ResourceHelper . LoadXmlDocument ( $"{nameof ( Stocks )}.Resources.{flieName}" ) ) { }

		private StockMarket ( XDocument document ) : this ( )
		{
			//ToDo LoadStocks
		}

		public StockMarket ( ) : base ( ) { }

		public override string ToString ( )
		{
			return $"StockMarket ";

			//Todo:Compele this;
		}

	}

}