using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync
{
	/// <summary>
	/// Defines a comparison item entry.
	/// </summary>
	public class ItemEntry
	{
		/// <summary>
		/// The full path to the item.
		/// </summary>
		public string FullPath { get; set; }

		/// <summary>
		/// The path to the comparison root of the item.
		/// </summary>
		public string CompareRootPath { get; set; }

		/// <summary>
		/// The path to the item relative to the comparison root.
		/// This is used to match up an item from the other side of the comparison.
		/// </summary>
		public string RelativePath
		{
			get
			{
				return FullPath.Replace(CompareRootPath, "").TrimStart('\\');
			}
		}

		/// <summary>
		/// Creates a new instance of the item based on the comparison root and item's full path.
		/// </summary>
		/// <param name="compareRootPath">The root path of the comparison.</param>
		/// <param name="fullPath">The full path to the item.</param>
		public ItemEntry(string compareRootPath, string fullPath)
		{
			CompareRootPath = compareRootPath;
			FullPath = fullPath;
		}

		//public static ItemEntry CreateFromString(string path, string fullPath)
		//{
		//    return new ItemEntry(path, fullPath);
		//}

	}

	//public class ItemEntryList : List<ItemEntry>
	//{
	//    public ItemEntry FindByRelativePath(string path)
	//    {
	//        return this.SingleOrDefault(i => i.RelativePath == path);
	//    }
	//}

}
