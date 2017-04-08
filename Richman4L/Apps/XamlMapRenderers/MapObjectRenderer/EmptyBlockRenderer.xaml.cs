﻿using System ;
using System . Collections . Generic ;
using System . Linq ;

using Windows . Foundation ;
using Windows . UI . Xaml ;

using WenceyWang . Richman4L . Annotations ;
using WenceyWang . Richman4L . Maps ;

namespace WenceyWang . Richman4L . Apps . XamlMapRenderers . MapObjectRenderer
{

	public sealed partial class EmptyBlockRenderer : MapObjectRenderer , IMapObjectRenderer <EmptyBlock>
	{

		public override Size Size => new Size ( 100 , 50 ) ;

		public EmptyBlockRenderer ( ) { InitializeComponent ( ) ; }

		public static readonly DependencyProperty TargetProperty =
			DependencyProperty . Register ( nameof(Target) ,
											typeof ( EmptyBlock ) ,
											typeof ( EmptyBlockRenderer ) ,
											new PropertyMetadata ( null ) ) ;

		public EmptyBlock Target
		{
			get => ( EmptyBlock ) GetValue ( TargetProperty ) ;
			private set => SetValue ( TargetProperty , value ) ;
		}

		public void StartUp ( ) { }

		public void SetTarget ( [NotNull] EmptyBlock target )
		{
			if ( Target == null )
			{
				Target = target ;
				StartUp ( ) ;
			}
			else
			{
				throw new InvalidOperationException ( ) ;
			}
		}

		public void Update ( ) { }

		public override void Hide ( ) { }

		public override void Show ( ) { }

	}

}
