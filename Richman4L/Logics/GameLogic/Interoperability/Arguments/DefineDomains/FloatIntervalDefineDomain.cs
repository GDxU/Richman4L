using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

namespace WenceyWang . Richman4L . Logics . Interoperability . Arguments . DefineDomains
{

	public sealed class FloatIntervalDefineDomain : ArgumentValueDefineDomain <double>
	{

		public double LeftEndpoint { get ; }

		public bool IsLeftClosed { get ; }


		public double RightEndpoint { get ; }

		public bool IsRightClosed { get ; }

		public FloatIntervalDefineDomain ( double leftEndpoint ,
											bool isLeftClosed ,
											double rightEndpoint ,
											bool isRightClosed )
		{
			LeftEndpoint = leftEndpoint ;
			IsLeftClosed = isLeftClosed ;
			RightEndpoint = rightEndpoint ;
			IsRightClosed = isRightClosed ;
		}


		public override bool IsValid ( [NotNull] double value )
		{
			try
			{
				double number = Convert . ToDouble ( value ) ;
				bool valid = true ;
				if ( IsLeftClosed )
				{
					valid &= number >= LeftEndpoint ;
				}
				else
				{
					valid &= number > LeftEndpoint ;
				}
				if ( IsRightClosed )
				{
					valid &= number <= RightEndpoint ;
				}
				else
				{
					valid &= number < RightEndpoint ;
				}
				return valid ;
			}
			catch ( Exception )
			{
				return false ;
			}
		}

	}

}
