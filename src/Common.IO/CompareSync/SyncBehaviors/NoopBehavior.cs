using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync.SyncBehaviors
{

	/// <summary>
	/// Defines a sync behavior that does nothing.
	/// </summary>
	class NoopBehavior : SyncBehavior
	{
		protected override CompareSyncMessageEventArgs DoBehaviorAction(CompareItem item)
		{
			return new CompareSyncMessageEventArgs();
		}

		public NoopBehavior(CompareSyncWorker worker)
			: base(worker)
		{
		}
	}
}
