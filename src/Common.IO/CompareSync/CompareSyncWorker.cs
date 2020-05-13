using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.IO.CompareSync.Filters;
using Common.IO.CompareSync.SyncBehaviors;

namespace Common.IO.CompareSync
{
	internal class CompareSyncWorker
	{
		private SyncBehavior _syncBehavior;

		public FilterList Exclusions { get; set; }
		public string LeftCompareRootPath { get; set; }
		public string RightCompareRootPath { get; set; }
		public IEnumerable<string> Files { get; set; }

		public event CompareSyncMessageDelegate SyncActionOccurred;

		public CompareSyncWorker(string leftPath, string rightPath)
		{
			LeftCompareRootPath = leftPath;
			RightCompareRootPath = rightPath;
			Exclusions = new FilterList();
			_syncBehavior = new NoopBehavior(this);
		}

		//public void CompareDir()
		//{
		//    _syncBehavior = new NoopBehavior();
		//}

		public void TestMirrorDir()
		{
			_syncBehavior = new MirrorTestBehavior(this);
			DoWork();
		}

		public void MirrorDir()
		{
			_syncBehavior = new MirrorBehavior(this);
			DoWork();
		}

		private void DoWork()
		{
			List<string> lstLeftPaths, lstRightPaths;
			List<CompareItem> lstItems = new List<CompareItem>();
			List<ItemEntry> lstLeftItems, lstRightItems;
			CompareItem compItem;
			CompareSyncMessageEventArgs syncMessageArgs;
			bool blnSkipFile;

			lstLeftPaths = Directory.GetFiles(LeftCompareRootPath, "*.*", SearchOption.AllDirectories).ToList();
			if(Directory.Exists(RightCompareRootPath))
			{
				lstRightPaths = Directory.GetFiles(RightCompareRootPath, "*.*", SearchOption.AllDirectories).ToList();
			}
			else
			{
				lstRightPaths = new List<string>();
			}

			lstLeftItems = lstLeftPaths.ConvertAll<ItemEntry>(delegate(string path) { return new ItemEntry(LeftCompareRootPath, path); });
			lstRightItems = lstRightPaths.ConvertAll<ItemEntry>(delegate(string path) { return new ItemEntry(RightCompareRootPath, path); });

			// Build initial comparison pair list from left items
			foreach(ItemEntry leftItem in lstLeftItems)
			{
				compItem = new CompareItem();
				compItem.Left = leftItem;
				// Try to load the matching right item (since this is windows, we can lowerize the paths, case matters not ... for now.)
				compItem.Right = lstRightItems.SingleOrDefault(i => i.RelativePath.ToLower() == leftItem.RelativePath.ToLower());
				if(compItem.Right != null)
				{
					// Match found, remove from right list.
					lstRightItems.Remove(compItem.Right);
				}
				lstItems.Add(compItem);
			}

			// Put remaining items from the right list into the comparison pair list (as right-orphans)
			foreach(ItemEntry item in lstRightItems)
			{
				compItem = new CompareItem();
				compItem.Right = item;
				lstItems.Add(compItem);
			}

			// Set the IsFiltered flag for those items that match any exclusion filter
			lstItems.ForEach(i => i.IsFiltered = Exclusions.MatchesAny(i.RelativePath));

			//if(Files != null && Files.Length > 0)
			//{
			//    lstItems = lstItems.Where(i => Files.Contains(i.RelativePath)).ToList();
			//    //if(!Files.Contains(item.RelativePath))
			//    //{
			//    //    blnSkipFile = true;
			//    //}
			//}

			// For all comparison pairs 
			foreach(var item in lstItems)
			{
				blnSkipFile = false;

				if(Files != null && Files.Count() > 0)
				{
					if(!Files.Contains(item.RelativePath))
					{
						blnSkipFile = true;
					}
				}

				if(!blnSkipFile)
				{
					// Do comparison
					if(item.Left == null)
					{
						item.CompareState = CompareStateType.RightOrphan;
						item.Left = new ItemEntry(LeftCompareRootPath, Path.Combine(LeftCompareRootPath, item.Right.RelativePath));
					}
					else if(item.Right == null)
					{
						item.CompareState = CompareStateType.LeftOrphan;
						item.Right = new ItemEntry(RightCompareRootPath, Path.Combine(RightCompareRootPath, item.Left.RelativePath));
					}
					else
					{
						// If necessary, this basic comparison logic could be refactored using 
						// a behavior pattern to support different comparison algorithms.
						item.CompareState = FileComparer.CompareFiles(item.Left.FullPath, item.Right.FullPath);
					}

					// Call sync action work method
					if(Exclusions.MatchesAny(item.RelativePath))
					{
						syncMessageArgs = new CompareSyncMessageEventArgs()
						{
							SyncAction = SyncActionType.Ignored,
							SyncActionText = "Item skipped due to filter."
						};
					}
					else
					{
						syncMessageArgs = _syncBehavior.SyncItem(item);
					}
					syncMessageArgs.Item = item;
					SendMessage(syncMessageArgs);
				}

			}

		}

		private void SendMessage(CompareSyncMessageEventArgs args)
		{
			if(SyncActionOccurred != null)
			{
				SyncActionOccurred(this, args);
			}
		}

	}
}
