using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.IO.CompareSync;
using Common.IO.CompareSync.Filters;
using System.IO;

namespace Common.IO.Tests
{
	[TestClass]
	public class FilterTests
	{

		[TestMethod]
		public void CreateRootDirectoryFilter()
		{
			Filter filter = Filter.FromString(BuildSeparatedPath(".{0}test{0}"));
			Assert.IsInstanceOfType(filter, typeof(RelativeDirectoryFilter));
		}

		[TestMethod]
		public void CreateGlobalDirectoryFilter()
		{
			Filter filter = Filter.FromString(BuildSeparatedPath("test{0}"));
			Assert.IsInstanceOfType(filter, typeof(GlobalDirectoryFilter));
		}

		[TestMethod]
		public void CreateRootFileFilter()
		{
			Filter filter = Filter.FromString(BuildSeparatedPath(".{0}test.ext"));
			Assert.IsInstanceOfType(filter, typeof(RelativeFileFilter));
		}

		[TestMethod]
		public void CreateGlobalFileFilter()
		{
			Filter filter = Filter.FromString("test.ext");
			Assert.IsInstanceOfType(filter, typeof(GlobalFileFilter));
		}

		[TestMethod]
		public void CreateFiltersFromList()
		{
			FilterList filters = FilterList.FromString(BuildSeparatedPath(".{0}test{0};test{0};.{0}test.ext;test.ext"));
			Assert.AreEqual<int>(4, filters.Count);
			Assert.IsInstanceOfType(filters[0], typeof(RelativeDirectoryFilter));
			Assert.IsInstanceOfType(filters[1], typeof(GlobalDirectoryFilter));
			Assert.IsInstanceOfType(filters[2], typeof(RelativeFileFilter));
			Assert.IsInstanceOfType(filters[3], typeof(GlobalFileFilter));
		}


		private string BuildSeparatedPath(string format)
		{
			return string.Format(format, Path.DirectorySeparatorChar);
		}

	}
}
