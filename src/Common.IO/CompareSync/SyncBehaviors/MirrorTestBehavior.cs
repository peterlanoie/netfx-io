
namespace Common.IO.CompareSync.SyncBehaviors
{
	class MirrorTestBehavior : SyncBehavior
	{
		protected override CompareSyncMessageEventArgs DoBehaviorAction(CompareItem item)
		{
			CompareSyncMessageEventArgs result = new CompareSyncMessageEventArgs();
			switch(item.CompareState)
			{
				case CompareStateType.Different:
				case CompareStateType.LeftOrphan:
					result.SyncActionText = string.Format("'{0}' will be copied to '{1}'", item.RelativePath, item.RightCompareRootPath);
					result.SyncAction = SyncActionType.Copied;
					break;
				case CompareStateType.RightOrphan:
					result.SyncActionText = string.Format("'{0}' will be deleted from '{1}'", item.RelativePath, item.RightCompareRootPath);
					result.SyncAction = SyncActionType.Deleted;
					break;
			}
			return result;
		}

		public MirrorTestBehavior(CompareSyncWorker worker)
			: base(worker)
		{
		}
	}
}
