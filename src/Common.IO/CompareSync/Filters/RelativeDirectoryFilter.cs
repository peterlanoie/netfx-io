using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync.Filters
{
	/// <summary>
	/// Defines a filter for a directory at the root of a path.
	/// </summary>
	public class RelativeDirectoryFilter : Filter
	{
		internal static Filter Create(string filterValue)
		{
			return new RelativeDirectoryFilter() { Value = filterValue };
		}

		internal override bool IsMatch(string path)
		{
			return path.ToLower().StartsWith(this.Value.ToLower());
		}
	}
}
