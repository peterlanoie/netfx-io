using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync
{
	/// <summary>
	/// Defines the sync action [to be] taken on a compare item.
	/// </summary>
	public enum SyncActionType
	{
		/// <summary>
		/// No action has yet to be taken.
		/// </summary>
		None = 0,

		/// <summary>
		/// Item was or will be copied.
		/// </summary>
		Copied = 1,

		/// <summary>
		/// Item was or will be skipped because no action is necessary.
		/// </summary>
		Skipped = 2,

		/// <summary>
		/// Item was or will be deleted.
		/// </summary>
		Deleted = 4,

		/// <summary>
		/// Item was or will be ignored due to a filter.
		/// </summary>
		Ignored = 8, 
	}
}
