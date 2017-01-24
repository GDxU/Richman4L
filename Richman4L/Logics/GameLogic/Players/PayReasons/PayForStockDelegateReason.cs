using System ;
using System . Collections ;
using System . Linq ;

using WenceyWang . Richman4L . Properties ;
using WenceyWang . Richman4L . Stocks ;

namespace WenceyWang . Richman4L . Players .PayReasons
{

	public class PayForStockDelegateReason : PayReason
	{

		[NotNull]
		public StockDelegate StockDelegate { get ; }

		public PayForStockDelegateReason ( [NotNull] StockDelegate stockDelegate )
		{
			if ( stockDelegate == null )
			{
				throw new ArgumentNullException ( nameof ( stockDelegate ) ) ;
			}

			StockDelegate = stockDelegate ;
		}

	}

}