﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . Richman4L . Annotations ;

namespace WenceyWang . Richman4L . Interoperability . Arguments . DefineDomains
{

	public abstract class ArgumentValueDefineDomain
	{

		public abstract bool IsValid ( [NotNull] object value ) ;

	}

	public abstract class ArgumentValueDefineDomain <T> : ArgumentValueDefineDomain
	{

		public sealed override bool IsValid ( [NotNull] object value ) { return value is T val && IsValid ( val ) ; }

		public abstract bool IsValid ( T value ) ;

	}

}
