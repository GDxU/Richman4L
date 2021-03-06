﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using WenceyWang . Richman4L . Logics . Cards ;

namespace WenceyWang . Richman4L . Logics . Players . Events
{

	public class PlayerReceiveCardEventArgs : PlayerEventArgs
	{

		/// <summary>
		///     卡片
		/// </summary>
		[NotNull]
		public Card Card { get ; set ; }

		/// <summary>
		///     送出卡片的玩家
		/// </summary>
		[NotNull]
		public Player Source { get ; set ; }

		public PlayerReceiveCardEventArgs ( [NotNull] Card card , [NotNull] Player source )
		{
			Card = card ?? throw new ArgumentNullException ( nameof(card) ) ;
			Source = source ?? throw new ArgumentNullException ( nameof(source) ) ;
		}

	}

}
