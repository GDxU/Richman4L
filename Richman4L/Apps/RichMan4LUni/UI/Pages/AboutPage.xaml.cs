﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Windows . Foundation . Metadata ;
using Windows . Phone . UI . Input ;
using Windows . UI ;
using Windows . UI . Xaml ;
using Windows . UI . Xaml . Navigation ;

using WenceyWang . Richman4L . Apps . Uni . Logic ;

namespace WenceyWang . Richman4L . Apps . Uni . UI . Pages
{

	/// <summary>
	///     关于页。
	/// </summary>
	public sealed partial class AboutPage : AnimatePage
	{

		public static Color PageColor => XamlResources . Resources . Blue ;

		public AboutPage ( )
		{
			InitializeComponent ( ) ;
			StartStoryboard . Completed += StartStoryboardCompleted ;
		}

		protected override void OnNavigatedTo ( NavigationEventArgs e ) { }

		private void Page_Loaded ( object sender , RoutedEventArgs e ) { StartStoryboard . Begin ( ) ; }

		private void StartStoryboardCompleted ( object sender , object e )
		{
			if ( AppSettings . Current . OcdMode )
			{
				MainGrid . TurnOnOcdMode ( ) ;
			}
			StartStoryboard . Completed -= StartStoryboardCompleted ;
		}


		private void SettingPageButton_Click ( object sender , object e )
		{
			SetEventArgsHandled ( e ) ;
			this . NavigateTo <SettingPage> ( ) ;
		}

		public override void RemoveControl ( )
		{
			if ( ApiInformation . IsEventPresent ( "Windows.Phone.UI.Input.HardwareButtons" ,
													nameof(HardwareButtons . BackPressed) ) )
			{
				HardwareButtons . BackPressed -= SettingPageButton_Click ;
			}
			SettingPageButton . Click -= SettingPageButton_Click ;
		}

		public override void AddControl ( )
		{
			if ( ApiInformation . IsEventPresent ( "Windows.Phone.UI.Input.HardwareButtons" ,
													nameof(HardwareButtons . BackPressed) ) )
			{
				HardwareButtons . BackPressed += SettingPageButton_Click ;
			}
			SettingPageButton . Click += SettingPageButton_Click ;
		}

	}

}
