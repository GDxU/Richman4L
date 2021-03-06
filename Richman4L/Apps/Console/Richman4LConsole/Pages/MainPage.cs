﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole ;
using WenceyWang . FoggyConsole . Controls ;
using WenceyWang . Richman4L . Apps . Console . Fonts ;
using WenceyWang . Richman4L . Logics ;

namespace WenceyWang . Richman4L . Apps . Console . Pages
{

	public class MainPage : Page
	{

		private Canvas ContentCanvas { get ; } = new Canvas ( ) ;

		public FIGletLabel GameTitleLabel { get ; } = new FIGletLabel ( ) ;

		public Button NewGameButton { get ; } = new Button ( ) ;

		public Button LoadGameButton { get ; set ; } = new Button ( ) ;

		public Button SettingButton { get ; set ; } = new Button ( ) ;


		public string CurrentGameTitle { get => GameTitleLabel . Text ; private set => GameTitleLabel . Text = value ; }

		public MainPage ( )
		{
			FIGletLabel fLabel = new FIGletLabel
								{
									Text = "Richman4L" ,
									Font = FontsHelper . LoadFont ( "graffiti" ) ,
									ForegroundColor = ConsoleColor . Yellow ,
									BackgroundColor = ConsoleColor . DarkGreen
								} ;
		}


		public void UpdateContent ( ) { }

		public override void OnNavigateTo ( )
		{
			if ( Program . Current . Setting . AllowRandomTitle )
			{
				CurrentGameTitle = GameTitle . GetTitle ( Program . Current . Setting . AllowRandomTitleRoot ) . Content ;
			}
			else
			{
				CurrentGameTitle = GameTitle . Defult . Content ;
			}
			Application . Current . Stop ( ) ;
		}

		public override void Arrange ( Rectangle finalRect ) { base . Arrange ( finalRect ) ; }

		public override void Measure ( Size availableSize ) { base . Measure ( availableSize ) ; }

	}

}
