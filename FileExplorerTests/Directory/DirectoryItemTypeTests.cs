using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileExplorer;

namespace FileExplorer.Tests
{
    [TestClass]
    public class DirectoryItemTypeTests
    {
        [TestMethod]
        public void DirectoryItemType_Values_AreCorrect()
        {
            // Check enum values by name
            Assert.AreEqual(0, (int)DirectoryItemType.Drive);
            Assert.AreEqual(1, (int)DirectoryItemType.File);
            Assert.AreEqual(2, (int)DirectoryItemType.Folder);
        }

        [TestMethod]
        public void DirectoryItemType_CanBeParsedFromString()
        {
            Assert.AreEqual(DirectoryItemType.Drive, (DirectoryItemType)System.Enum.Parse(typeof(DirectoryItemType), "Drive"));
            Assert.AreEqual(DirectoryItemType.File, (DirectoryItemType)System.Enum.Parse(typeof(DirectoryItemType), "File"));
            Assert.AreEqual(DirectoryItemType.Folder, (DirectoryItemType)System.Enum.Parse(typeof(DirectoryItemType), "Folder"));
        }

        [TestMethod]
        public void DirectoryItemType_ToString_ReturnsName()
        {
            Assert.AreEqual("Drive", DirectoryItemType.Drive.ToString());
            Assert.AreEqual("File", DirectoryItemType.File.ToString());
            Assert.AreEqual("Folder", DirectoryItemType.Folder.ToString());
        }
    }
}
