using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync.SyncBehaviors
{
	internal abstract class SyncBehavior
	{

		public CompareSyncMessageEventArgs SyncItem(CompareItem item)
		{
			CompareSyncMessageEventArgs result = new CompareSyncMessageEventArgs();
			switch(item.CompareState)
			{
				case CompareStateType.Unknown:
				case CompareStateType.Match:
					result.SyncActionText = string.Format("No action taken on '{0}'", item.RelativePath);
					result.SyncAction = SyncActionType.None;
					break;
				case CompareStateType.LeftOrphan:
				case CompareStateType.RightOrphan:
				case CompareStateType.Different:
					result = DoBehaviorAction(item);
					break;
			}
			return result;
		}

		protected abstract CompareSyncMessageEventArgs DoBehaviorAction(CompareItem item);


		public CompareSyncWorker Worker { get; set; }

		public SyncBehavior(CompareSyncWorker worker)
		{
			Worker = worker;
		}
	}
}
