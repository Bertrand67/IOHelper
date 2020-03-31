using System;
using System.IO;

namespace IOHelper
{
	public static class DirectoryInfoExtensions
	{
		/// <summary>
		/// Returns last write time of all directories or files within current directory.
		/// </summary>
		/// <param name="directoryInfo">Current directory.</param>
		/// <returns>Last write time of all directories or files.</returns>
		public static DateTime LastWriteTimeWithin(this DirectoryInfo directoryInfo)
		{
			DateTime result = directoryInfo.LastWriteTime;
			foreach(FileSystemInfo fileSystemInfo in directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
			{
				if (fileSystemInfo.LastWriteTime > result)
				{
					result = fileSystemInfo.LastWriteTime;
				}
			}
			return result;
		}
	}
}
