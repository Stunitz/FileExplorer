﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileExplorer
{
    /// <summary>
    /// Helper class to query information about directories
    /// </summary>
    public static class DirectoryStructure
    {
        /// <summary>
        /// Gets all logical drives on the computer
        /// </summary>
        /// <returns></returns>
        public static List<DirectoryItem> GetLogicalDrives()
        {
            // Get every logical drive on the machine
            return Directory.GetLogicalDrives().Select(drive => new DirectoryItem { FullPath = drive, Type = DirectoryItemType.Drive }).ToList();
        }

        /// <summary>
        /// Gets the directories top-level content
        /// </summary>
        /// <param name="fullPath">The full path to the directory</param>
        /// <returns></returns>
        public static List<DirectoryItem> GetDirectoryContents(string fullPath)
        {
            // Create a empty list
            var items = new List<DirectoryItem>();


            #region Get Folders

            /* Try and get directories from the folder
                ignoring any issues doing so */
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    items.AddRange(dirs.Select(dir => new DirectoryItem { FullPath = dir, Type = DirectoryItemType.Folder }));
            }
            catch { }
            
            #endregion

            #region Get Files
            
            /* Try and get files from the folder
                ignoring any issues doing so */
            try
            {
                var files = Directory.GetFiles(fullPath);

                if (files.Length > 0)
                    items.AddRange(files.Select(file => new DirectoryItem { FullPath = file, Type = DirectoryItemType.File}));
            }
            catch { }

            #endregion

            return items;
        }

        #region Helpers

        /// <summary>
        /// Find the file or the folder name from a full path
        /// </summary>
        /// <param name="path">The full path of the item</param>
        /// <returns></returns>
        public static string GetFolderOrFileName(string path)
        {
            // If we have no path, return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Make all slashes to back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Remove trailing backslashes except for root (e.g., C:\)
            while (normalizedPath.Length > 3 && normalizedPath.EndsWith("\\"))
                normalizedPath = normalizedPath.TrimEnd('\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex < 0)
                return normalizedPath;

            // Return the name after the last back slash
            return normalizedPath.Substring(lastIndex + 1);
        }

        #endregion
    }
}
