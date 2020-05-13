using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync
{
	/// <summary>
	/// Defines the event arguments for a comparison sync action message.
	/// </summary>
	public class CompareSyncMessageEventArgs : EventArgs
	{
		/// <summary>
		/// The comparison item.
		/// </summary>
		public CompareItem Item { get; set; }

		/// <summary>
		/// What sync action has been taken on the item.
		/// </summary>
		public SyncActionType SyncAction { get; set; }

		/// <summary>
		/// Test description of the action taken by the sync operation.
		/// </summary>
		public string SyncActionText { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CompareSyncMessageEventArgs()
		{
			SyncAction = SyncActionType.None;
			SyncActionText = "No action taken.";
		}
	}

	/// <summary>
	/// A delegate used for comparison sync action messages.
	/// </summary>
	/// <param name="sender">The message sender.</param>
	/// <param name="e">The event args.</param>
	public delegate void CompareSyncMessageDelegate(object sender, CompareSyncMessageEventArgs e);
}
