using System;
using System.Collections.Generic;
using Common.IO.CompareSync.Filters;

namespace Common.IO.CompareSync
{

	/// <summary>
	/// Provides file synchronization functionality.
	/// </summary>
	public static class DirectorySynchronizer
	{

		/// <summary>
		/// Mirrors the contents of <paramref name="sourceDir"/> from <paramref name="targetDir"/>.
		/// </summary>
		/// <param name="sourceDir">The directory from which files will be copied.</param>
		/// <param name="targetDir">The directory to which files will be copied and/or deleted.</param>
		public static void MirrorDirectory(string sourceDir, string targetDir)
		{
			MirrorDirectory(sourceDir, targetDir, null, null, null, false);
		}

		/// <summary>
		/// Mirrors the contents of <paramref name="sourceDir"/> from <paramref name="targetDir"/>.
		/// </summary>
		/// <param name="sourceDir">The directory from which files will be copied.</param>
		/// <param name="targetDir">The directory to which files will be copied and/or deleted.</param>
		/// <param name="test">Whether or not to run in test mode. Everything happens the same except files are actually copied to and/or deleted from the target directory.</param>
		public static void MirrorDirectory(string sourceDir, string targetDir, bool test)
		{
			MirrorDirectory(sourceDir, targetDir, null, null, null, test);
		}

		/// <summary>
		/// Mirrors the contents of <paramref name="sourceDir"/> from <paramref name="targetDir"/>.
		/// Calls the <paramref name="messageCallback"/> with messages of what did/will happen.
		/// </summary>
		/// <param name="sourceDir">The directory from which files will be copied.</param>
		/// <param name="targetDir">The directory to which files will be copied and/or deleted.</param>
		/// <param name="files">Optional list of files to include in the mirror. If empty or null, all files will be processed. Can be null.</param>
		/// <param name="exclusions">Optional list of file or directory exclusions. Can be null.</param>
		/// <param name="messageCallback">Optional callback for messaging. Can be null.</param>
		/// <param name="test">Whether or not to run in test mode. Everything happens the same except files are actually copied to and/or deleted from the target directory.</param>
		public static void MirrorDirectory(string sourceDir, string targetDir, IEnumerable<string> files, FilterList exclusions, CompareSyncMessageDelegate messageCallback, bool test)
		{
			Action<CompareSyncWorker> action;
			if(test)
			{
				action = delegate(CompareSyncWorker w) { w.TestMirrorDir(); };
			}
			else
			{
				action = delegate(CompareSyncWorker w) { w.MirrorDir(); };
			}

			CompareSyncWorker worker = new CompareSyncWorker(sourceDir, targetDir);
			if(files != null)
			{
				worker.Files = files;
			}
			if(exclusions != null)
			{
				worker.Exclusions = exclusions;
			}
			if(messageCallback != null)
			{
				worker.SyncActionOccurred += messageCallback;
			}
			action(worker);
		}

	}
}
