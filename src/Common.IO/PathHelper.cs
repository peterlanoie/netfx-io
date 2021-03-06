﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Web;

namespace Common.IO
{

	/// <summary>
	/// Provides helper methods for dealing with paths.
	/// </summary>
	public static class PathHelper
	{

		/// <summary>
		/// Gets the resolved directory path of the specified file. 
		/// Given a non-rooted file path, the method will resolve 
		/// the file name relative to the current runtime directory.
		/// </summary>
		/// <param name="file">Fully qualified or relative file.</param>
		/// <returns>Fully qualified directory path of file.</returns>
		public static string GetDirectory(string file)
		{
			string strDir = file;

			if(!Path.IsPathRooted(file))
			{
				file = ResolveRuntimePath(file);
			}
			strDir = Path.GetDirectoryName(file);

			return strDir;
		}

		/// <summary>
		/// Resolves a file name to a fully qualified path. A fully qualified file path passed in will return as-is.  
		/// A relative file will be resolved to a path relative to the application runtime directory.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetFullFileName(string fileName)
		{
			if(Path.IsPathRooted(fileName))
			{
				return fileName;
			}
			return ResolveRuntimePath(fileName);
		}

		/// <summary>
		/// Resolves a relative path to a full path under the current runtime location.
		/// Helpful for referring to assets in paths included with or generated by the build output of assemblies (config/log files, etc.).
		/// Will resolve paths starting with '~' as web relative, otherwise path is resolved relative to the executing assemblies.
		/// </summary>
		/// <param name="relativePath">A path relative to the runtime directory.</param>
		/// <returns></returns>
		public static string ResolveRuntimePath(string relativePath)
		{
			if(relativePath.StartsWith("~"))
			{
				if(HttpContext.Current == null)
				{
					throw new InvalidOperationException(string.Format("Current application is not a web application, can not resolve the value \"{0}\" because it starts with a '~'", relativePath));
				}
				return HttpContext.Current.Server.MapPath(relativePath);
			}
			return Path.Combine(GetRuntimeDirectory(), relativePath);
		}

		/// <summary>
		/// Returns the directory in which the code is running, either the web application /bin directory or the running assembly's location.
		/// </summary>
		/// <returns></returns>
		public static string GetRuntimeDirectory()
		{
			string dir;

			if(HttpContext.Current != null)
			{
				dir = HttpContext.Current.Server.MapPath("~/bin");
			}
			else
			{
				dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			}
			return dir;
		}

		/// <summary>
		/// Creates the specified directory if necessary. 
		/// Returns true if directory was created or false if already there.
		/// </summary>
		/// <param name="dir">The directory to create.</param>
		/// <returns>Boolean indicating whether (true) or not the directory was created.</returns>
		public static bool EnsureDirectoryExists(string dir)
		{
			if(!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Checks each of the items <paramref name="pathList"/> and returns the first found on the system.
		/// Returns null if none are found.
		/// </summary>
		/// <param name="separator">List separator character.</param>
		/// <param name="pathList">Character separated list of paths to check.</param>
		/// <returns></returns>
		public static string FindFirstValidPath(char separator, string pathList)
		{
			return FindFirstValidPath(pathList.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// Checks each of the items <paramref name="paths"/> and returns the first found on the system.
		/// Returns null if none are found.
		/// </summary>
		/// <param name="paths">List of paths to check.</param>
		/// <returns></returns>
		public static string FindFirstValidPath(params string[] paths)
		{
			string path = null;
			foreach(var item in paths)
			{
				if(File.Exists(item))
				{
					path = item;
				}
			}
			return path;
		}

	}
}
