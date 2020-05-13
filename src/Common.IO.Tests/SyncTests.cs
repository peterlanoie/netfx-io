using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Common.IO.CompareSync;
using System.Diagnostics;
using Common.IO.CompareSync.Filters;

namespace Common.IO.Tests
{
	[TestClass]
	public class SyncTests
	{
		CompareSyncMessageDelegate _msgHandler;
		string _projectRoot, _sampleDataRoot, _targetRoot;

		public SyncTests()
		{
			_msgHandler = new CompareSyncMessageDelegate(CompareSyncMessageHandler);
			_projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\.."));
			_sampleDataRoot = Path.Combine(_projectRoot, @"SampleData\DirMirror");
			_targetRoot = Path.Combine(_projectRoot, @"TestResults\DirMirror3");
		}

		[TestInitialize]
		public void Init()
		{
			if(Directory.Exists(_targetRoot))
			{
				foreach(var item in Directory.GetFiles(_targetRoot, "*.*", SearchOption.AllDirectories))
				{
					File.SetAttributes(item, FileAttributes.Normal);
				}
				Directory.Delete(_targetRoot, true);
			}
		}

		[TestMethod]
		public void TestMirrorDirectory()
		{
			DirectorySynchronizer.MirrorDirectory(_sampleDataRoot, Path.Combine(_targetRoot, "MirrorTest"), null, null, _msgHandler, true);
		}

		[TestMethod]
		public void MirrorDirectory()
		{
			DirectorySynchronizer.MirrorDirectory(_sampleDataRoot, Path.Combine(_targetRoot, "MirrorTest"), null, null, _msgHandler, false);
		}

		[TestMethod]
		public void MirrorDirectoryWithFilters()
		{
			FilterList exclusions = new FilterList();
			string targetDir = Path.Combine(_targetRoot, "FilterTest");

			exclusions.AddRelativeFileFilter(@"folder.04\folder.05\file.09.exclude.relative");
			exclusions.AddRelativeDirFilter("folder.06.exclude.relative");
			exclusions.AddGlobalDirFilter("folder.10.exclude.global");
			exclusions.AddRelativeFileFilter(@"file.07.exclude.relative");
			exclusions.AddGlobalFileFilter(@"file.08.exclude.global");
			DirectorySynchronizer.MirrorDirectory(_sampleDataRoot, targetDir, null, exclusions, _msgHandler, false);

			// These files aren't filtered and SHOULD exist
			Assert.IsTrue(RightFileExist(targetDir, @"folder.01\folder.02\folder.03\file.05"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.01\folder.02\folder.03\file.06"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.01\folder.02\file.03"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.01\folder.02\file.04"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.04\folder.05\file.11"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.11\folder.12\file.12"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.11\folder.13\file.13"));
			Assert.IsTrue(RightFileExist(targetDir, @"file.01"));
			Assert.IsTrue(RightFileExist(targetDir, @"file.02"));

			// This file NAME matches a relative file filter, but it's in a sub folder
			// therefore it should exist
			Assert.IsTrue(RightFileExist(targetDir, @"folder.09\file.07.exclude.relative"));

			// This file is in a FOLDER who's NAME matches that of a folder in a RELATIVE filter
			// and should thus still get copied
			Assert.IsTrue(RightFileExist(targetDir, @"folder.11\folder.06.exclude.relative\file.06"));

			// These are files that are somehow match one of the 4 filters
			Assert.IsFalse(RightFileExist(targetDir, @"folder.10.exclude.global\file.10"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.06.exclude.relative\file.06"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.04\folder.05\file.09.exclude.relative"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.04\folder.05\file.08.exclude.global"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.04\file.08.exclude.global"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.11\folder.10.exclude.global\file.10"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.11\folder.12\folder.10.exclude.global\file.10"));
			Assert.IsFalse(RightFileExist(targetDir, @"file.08.exclude.global"));
			Assert.IsFalse(RightFileExist(targetDir, @"file.07.exclude.relative"));
		}

		[TestMethod]
		public void MirrorDirectoryWithFileList()
		{
			string targetDir = Path.Combine(_targetRoot, "FileListTest");
			string[] files;
			files = new string[]{
				@"folder.01\folder.02\folder.03\file.05",
				@"folder.01\folder.02\folder.03\file.06",
				@"folder.01\folder.02\file.03",
				@"folder.04\folder.05\file.11",
				@"file.01"
			};

			DirectorySynchronizer.MirrorDirectory(_sampleDataRoot, targetDir, files, null, _msgHandler, false);

			// These files SHOULD exist
			Assert.IsTrue(RightFileExist(targetDir, @"folder.01\folder.02\folder.03\file.05"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.01\folder.02\folder.03\file.06"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.01\folder.02\file.03"));
			Assert.IsTrue(RightFileExist(targetDir, @"folder.04\folder.05\file.11"));
			Assert.IsTrue(RightFileExist(targetDir, @"file.01"));

			// These files SHOULD NOT exist
			Assert.IsFalse(RightFileExist(targetDir, @"folder.01\folder.02\file.04"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.11\folder.12\file.12"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.11\folder.13\file.13"));
			Assert.IsFalse(RightFileExist(targetDir, @"file.02"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.09\file.07.exclude.relative"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.11\folder.06.exclude.relative\file.06"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.10.exclude.global\file.10"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.06.exclude.relative\file.06"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.04\folder.05\file.09.exclude.relative"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.04\folder.05\file.08.exclude.global"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.04\file.08.exclude.global"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.11\folder.10.exclude.global\file.10"));
			Assert.IsFalse(RightFileExist(targetDir, @"folder.11\folder.12\folder.10.exclude.global\file.10"));
			Assert.IsFalse(RightFileExist(targetDir, @"file.08.exclude.global"));
			Assert.IsFalse(RightFileExist(targetDir, @"file.07.exclude.relative"));
		}

		private bool RightFileExist(string targetDir, string path)
		{
			return File.Exists(Path.Combine(targetDir, path));
		}

		private void CompareSyncMessageHandler(object sender, CompareSyncMessageEventArgs e)
		{
			Debug.Print(string.Format("{0} ==> {1}", e.Item.RelativePath, e.Item.RightCompareRootPath));
		}

	}
}
