﻿using System ;
using System . Collections ;
using System . Linq ;

using Microsoft . VisualStudio . TestTools . UnitTesting ;

namespace WenceyWang . Richman4L .UnitTests
{

	[TestClass]
	public class StartUpTest
	{

		[TestMethod]
		public void GetTaskTest ( ) { Startup . RunAllTask ( ) ; }

		[TestMethod]
		public void RunTaskTest ( ) { Startup . RunAllTask ( ) . Wait ( ) ; }

	}

}