//using System;


//namespace Dwares.Dwarf.Toolkit
//{
//	public static class Path
//	{
//		const char Separator = '/';

//		public static string GetFullPath(string path, string basePath)
//			=> GetFullPath(path, basePath, Separator, Separator);

//		public static string GetClassPath(string path, string basePath)
//			=> GetFullPath(path, basePath, '.', (char)0);

//		static string GetFullPath(string path, string basePath, char sep, char root)
//		{
//			if (path == null)
//				throw new ArgumentNullException(nameof(path));

//			if (path.Length == 0)
//				return path;

//			var separator = new char[] { Separator };
//			var pathParts = path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

//			if (path[0] == Separator) {
//				var fullPath = string.Join(sep.ToString(), pathParts);
//				if (root != 0) {
//					fullPath = root + fullPath;
//				}
//				return fullPath;
//			}

//			if (basePath == null)
//				throw new ArgumentException(nameof(basePath));

//			var baseParts = basePath.Split(separator);
//			int baseLength = baseParts.Length;

//			int pathStart;
//			for (pathStart = 0; pathStart < pathParts.Length; pathStart++) {
//				if (pathParts[pathStart] == ".")
//					continue;
//				if (pathParts[pathStart] == "..")
//					continue;
//			}
//		}
//	}
//}
