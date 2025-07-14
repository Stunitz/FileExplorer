using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FileExplorer.Tests
{
    [TestClass]
    public class DirectoryStructureViewModelTests
    {
        [TestMethod]
        public void Constructor_InitializesItemsWithLogicalDrives()
        {
            // Arrange & Act
            var viewModel = new DirectoryStructureViewModel();

            // Assert
            Assert.IsNotNull(viewModel.Items, "Items should not be null after construction.");
            var expectedDrives = DirectoryStructure.GetLogicalDrives();
            Assert.AreEqual(expectedDrives.Count, viewModel.Items.Count, "Items count should match logical drives count.");
            foreach (var item in viewModel.Items)
            {
                Assert.IsNotNull(item);
                Assert.AreEqual(DirectoryItemType.Drive, item.Type);
                Assert.IsTrue(expectedDrives.Any(d => d.FullPath == item.FullPath));
            }
        }
    }
}
