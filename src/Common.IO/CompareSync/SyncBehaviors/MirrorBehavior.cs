using System.IO;

namespace Common.IO.CompareSync.SyncBehaviors
{
	class MirrorBehavior : SyncBehavior
	{
		protected override CompareSyncMessageEventArgs DoBehaviorAction(CompareItem item)
		{
			CompareSyncMessageEventArgs result = new CompareSyncMessageEventArgs();
			switch(item.CompareState)
			{
				case CompareStateType.Different:
				case CompareStateType.LeftOrphan:
					string dir = Path.GetDirectoryName(Path.Combine(this.Worker.RightCompareRootPath, item.RelativePath));
					if(!Directory.Exists(dir))
					{
						Directory.CreateDirectory(dir);
					}
					File.Copy(item.Left.FullPath, item.Right.FullPath, true);
					result.SyncActionText = string.Format("'{0}' copied to '{1}'", item.RelativePath, item.RightCompareRootPath);
					result.SyncAction = SyncActionType.Copied;
					break;
				case CompareStateType.RightOrphan:
					File.Delete(item.Right.FullPath);
					result.SyncActionText = string.Format("'{0}' deleted from '{1}'", item.RelativePath, item.RightCompareRootPath);
					result.SyncAction = SyncActionType.Deleted;
					break;
			}
			return result;
		}

		public MirrorBehavior(CompareSyncWorker worker)
			: base(worker)
		{

		}
	}
}
