using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace FileExplorer.Tests
{
    [TestClass]
    public class DirectoryStructureTests
    {
        [TestMethod]
        public void GetLogicalDrives_ReturnsDrives()
        {
            // Act
            var drives = DirectoryStructure.GetLogicalDrives();

            // Assert
            Assert.IsNotNull(drives);
            Assert.IsTrue(drives.Count > 0, "Should return at least one logical drive");
            foreach (var drive in drives)
            {
                Assert.IsTrue(drive.Type == DirectoryItemType.Drive);
                Assert.IsFalse(string.IsNullOrWhiteSpace(drive.FullPath));
            }
        }

        [TestMethod]
        public void GetDirectoryContents_ReturnsFoldersAndFiles()
        {
            // Arrange
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            string subDir = Path.Combine(tempDir, "subfolder");
            Directory.CreateDirectory(subDir);
            string file = Path.Combine(tempDir, "file.txt");
            File.WriteAllText(file, "test");

            try
            {
                // Act
                var items = DirectoryStructure.GetDirectoryContents(tempDir);

                // Assert
                Assert.IsNotNull(items);
                Assert.IsTrue(items.Any(i => i.Type == DirectoryItemType.Folder && i.FullPath == subDir));
                Assert.IsTrue(items.Any(i => i.Type == DirectoryItemType.File && i.FullPath == file));
            }
            finally
            {
                // Cleanup
                File.Delete(file);
                Directory.Delete(subDir);
                Directory.Delete(tempDir);
            }
        }

        [TestMethod]
        public void GetDirectoryContents_InvalidPath_ReturnsEmpty()
        {
            // Act
            var items = DirectoryStructure.GetDirectoryContents("Z:\\ThisPathShouldNotExist");

            // Assert
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        [DataRow(null, "")]
        [DataRow("", "")]
        [DataRow("C:\\folder\\file.txt", "file.txt")]
        [DataRow("C:/folder/file.txt", "file.txt")]
        [DataRow("file.txt", "file.txt")]
        [DataRow("C:\\folder\\", "folder")]
        [DataRow("C:/folder/", "folder")]
        public void GetFolderOrFileName_ReturnsExpected(string input, string expected)
        {
            // Act
            var result = DirectoryStructure.GetFolderOrFileName(input);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}