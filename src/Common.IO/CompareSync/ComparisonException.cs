using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync
{
	/// <summary>
	/// Exception thrown when a comparison operation can not be performed.
	/// </summary>
	[Serializable]
	public class ComparisonException : Exception
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ComparisonException() { }

		/// <summary>
		/// Creates a new instance of the exception with the assigned message.
		/// </summary>
		/// <param name="message">Exception message.</param>
		public ComparisonException(string message) : base(message) { }

		/// <summary>
		/// Creates a new instance of the exception with the assigned message.
		/// </summary>
		/// <param name="message">Exception message.</param>
		/// <param name="inner">Inner exception.</param>
		public ComparisonException(string message, Exception inner) : base(message, inner) { }

	}
}
