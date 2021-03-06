﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.IO.FileSystem
{
	/// <summary>
	/// Defines a file system tree entry.
	/// </summary>
	[Serializable]
	public abstract class TreeEntry
	{
		private int _nextNodeIndex = 0;

		/// <summary>
		/// The item's parent. Can be null for the root node.
		/// </summary>
		public TreeEntry Parent { get; private set; }

		/// <summary>
		/// An autogenerated, unique Id for the item in the tree.
		/// Helpful for identifying the nodes when in a flat list.
		/// </summary>
		public int NodeId { get; private set; }

		/// <summary>
		/// Returns the name of the file system entry without the root portion.
		/// </summary>
		public string RootRelativePath
		{
			get { return this.Info.FullName.Replace(TreeRootPath, "").TrimStart('\\');; }
		}

		/// <summary>
		/// Gets the file system info class for this item.
		/// </summary>
		public FileSystemInfo Info { get; private set; }

		/// <summary>
		/// Gets the root ancestor of the tree item
		/// </summary>
		protected TreeEntry RootEntry
		{
			get
			{
				return ForAllParents(null);
			}
		}

		/// <summary>
		/// Gets the tree depth of the entry.
		/// </summary>
		public int Depth
		{
			get
			{
				int intDepth = 0;
				ForAllParents(delegate(TreeEntry t) { intDepth++; });
				return intDepth;
			}
		}

		/// <summary>
		/// Gets the root path of the root entry of the tree.
		/// </summary>
		public string TreeRootPath
		{
			get { return RootEntry.Info.FullName; }
		}

		/// <summary>
		/// Gets the node Id of this entry's parent or -1 of this entry is the root entry.
		/// </summary>
		public int ParentNodeId
		{
			get { return this.Parent != null ? this.Parent.NodeId : -1; }
		}

		/// <summary>
		/// Creates a new tree entry.
		/// </summary>
		/// <param name="info">The entry's file system info.</param>
		/// <param name="parent">The optional parent node of the tree entry.</param>
		public TreeEntry(FileSystemInfo info, TreeEntry parent)
		{
			Info = info;
			Parent = parent;
			NodeId = this.RootEntry._nextNodeIndex++;
		}

		/// <summary>
		/// Performs the specified <paramref name="action"/> on all parents of the entry.
		/// Returns the root entry.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private TreeEntry ForAllParents(Action<TreeEntry> action)
		{
			TreeEntry objNode = this;
			while(objNode.Parent != null)
			{
				objNode = objNode.Parent;
				if(action != null) action(objNode);
			}
			return objNode;
		}

		/// <summary>
		/// Gets a list of the nodes that make up this branch from the tree root down to this node's parent.
		/// </summary>
		/// <returns></returns>
		public List<TreeEntry> GetBranchEntries()
		{
			List<TreeEntry> lstEntries = new List<TreeEntry>();
			ForAllParents(delegate(TreeEntry t) { lstEntries.Add(t); });
			lstEntries.Reverse();
			return lstEntries;
		}

		/// <summary>
		/// Returns a list of the node IDs from the tree root down to this node's parent.
		/// </summary>
		/// <returns></returns>
		public List<int> GetParentNodeIds()
		{
			return this.GetBranchEntries().Select(e => e.NodeId).ToList();
		}
	}
}
