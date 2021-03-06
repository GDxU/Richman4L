﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . Richman4L . Logics . Stocks
{

	public static class StockBuffExtensions
	{

		public static bool IsBlockBuy ( this Stock stock )
		{
			if ( stock == null )
			{
				throw new ArgumentNullException ( nameof(stock) ) ;
			}

			return stock . Buffs . Any ( item => item . BlockBuy ) ;
		}

		public static bool IsBlockSell ( this Stock stock )
		{
			if ( stock == null )
			{
				throw new ArgumentNullException ( nameof(stock) ) ;
			}

			return stock . Buffs . Any ( item => item . BlockSell ) ;
		}

	}

}
