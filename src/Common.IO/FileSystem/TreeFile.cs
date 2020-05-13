using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.IO.FileSystem
{
	/// <summary>
	/// Represents a file entry in a file system tree.
	/// </summary>
	[Serializable]
	public class TreeFile : TreeEntry
	{
		/// <summary>
		/// Returns the file info for this file.
		/// </summary>
		public FileInfo FileInfo
		{
			get
			{
				return (FileInfo)this.Info;
			}
		}

		internal TreeFile(string filename, TreeEntry parent)
			: base(new FileInfo(filename), parent)
		{
		}

	}
}
