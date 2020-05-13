using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.IO.CompareSync.Filters
{
	/// <summary>
	/// Defines a filter for a directory in any part of a path.
	/// </summary>
	public class GlobalDirectoryFilter : Filter
	{
		internal static Filter Create(string filterValue)
		{
			return new GlobalDirectoryFilter() { Value = filterValue };
		}

		internal override bool IsMatch(string path)
		{
			string[] parts = path.ToLower().Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
			return parts.Contains(this.Value.ToLower());
		}
	}
}
