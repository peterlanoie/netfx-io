using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.IO
{

	/// <summary>
	/// Provides additional static helper methods for directory operations.
	/// </summary>
	public static class DirectoryHelper
	{
		/// <summary>
		/// Returns the names of files in the specified directory that match any of the specified
		/// search patterns, using a value to determine whether to search subdirectories.
		/// </summary>
		/// <param name="path">The directory to search.</param>
		/// <param name="option">Search option.</param>
		/// <param name="searchPatterns">
		///   One or more search strings to match against the names of files in path. The parameter
		///   cannot end in two periods ("..") or contain two periods ("..") followed by
		///   System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
		///   nor can it contain any of the characters in System.IO.Path.InvalidPathChars.
		/// </param>
		/// <returns>
		///   A String array containing the names of files in the specified directory that
		///   match any of the specified search patterns. File names include the full path.
		/// </returns>
		public static string[] GetFiles(string path, SearchOption option, params string[] searchPatterns)
		{
			List<string> files = new List<string>();
			searchPatterns.ToList().ForEach(x => files.AddRange(Directory.GetFiles(path, x, option)));
			return files.Distinct().ToArray();
		}

		/// <summary>
		/// Resets all files to attribute settings of "Normal".
		/// Useful for clearing attribute flags that prevent deletion (e.g. readonly).
		/// </summary>
		/// <param name="directory">Directory to process.</param>
		/// <param name="option">File search option.</param>
		public static void ResetAllAttributes(string directory, SearchOption option = SearchOption.AllDirectories)
		{
			var files = DirectoryHelper.GetFiles(directory, option, "*");
			files.ToList().ForEach(x => File.SetAttributes(x, FileAttributes.Normal));
		}
	}
}
