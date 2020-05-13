using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Common.IO.CompareSync
{
	/// <summary>
	/// Provides file comparison logic.
	/// </summary>
	public static class FileComparer
	{

		/// <summary>
		/// Determines whether the two files are the same, different or either is missing.
		/// </summary>
		/// <param name="leftFile">Left file to compare.</param>
		/// <param name="rightFile">Right file to compare.</param>
		/// <returns></returns>
		public static CompareStateType CompareFiles(string leftFile, string rightFile)
		{
			if(File.Exists(leftFile) & !File.Exists(rightFile) )
			{
				return CompareStateType.LeftOrphan;
			}
			if(!File.Exists(leftFile) & File.Exists(rightFile))
			{
				return CompareStateType.RightOrphan;
			}

			return FileBytesMatch(leftFile, rightFile) ? CompareStateType.Match : CompareStateType.Different;
		}
		
		//Borrowed from MSDN: http://support.microsoft.com/kb/320348
		/// <summary>
		/// Determines whether or not the bytes of two files are the same.
		/// </summary>
		/// <param name="leftFile">Left file to compare.</param>
		/// <param name="rightFile">Right file to compare.</param>
		/// <returns></returns>
		public static bool FileBytesMatch(string leftFile, string rightFile)
		{
			int leftFileByte;
			int rightFileByte;
			FileStream leftFS;
			FileStream rightFS;

			// Determine if the same file was referenced two times.
			if(leftFile == rightFile)
			{
				// Return true to indicate that the files are the same.
				return true;
			}

			// Open the two files.
			leftFS = new FileStream(leftFile, FileMode.Open, FileAccess.Read);
			rightFS = new FileStream(rightFile, FileMode.Open, FileAccess.Read);

			// Check the file sizes. If they are not the same, the files 
			// are not the same.
			if(leftFS.Length != rightFS.Length)
			{
				// Close the file
				leftFS.Close();
				rightFS.Close();

				// Return false to indicate files are different
				return false;
			}

			// Read and compare a byte from each file until either a
			// non-matching set of bytes is found or until the end of
			// file1 is reached.
			do
			{
				// Read one byte from each file.
				leftFileByte = leftFS.ReadByte();
				rightFileByte = rightFS.ReadByte();
			}
			while((leftFileByte == rightFileByte) && (leftFileByte != -1));

			// Close the files.
			leftFS.Close();
			rightFS.Close();

			// Return the success of the comparison. "file1byte" is 
			// equal to "file2byte" at this point only if the files are 
			// the same.
			return ((leftFileByte - rightFileByte) == 0);
		}

	}
}
