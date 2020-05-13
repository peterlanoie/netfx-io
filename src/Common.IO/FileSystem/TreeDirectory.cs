using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.IO.FileSystem
{
	/// <summary>
	/// Represents a directory in a file system tree.
	/// </summary>
	[Serializable]
	public class TreeDirectory : TreeEntry
	{
		/// <summary>
		/// Gets the directory info this this entry.
		/// </summary>
		public DirectoryInfo DirInfo {
			get
			{
				return (DirectoryInfo)this.Info;
			}
		}

		private List<TreeDirectory> _directories;
		private List<TreeFile> _files;

		private Func<string, bool> _includeChildPredicate;

		/// <summary>
		/// Gets the list of directories in this directory.
		/// </summary>
		public List<TreeDirectory> Directories {
			get
			{
				if(_directories == null)
				{
					_directories = PopulateItemList<TreeDirectory>(Directory.GetDirectories(this.DirInfo.FullName), s => new TreeDirectory(s, this, _includeChildPredicate));
				}
				return _directories;
			}
		}

		/// <summary>
		/// Gets the list of files in this directory.
		/// </summary>
		public List<TreeFile> Files
		{
			get
			{
				if(_files == null)
				{
					_files = PopulateItemList<TreeFile>(Directory.GetFiles(this.DirInfo.FullName), s => new TreeFile(s, this));
				}
				return _files;
			}
		}

		private List<T> PopulateItemList<T>(string[] entries, Func<string, T> selector)
		{
			List<T> lstItems = new List<T>();
			lstItems = entries.ToList().Where(_includeChildPredicate).Select(s => selector(s)).ToList();
			return lstItems;
		}

		private TreeDirectory(string path, TreeEntry parent, Func<string, bool> includeChildPredicate)
			: base(new DirectoryInfo(path), parent)
		{
			_includeChildPredicate = includeChildPredicate;
		}

		/// <summary>
		/// Creates a new instance of the directory item anchored at the <paramref name="rootPath"/>
		/// </summary>
		/// <param name="rootPath"></param>
		/// <returns></returns>
		public static TreeDirectory CreateTree(string rootPath)
		{
			return CreateTree(rootPath, s => true);
		}

		/// <summary>
		/// Creates a new instance of the directory item anchored at the <paramref name="rootPath"/> 
		/// with the specified <paramref name="includeChildPredicate"/> used to test each child item for inclusion.
		/// </summary>
		/// <param name="rootPath">The root path of the directory.</param>
		/// <param name="includeChildPredicate">Predicate method used to test each child item for inclusion.</param>
		/// <returns></returns>
		public static TreeDirectory CreateTree(string rootPath, Func<string, bool> includeChildPredicate)
		{
			TreeDirectory objDirRoot = new TreeDirectory(rootPath, null, includeChildPredicate);

			return objDirRoot;
		}

		/// <summary>
		/// Gets the directories items as a flat list.
		/// </summary>
		/// <returns></returns>
		public List<TreeEntry> GetFlatEntryList()
		{
			List<TreeEntry> lstEntries = new List<TreeEntry>();
			AddChildEntries(lstEntries, this);
			return lstEntries;
		}

		private void AddChildEntries(List<TreeEntry> list, TreeDirectory dir)
		{
			list.Add(dir);
			foreach(TreeDirectory childDir in dir.Directories)
			{
				AddChildEntries(list, childDir);
			}
			foreach(TreeFile file in dir.Files)
			{
				list.Add(file);
			}
		}

	}
}
