
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.CompareSync
{
	/// <summary>
	/// Defines a comparison item pair containing the left and right items.
	/// </summary>
	public class CompareItem
	{

		/// <summary>
		/// Gets the relative path of the item from which ever side is available.
		/// If neighter side has been set, throws a <see cref="ComparisonException"/>.
		/// </summary>
		/// <exception cref="ComparisonException">Thrown if neither the Left nor Right properties are set.</exception>
		public string RelativePath
		{
			get
			{
				if(Left != null)
				{
					return Left.RelativePath;
				}
				else if(Right != null)
				{
					return Right.RelativePath;
				}
				throw new ComparisonException("Compare item does not contain either a left or right entry.");
			}
		}

		internal ItemEntry Left { get; set; }

		internal ItemEntry Right { get; set; }

		/// <summary>
		/// Gets the comparison root path of the left item.  Will be null if .Left is null.
		/// </summary>
		public string LeftCompareRootPath { get { return Left != null ? Left.CompareRootPath : null; } }

		/// <summary>
		/// Gets the comparison root path of the right item.  Will be null if .Right is null.
		/// </summary>
		public string RightCompareRootPath { get { return Right != null ? Right.CompareRootPath : null; } }

		/// <summary>
		/// Gets the full path of the left item.  Will be null if .Left is null.
		/// </summary>
		public string LeftFullPath { get { return Left != null ? Left.FullPath : null; } }

		/// <summary>
		/// Gets the full path of the right item.  Will be null if .Right is null.
		/// </summary>
		public string RightFullPath { get { return Right != null ? Right.FullPath : null; } }

		/// <summary>
		/// The state of the comparison if a compare has been performed.
		/// </summary>
		public CompareStateType CompareState { get; set; }

		/// <summary>
		/// Whether or not this item has been filtered out.
		/// </summary>
		public bool IsFiltered { get; set; }

		///// <summary>
		///// What sync action has been taken on the item.
		///// </summary>
		//public SyncActionType SyncAction { get; set; }

		///// <summary>
		///// Test description of the action take by the sync operation.
		///// </summary>
		//public string SyncActionText { get; set; }

		/// <summary>
		/// Creates a new instance of the compare item with a CompareState of Unknown.
		/// </summary>
		public CompareItem()
		{
			CompareState = CompareStateType.Unknown;
			IsFiltered = false;
//			SyncAction = SyncActionType.None;
		}

		/// <summary>
		/// Returns the relative path of the compare item.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.RelativePath;
		}
	}
}
