using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileExplorer;

namespace FileExplorer.Tests
{
    [TestClass]
    public class DirectoryItemTests
    {
        [TestMethod]
        public void Name_ReturnsFullPath_ForDrive()
        {
            // Arrange
            var item = new DirectoryItem { Type = DirectoryItemType.Drive, FullPath = "C:\\" };

            // Act
            var name = item.Name;

            // Assert
            Assert.AreEqual("C:\\", name);
        }

        [TestMethod]
        public void Name_ReturnsFileName_ForFile()
        {
            // Arrange
            var item = new DirectoryItem { Type = DirectoryItemType.File, FullPath = "C:/folder/file.txt" };

            // Act
            var name = item.Name;

            // Assert
            Assert.AreEqual("file.txt", name);
        }

        [TestMethod]
        public void Name_ReturnsFolderName_ForFolder()
        {
            // Arrange
            var item = new DirectoryItem { Type = DirectoryItemType.Folder, FullPath = "C:/folder/" };

            // Act
            var name = item.Name;

            // Assert
            Assert.AreEqual("folder", name);
        }

        [TestMethod]
        public void Name_ReturnsEmpty_ForNullOrEmptyPath()
        {
            // Arrange
            var itemNull = new DirectoryItem { Type = DirectoryItemType.File, FullPath = null };
            var itemEmpty = new DirectoryItem { Type = DirectoryItemType.Folder, FullPath = string.Empty };

            // Act & Assert
            Assert.AreEqual(string.Empty, itemNull.Name);
            Assert.AreEqual(string.Empty, itemEmpty.Name);
        }
    }
}
