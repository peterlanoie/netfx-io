using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync.Filters
{
	/// <summary>
	/// Defines a filter for a file in a specific directory path.
	/// </summary>
	public class RelativeFileFilter : Filter
	{
		internal static Filter Create(string filterValue)
		{
			return new RelativeFileFilter() { Value = filterValue };
		}

		internal override bool IsMatch(string path)
		{
			return path.ToLower() == Value.ToLower();
		}
	}
}
