using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Common.IO.CompareSync.Filters
{
	/// <summary>
	/// Defines the base type of a filter.
	/// </summary>
	public abstract class Filter
	{
		/// <summary>
		/// The value of the filter.
		/// </summary>
		public string Value { get; set; }

		internal abstract bool IsMatch(string path);

		/// <summary>
		/// Creates a Filter instance based on the supplied filter syntax value.
		/// </summary>
		/// <param name="value">A value in the format of filter syntax.</param>
		/// <returns></returns>
		public static Filter FromString(string value)
		{
			bool isRelative = value.StartsWith(string.Format(".{0}", Path.DirectorySeparatorChar));
			bool isDirectory = value.EndsWith(Path.DirectorySeparatorChar.ToString());

			string filterValue = StripIdentifiers(value);

			if(isDirectory)
			{
				if(isRelative)
				{
					return RelativeDirectoryFilter.Create(filterValue);
				}
				else
				{
					return GlobalDirectoryFilter.Create(filterValue);
				}
			}
			else
			{
				if(isRelative)
				{
					return RelativeFileFilter.Create(filterValue);
				}
				return GlobalFileFilter.Create(filterValue);
			}
		}

		private static string StripIdentifiers(string value)
		{
			string newValue = value;
			Regex rgxRooter = new Regex(@"^\.\\", RegexOptions.Compiled | RegexOptions.Singleline);
			Regex rgxDirer = new Regex(@"\\$", RegexOptions.Compiled | RegexOptions.Singleline);
			newValue = rgxRooter.Replace(newValue, "");
			newValue = rgxDirer.Replace(newValue, "");
			return newValue;
		}

		//private static bool IsRooted(string path)
		//{
		//    return 
		//}

		//private static bool IsDirectory(string path)
		//{
		//    return 
		//}

	}

	/// <summary>
	/// Defines a list of filters.
	/// </summary>
	public class FilterList : List<Filter>
	{
		/// <summary>
		/// Whether or not the supplied path matches any of the filters in the list.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool MatchesAny(string path)
		{
			return this.Any(f => f.IsMatch(path));
		}

		/// <summary>
		/// Adds a filter for a root relative directory.
		/// </summary>
		/// <param name="relativeDirPath"></param>
		public void AddRelativeDirFilter(string relativeDirPath)
		{
			this.Add(RelativeDirectoryFilter.Create(relativeDirPath));
		}

		/// <summary>
		/// Adds a filter for a global directory name.
		/// </summary>
		/// <param name="dirName"></param>
		public void AddGlobalDirFilter(string dirName)
		{
			this.Add(GlobalDirectoryFilter.Create(dirName));
		}

		/// <summary>
		/// Adds a filter for a root relative file path.
		/// </summary>
		/// <param name="relativeFilePath"></param>
		public void AddRelativeFileFilter(string relativeFilePath)
		{
			this.Add(RelativeFileFilter.Create(relativeFilePath));
		}

		/// <summary>
		/// Adds a filter for a global file name.
		/// </summary>
		/// <param name="fileName"></param>
		public void AddGlobalFileFilter(string fileName)
		{
			this.Add(GlobalFileFilter.Create(fileName));
		}

		//public bool MatchesAll(string path)
		//{
		//    return this.All(f => f.IsMatch(path));
		//}

		/// <summary>
		/// Creates a filter list from a semicolon separated filter syntax string.
		/// </summary>
		/// <param name="filterList"></param>
		/// <returns></returns>
		public static FilterList FromString(string filterList)
		{
			FilterList lstFilters = new FilterList();
			string[] filters = filterList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			foreach(var item in filters)
			{
				lstFilters.Add(Filter.FromString(item));
			}
			return lstFilters;
		}

	}

}
