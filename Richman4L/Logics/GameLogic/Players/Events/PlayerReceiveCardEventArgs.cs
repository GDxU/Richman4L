﻿/*
* Richman4L: A free game with a rule like Richman4Fun.
* Copyright (C) 2010-2016 Wencey Wang
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;

using WenceyWang . Richman4L . Cards;
using WenceyWang . Richman4L . Properties;

namespace WenceyWang . Richman4L . Players . Events
{

	public class PlayerReceiveCardEventArgs : PlayerEventArgs
	{
		/// <summary>
		/// 卡片
		/// </summary>
		[NotNull]
		public Card Card { get; set; }

		/// <summary>
		/// 送出卡片的玩家
		/// </summary>
		[NotNull]
		public Player Source { get; set; }

		public PlayerReceiveCardEventArgs ( [NotNull] Card card , [NotNull] Player source )
		{
			if ( card == null )
			{
				throw new ArgumentNullException ( nameof ( card ) );
			}
			if ( source == null )
			{
				throw new ArgumentNullException ( nameof ( source ) );
			}

			Card = card;
			Source = source;
		}

	}

}