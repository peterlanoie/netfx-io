using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.IO.CompareSync.Filters
{
	/// <summary>
	/// Defines a filter for a file in any directory.
	/// </summary>
	public class GlobalFileFilter : Filter
	{
		internal static Filter Create(string filterValue)
		{
			return new GlobalFileFilter() { Value = filterValue };
		}

		internal override bool IsMatch(string path)
		{
			return Path.GetFileName(path).ToLower() == Value.ToLower();
		}
	}
}
