﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using WenceyWang . Richman4L . Annotations ;
using WenceyWang . Richman4L . Auctions ;
using WenceyWang . Richman4L . Banks ;
using WenceyWang . Richman4L . Buffs . PlayerBuffs ;
using WenceyWang . Richman4L . Calendars ;
using WenceyWang . Richman4L . Cards ;
using WenceyWang . Richman4L . Maps ;
using WenceyWang . Richman4L . Maps . Buildings ;
using WenceyWang . Richman4L . Maps . Roads ;
using WenceyWang . Richman4L . Players . Commands ;
using WenceyWang . Richman4L . Players . Events ;
using WenceyWang . Richman4L . Players . Models ;
using WenceyWang . Richman4L . Players . PayReasons ;
using WenceyWang . Richman4L . Stocks ;

namespace WenceyWang . Richman4L . Players
{

	public sealed class AssetTransactionAgreement
	{

		public WithAssetObject PartyA { get ; set ; }

		public IAsset PartyAProvide { get ; set ; }

		public WithAssetObject PartyB { get ; set ; }

		public IAsset PartyBProvide { get ; set ; }


		/// <summary>
		///     被用于希望卖东西
		/// </summary>
		/// <param name="partyA"></param>
		/// <param name="partyAProvide"></param>
		/// <param name="partyB"></param>
		public AssetTransactionAgreement ( WithAssetObject partyA , IAsset partyAProvide , WithAssetObject partyB )
		{
			PartyA = partyA ;
			PartyAProvide = partyAProvide ;
			PartyB = partyB ;
		}

		/// <summary>
		///     被用于希望买东西
		/// </summary>
		/// <param name="partyA"></param>
		/// <param name="partyB"></param>
		/// <param name="partyBProvide"></param>
		public AssetTransactionAgreement ( WithAssetObject partyA , WithAssetObject partyB , IAsset partyBProvide )
		{
			PartyA = partyA ;
			PartyB = partyB ;
			PartyBProvide = partyBProvide ;
		}

	}

	/// <summary>
	///     钱被作为资产给出
	/// </summary>
	public class MoneyAsset : IAsset
	{

		public long Amount { get ; }

		public WithAssetObject Owner { get ; private set ; }

		public long MinimumValue => Amount ;

		public bool CanGive { get ; }

		public void GiveTo ( WithAssetObject newOwner )
		{
			newOwner . ReceivePayReason ( new PayMoneyForGiveMoneyAsset ( this , Owner ) ) ;
			Owner = newOwner ;
		}

	}


	//Todo:玩家破产了怎么办？
	//Todo:玩家希望退出游戏怎么办？
	public class Player : WithAssetObject
	{

		private long _money ;

		public PlayerOnMap MapObject { get ; set ; }

		[Own]
		[CanBeNull]
		public IUser User { get ; }

		/// <summary>
		///     玩家所拥有的前进类型
		/// </summary>
		[Own ( PropertyVisability . Owner )]
		public List <MoveType> MoveTypeList { get ; } = new List <MoveType> ( ) ;

		/// <summary>
		///     玩家的名称
		/// </summary>
		[NotNull]
		[Own]
		public string Name => $"{User ? . DisplayName ?? "No One"} Play as {Model . Name}" ;

		[Own]
		public int Health { get ; set ; }

		/// <summary>
		///     玩家的模型
		/// </summary>
		[NotNull]
		[Own]
		public PlayerModel Model { get ; }

		[NotNull]
		[ItemNotNull]
		private Dictionary <DiceType , int> diceList { get ; } = new Dictionary <DiceType , int> ( ) ;

		[NotNull]
		[ItemNotNull]
		[Own ( PropertyVisability . Owner )]
		public ReadOnlyDictionary <DiceType , int> DiceList { get ; }

		/// <summary>
		///     玩家的游戏点数
		/// </summary>
		[Own ( PropertyVisability . Owner )]
		public long GamePoint { get ; set ; } = 0 ;

		/// <summary>
		///     玩家的股票
		/// </summary>
		[NotNull]
		[ItemNotNull]
		public Dictionary <Stock , int> Stocks { get ; }


		/// <summary>
		///     玩家的地块
		/// </summary>
		[NotNull]
		[ItemNotNull]
		public List <Area> Areas => Map . Currnet . Objects . Where ( mapObject => ( mapObject as Area ) ? . Owner == this ) .
										Select ( mapObject => ( Area ) mapObject ) . ToList ( ) ;

		/// <summary>
		///     玩家当前的位置
		/// </summary>
		[NotNull]
		public Road Position { get ; private set ; }

		/// <summary>
		///     玩家前一个位置（用于确定玩家的方向）
		/// </summary>
		[NotNull]
		public Road PreviousPosition { get ; private set ; }

		/// <summary>
		///     玩家的增益效果
		/// </summary>
		[NotNull]
		[ItemNotNull]
		[Own]
		public List <PlayerBuff> Buffs { get ; } = new List <PlayerBuff> ( ) ;

		[Own]
		public bool HaveMoveToday { get ; protected set ; }

		/// <summary>
		///     指示玩家能否获得收益
		/// </summary>
		public bool CanGetCharge => State == PlayerState . Normal && this . IsBlockGetCharge ( ) ;

		/// <summary>
		///     指示玩家当前是否能移动
		/// </summary>
		[Own]
		public bool CanMove => ! HaveMoveToday && State == PlayerState . Normal && this . IsBlockMoving ( ) ;

		[NotNull]
		[Own ( PropertyVisability . Owner )]
		public ReadOnlyCollection <long> MoneyHistory { get ; }

		[NotNull]
		private List <long> moneyHistory { get ; }

		[Own]
		public long Money
		{
			get => _money ;
			private set
			{
				_money = value ;
				if ( _money < 0 )
				{
					//理论上这个情况不该发生
					//要求付款的时候会要求那个CanPay,否则会直接触发破产

					Bankruptcy ( PlayerBankruptcyReason . CanNotPay ) ; //Todo:Money Not Enough?
				}
			}
		}

		[Own]
		public decimal PropertiesInMoney => Money + SavedMoney . Sum ( proof => proof . MoneyToGet )
											- BorrowedMoney . Sum ( proof => proof . MoneyToReturn ) ;

		[NotNull]
		[ItemNotNull]
		[Own ( PropertyVisability . Owner )]
		public List <SavingBankProof> SavedMoney { get ; } = new List <SavingBankProof> ( ) ;

		[NotNull]
		[ItemNotNull]
		public List <BorrowingBankProof> BorrowedMoney { get ; } = new List <BorrowingBankProof> ( ) ;


		public Player ( [NotNull] PlayerModel model , long startMoney )
		{
			if ( startMoney < 0 )
			{
				throw new ArgumentOutOfRangeException ( nameof(startMoney) ) ;
			}

			Model = model ?? throw new ArgumentNullException ( nameof(model) ) ;
			Money = startMoney ;
		}

		public int GetStockNumber ( [NotNull] Stock stock )
		{
			if ( stock == null )
			{
				throw new ArgumentNullException ( nameof(stock) ) ;
			}

			if ( Stocks . ContainsKey ( stock ) )
			{
				return Stocks [ stock ] ;
			}
			else
			{
				return 0 ;
			}
		}

		public void TakeHarm ( HarmType type , int volume ) { }

		public void Pay ( long money )
		{
			//Todo:
		}

		public void ReceiveMoney ( long money )
		{
			//Todo:
		}

		[NotNull]
		public ReadOnlyCollection <PlayerCommand> GetAviliableCommands ( )
		{
			List <PlayerCommand> commands = new List <PlayerCommand> ( ) ;


			return new ReadOnlyCollection <PlayerCommand> ( commands ) ;
		}

		[PublicAPI]
		public void RemoveBuff ( [NotNull] PlayerBuff buff )
		{
			if ( buff == null )
			{
				throw new ArgumentNullException ( nameof(buff) ) ;
			}
			if ( ! Buffs . Contains ( buff ) )
			{
				throw new ArgumentException ( $"{nameof(buff)} is not for this player" , nameof(buff) ) ;
			}

			Buffs . Remove ( buff ) ;

			LostBuffEvent ? . Invoke ( this , new PlayerLostBuffEventArgs ( buff ) ) ;
		}

		[CanBeNull]
		public event EventHandler <PlayerLostBuffEventArgs> LostBuffEvent ;

		/// <summary>
		///     在指定位置建造指定类型的建筑建造建筑
		/// </summary>
		/// <param name="position">要建造建筑的位置</param>
		/// <param name="buildingType">要建筑的类型</param>
		public void BuildBuilding ( [NotNull] Area position , [NotNull] BuildingType buildingType )
		{
			#region Check ArgumentsInfo and State

			if ( position == null )
			{
				throw new ArgumentNullException ( nameof(position) ) ;
			}
			if ( buildingType == null )
			{
				throw new ArgumentNullException ( nameof(buildingType) ) ;
			}
			if ( position . Owner != this )
			{
				throw new ArgumentException ( $"{nameof(position)} should owned by player." , nameof(position) ) ;
			}
			if ( ! Building . BuildingTypes . Contains ( buildingType ) )
			{
				throw new ArgumentException ( $"{nameof(buildingType)} have not been regis." , nameof(buildingType) ) ;
			}
			if ( ! position . IsBuildingAvailable ( buildingType ) )
			{
				throw new ArgumentException ( $"{nameof(buildingType)} can not build in {nameof(position)}." ,
											nameof(buildingType) ) ;
			}

			#endregion

			position . BuildBuildiing ( buildingType ) ;
			BuildBuildingEvent ? . Invoke ( this , new EventArgs ( ) ) ;
		}

		[CanBeNull]
		public event EventHandler BuildBuildingEvent ;


		public void ChangeState ( PlayerState state , int duration )
		{
			if ( duration < 1 )
			{
				throw new ArgumentOutOfRangeException ( nameof(duration) ) ;
			}

			PlayerState oldState = State ;
			State = state ;
			StateStartDate = Game . Current . Calendar . Today ;
			StateDuration = duration ;
			ChangeStateEvent ? . Invoke ( this , new PlayerChangeStateEventArgs ( oldState , State , duration ) ) ;
		}

		[CanBeNull]
		public event EventHandler <PlayerChangeStateEventArgs> ChangeStateEvent ;

		public override void StartDay ( GameDate thisDate )
		{
			#region Change State

			if ( State != PlayerState . Normal
				&& StateStartDate + StateDuration >= thisDate )
			{
				StateStartDate = thisDate ;
				StateDuration = 0 ;
				State = PlayerState . Normal ;
			}

			#endregion

			HaveMoveToday = false ;
		}

		public override void EndToday ( ) { moneyHistory . Add ( Money ) ; }

		public void ExitGame ( ) { Bankruptcy ( PlayerBankruptcyReason . BySelf ) ; }

		/// <summary>
		///     宣告破产
		/// </summary>
		/// <param name="reason">破产的原因</param>
		public void Bankruptcy ( PlayerBankruptcyReason reason )
		{
			//State = PlayerState . Normal ; //Why Normal? What if player is in jail or hospital

			//todo:Fix this
			foreach ( IAsset asset in Assets )
			{
				AuctionRequest request = new AuctionRequest ( asset . MinimumValue , this , asset ) ;
				Game . Current . AuctionPerformer . PerformAuction ( request ) ;
			}

			BankruptcyEvent ? . Invoke ( this , new PlayerBankruptcyEventArgs ( reason ) ) ;
		}

		[CanBeNull]
		public event EventHandler <PlayerBankruptcyEventArgs> BankruptcyEvent ;


		[NotNull]
		public override string ToString ( )
		{
			return Name ;
		}

		public bool CanPay ( long money ) { return Money >= money ; }

		/// <summary>
		///     移动
		/// </summary>
		/// <param name="moveType">移动类型</param>
		/// <param name="diceType">使用的骰子</param>
		public virtual void Move ( MoveType moveType , DiceType diceType )
		{
			if ( ! MoveTypeList . Contains ( moveType ) )
			{
				throw new ArgumentOutOfRangeException ( nameof(moveType) ) ;
			}

			//Todo:Check if player can use this dice

			if ( CanMove )
			{
				HaveMoveToday = true ;
				ReadOnlyCollection <int> diceResult = Game . Current . GameRule . GetDice ( diceType , ( int ) moveType ) ;
				Path route = Position . Route ( PreviousPosition , diceResult . Sum ( ) ) ;
				foreach ( Road item in route . Route )
				{
					Position . Pass ( this , moveType ) ;
					PreviousPosition = Position ;
					Position = item ;
				}

				Position . Stay ( this , moveType ) ;
				MoveEvent ? . Invoke ( this , new PlayerMoveEventArgs ( route , diceResult ) ) ;
			}
			else
			{
				MoveEvent ? . Invoke ( this ,
										new PlayerMoveEventArgs ( new Path ( ) , new ReadOnlyCollection <int> ( new List <int> ( ) ) ) ) ;
			}
		}

		[CanBeNull]
		public event EventHandler <PlayerMoveEventArgs> MoveEvent ;

		//看起来没必要
		///// <summary>
		/////     购买某个区域
		///// </summary>
		///// <param name="toBuy">要购买的区域</param>
		///// <returns></returns>
		//public BuyAreaResult BuyArea ( [NotNull] Area toBuy )
		//{
		//	if ( toBuy == null )
		//	{
		//		throw new ArgumentNullException ( nameof(toBuy) ) ;
		//	}

		//	BuyAreaResult result = BuyAreaResult . Crate ( toBuy ) ;

		//	if ( ! toBuy . IsBlockBuy ( ) )
		//	{
		//		if ( ! this . IsBlockBuyArea ( ) )
		//		{
		//			if ( toBuy . Owner == null )
		//			{
		//				if ( Money >= toBuy . Price )
		//				{
		//					Money -= toBuy . Price ;
		//					result . Area = toBuy ;
		//					toBuy . Owner = this ;
		//					result . StatusCode = BuyAreaStatusCode . Success ;
		//					result . CostMoney = toBuy . Price ;
		//				}
		//				else
		//				{
		//					result . StatusCode = BuyAreaStatusCode . MoneyNotEnough ;
		//				}
		//			}
		//			else
		//			{
		//				result . StatusCode = BuyAreaStatusCode . NotBuyable ;
		//			}
		//		}
		//		else
		//		{
		//			result . StatusCode = BuyAreaStatusCode . PlayerDebuff ;
		//		}
		//	}
		//	else
		//	{
		//		result . StatusCode = BuyAreaStatusCode . AreaDebuff ;
		//	}
		//	BuyAreaEvent ? . Invoke ( this , new PlayerBuyAreaEventArgs ( result ) ) ;
		//	return result ;
		//}

		[CanBeNull]
		public event EventHandler <PlayerBuyAreaEventArgs> BuyAreaEvent ;

		#region TakeAwayStock

		//public void TakeAwayStock([NotNull] Stock stock, int number)
		//{
		//	if (stock == null)
		//	{
		//		throw new ArgumentNullException(nameof(stock));
		//	}

		//	if (Stocks.ContainsKey(stock))
		//	{
		//		if (GetStockNumber(stock) > number)
		//		{
		//			GetStockNumber(stock) -= number;
		//		}
		//		else
		//		{
		//			if (GetStockNumber(stock) == number)
		//			{
		//				Stocks.Remove(stock);
		//			}
		//			else
		//			{
		//				if (GetStockNumber(stock) < number)
		//				{
		//					Bankruptcy(PlayerBankruptcyReason.OversoldStock);
		//				}
		//			}
		//		}
		//	}
		//	else
		//	{
		//		Bankruptcy(PlayerBankruptcyReason.OversoldStock);
		//	}

		//	TakeAwayStockEvent?.Invoke(this, new PlayerTakeAwayStockEventArgs(stock, number));
		//}

		[CanBeNull]
		public event EventHandler <PlayerTakeAwayStockEventArgs> TakeAwayStockEvent ;

		#endregion

		#region State

		/// <summary>
		///     玩家的状态
		/// </summary>
		[Own]
		public PlayerState State { get ; protected set ; }

		/// <summary>
		///     当前状态将会持续的时间
		/// </summary>
		[CanBeNull]
		[Own]
		public int StateDuration { get ; protected set ; }


		[CanBeNull]
		[Own]
		public GameDate StateStartDate { get ; protected set ; }

		#endregion


		#region Get Buff

		[CanBeNull]
		public void AddBuff ( [NotNull] PlayerBuff newBuff )
		{
			if ( newBuff == null )
			{
				throw new ArgumentNullException ( nameof(newBuff) ) ;
			}

			Buffs . Add ( newBuff ) ;
			GetBuffEvent ? . Invoke ( this , new PlayerGetBuffEventArgs ( newBuff ) ) ;
		}

		[CanBeNull]
		public event EventHandler <PlayerGetBuffEventArgs> GetBuffEvent ;

		#endregion

		#region ReceiveStock

		public void ReceiveStock ( [NotNull] Stock stock , int number )
		{
			if ( stock == null )
			{
				throw new ArgumentNullException ( nameof(stock) ) ;
			}

			if ( Stocks . ContainsKey ( stock ) )
			{
				Stocks [ stock ] += number ;
			}
			else
			{
				Stocks . Add ( stock , number ) ;
			}
			ReceiveStockEvent ? . Invoke ( this , new PlayerReceiveStockEventArgs ( stock , number ) ) ;
		}

		[CanBeNull]
		public event EventHandler <PlayerReceiveStockEventArgs> ReceiveStockEvent ;

		#endregion

		#region Cards

		/// <summary>
		///     玩家的卡片
		/// </summary>
		[NotNull]
		[ItemNotNull]
		[Own]
		public List <Card> Cards { get ; } = new List <Card> ( ) ;


		/// <summary>
		///     将某张卡片给某个玩家
		/// </summary>
		/// <param name="target"></param>
		public void GiveCard ( [NotNull] Card card , [NotNull] Player target )
		{
			#region Check Argument

			if ( card == null )
			{
				throw new ArgumentNullException ( nameof(card) ) ;
			}

			if ( ! Cards . Contains ( card ) )
			{
				throw new ArgumentException ( $"this player do not have {nameof(card)}" , nameof(card) ) ;
			}
			if ( target == null )
			{
				throw new ArgumentNullException ( nameof(target) ) ;
			}
			if ( target == this )
			{
				throw new ArgumentException ( $"{nameof(target)} can not be self" , nameof(target) ) ;
			}

			#endregion

			Cards . Remove ( card ) ;

			target . ReceiveCard ( card , this ) ;

			GiveCardEvent ? . Invoke ( this , new PlayerGiveCardEventArgs ( card , target ) ) ;
		}

		[CanBeNull]
		public event EventHandler <PlayerGiveCardEventArgs> GiveCardEvent ;

		public void ReceiveCard ( [NotNull] Card card , [NotNull] Player source )
		{
			#region Check Argument

			if ( card == null )
			{
				throw new ArgumentNullException ( nameof(card) ) ;
			}

			if ( Cards . Contains ( card ) )
			{
				throw new ArgumentException ( $"this player have {nameof(card)}" , nameof(card) ) ;
			}
			if ( source == null )
			{
				throw new ArgumentNullException ( nameof(source) ) ;
			}
			if ( source == this )
			{
				throw new ArgumentException ( $"{nameof(source)} can not be self" , nameof(source) ) ;
			}

			#endregion

			Cards . Add ( card ) ;
			ReceiveCardEvent ? . Invoke ( this , new PlayerReceiveCardEventArgs ( card , source ) ) ;
		}

		[CanBeNull]
		public event EventHandler <PlayerReceiveCardEventArgs> ReceiveCardEvent ;


		public override void ReceiveTransactionRequest ( AssetTransactionAgreement request )
		{
			throw new NotImplementedException ( ) ;
		}

		public override IEnumerable <IAsset> Assets { get ; }

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

		#endregion

	}

	public enum HarmType
	{

		Physics ,

		mind , //rename

		chem ,

		ridu

	}

	public enum PayType
	{

		Cash ,

		Check ,

		Transfer

	}

}
