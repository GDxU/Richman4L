﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Threading . Tasks ;
using System . Windows ;
using System . Windows . Media ;
using System . Windows . Navigation ;

using WenceyWang . Richman4L . Apps . Wpf . Logic ;
using WenceyWang . Richman4L . Apps . Wpf . UI . Controls ;
using WenceyWang . Richman4L . Logics ;
using WenceyWang . Richman4L . Logics . GameEnviroment ;
using WenceyWang . Richman4L . Logics . Players . Models ;

namespace WenceyWang . Richman4L . Apps . Wpf . UI . Pages
{

	/// <summary>
	///     玩家配置页
	/// </summary>
	public sealed partial class PlayerConfigPage : AnimatePage
	{

		private StartGameParameters Parameters { get ; set ; }

		public static Color PageColor => XamlResources . Resources . Lime ;

		public PlayerConfigPage ( ) { InitializeComponent ( ) ; }

		protected override void OnNavigatedTo ( NavigationEventArgs e )
		{
			Parameters = e . Parameter as StartGameParameters ;
		}

		private void Page_Loaded ( object sender , RoutedEventArgs e )
		{
			if ( Parameters != null )
			{
				GenerateList ( ) ;
			}
			StartStoryboard . Begin ( ) ;
			StartStoryboard . Completed += StartStoryboard_Completed ;
		}

		private void StartStoryboard_Completed ( object sender , object e )
		{
			if ( AppSettings . Current . OcdMode )
			{
				MainGrid . TurnOnOcdMode ( ) ;
			}
			StartStoryboard . Completed -= StartStoryboard_Completed ;
		}

		private void CreateGamePageButton_Click ( object sender , RoutedEventArgs e )
		{
			this . NavigateTo <CreateGamePage> ( Parameters ) ;
		}

		private void StartGameButton_Click ( object sender , RoutedEventArgs e )
		{
			LoadingPageArgument parameters = new LoadingPageArgument ( Task . Run ( ( ) =>
																					{
																						Game . PeapareNew ( ) ;
																						Game . Current . Start ( Parameters ) ;
																					} ) ,
																		currentPage => { currentPage . NavigateTo <GamePage> ( ) ; } ) ;

			this . NavigateTo <LoadingPage> ( parameters ) ;
		}

		public override void RemoveControl ( )
		{
			CreateGamePageButton . Click -= CreateGamePageButton_Click ;
			StartGameButton . Click -= StartGameButton_Click ;
		}

		public override void AddControl ( )
		{
			CreateGamePageButton . Click += CreateGamePageButton_Click ;
			StartGameButton . Click += StartGameButton_Click ;
		}

		private void AddButton_Click ( object sender , RoutedEventArgs e )
		{
			List <PlayerModelProxy> modelList = PlayerModelProxy . GetPlayerModels ( ) ;


			PlayerConfigListItem toAdd = new PlayerConfigListItem ( ) ;
			toAdd . Margin = new Thickness ( 0 , 20 , 0 , 0 ) ;
			toAdd . Delete += PlayerConfigListItem_Delete ;
			toAdd . PlayerModelName = modelList . FirstOrDefault ( ) . Name ;

			PlayerConfigStackPanel . Children . Add ( toAdd ) ;
			NamePlayer ( ) ;
		}

		private void PlayerConfigListItem_Delete ( object sender , RoutedEventArgs e )
		{
			PlayerConfigStackPanel . Children . Remove ( sender as UIElement ) ;
			NamePlayer ( ) ;
		}

		private void NamePlayer ( )
		{
			int number = 1 ;
			foreach ( UIElement item in PlayerConfigStackPanel . Children )
			{
				if ( item is PlayerConfigListItem )
				{
					( item as PlayerConfigListItem ) . PlayerName = "玩家 " + number ;
					number++ ;
				}
			}
		}

		private void GenerateParameters ( )
		{
			Parameters . PlayerConfig = new List <Tuple <PlayerModelProxy , PlayerConsole>> ( ) ;
			foreach ( UIElement item in PlayerConfigStackPanel . Children )
			{
				if ( item is PlayerConfigListItem )
				{
					//Todo:PlayerConsole
					//Parameters . PlayerConfig . Add ( new Tuple<PlayerModelProxy , Players . PlayerConsole> ( ( item as PlayerConfigListItem ) . PlayerModel    ) ) );
				}
			}
		}

		private void GenerateList ( )
		{
			foreach ( Tuple <PlayerModelProxy , PlayerConsole> item in Parameters . PlayerConfig )
			{
				PlayerConfigListItem toAdd = new PlayerConfigListItem ( ) ;
				toAdd . PlayerModelName = item . Item1 . Name ;
				toAdd . Controller = item . Item2 . ToString ( ) ;
				PlayerConfigStackPanel . Children . Add ( toAdd ) ;
			}

			NamePlayer ( ) ;
		}

	}

}
