using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync
{
	/// <summary>
	/// Defines the various states of an item's comparison.
	/// </summary>
	public enum CompareStateType
	{
		/// <summary>
		/// Items match byte for byte.
		/// </summary>
		Match,

		/// <summary>
		/// Left item is an orphan.
		/// </summary>
		LeftOrphan,

		/// <summary>
		/// Right item is an orphan.
		/// </summary>
		RightOrphan,

		/// <summary>
		/// Items differ by 1 or more bytes.
		/// </summary>
		Different,

		/// <summary>
		/// Item comparison is unknown. (Default for new <see cref="CompareItem"/>s.
		/// </summary>
		Unknown,
	}
}
