using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace FileExplorer.Tests
{
    [TestClass]
    public class DirectoryItemViewModelTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            var vm = new DirectoryItemViewModel("C:\\Test", DirectoryItemType.Folder);
            Assert.AreEqual("C:\\Test", vm.FullPath);
            Assert.AreEqual(DirectoryItemType.Folder, vm.Type);
            Assert.IsNotNull(vm.Children);
            Assert.AreEqual(1, vm.Children.Count);
            Assert.IsNull(vm.Children[0]);
            Assert.IsInstanceOfType(vm.ExpandCommand, typeof(ICommand));
        }

        [TestMethod]
        public void Name_ReturnsFullPath_ForDrive()
        {
            var vm = new DirectoryItemViewModel("D:\\", DirectoryItemType.Drive);
            Assert.AreEqual("D:\\", vm.Name);
        }

        [TestMethod]
        public void Name_UsesDirectoryStructure_ForNonDrive()
        {
            // Arrange
            var called = false;
            var original = typeof(DirectoryStructure).GetMethod("GetFolderOrFileName");
            // Can't mock static, so just test the real method
            var vm = new DirectoryItemViewModel("C:/folder/file.txt", DirectoryItemType.File);
            Assert.AreEqual("file.txt", vm.Name);
        }

        [TestMethod]
        public void CanExpand_IsFalse_ForFile()
        {
            var vm = new DirectoryItemViewModel("C:/folder/file.txt", DirectoryItemType.File);
            Assert.IsFalse(vm.CanExpand);
        }

        [TestMethod]
        public void CanExpand_IsTrue_ForFolderOrDrive()
        {
            var vm1 = new DirectoryItemViewModel("C:/folder", DirectoryItemType.Folder);
            var vm2 = new DirectoryItemViewModel("C:/", DirectoryItemType.Drive);
            Assert.IsTrue(vm1.CanExpand);
            Assert.IsTrue(vm2.CanExpand);
        }

        [TestMethod]
        public void IsExpanded_Getter_ReturnsTrue_WhenChildrenExist()
        {
            var vm = new DirectoryItemViewModel("C:/folder", DirectoryItemType.Folder)
            {
                Children = new ObservableCollection<DirectoryItemViewModel> { new DirectoryItemViewModel("C:/folder/child", DirectoryItemType.File) }
            };
            Assert.IsTrue(vm.IsExpanded);
        }

        [TestMethod]
        public void IsExpanded_Getter_ReturnsFalse_WhenNoChildren()
        {
            var vm = new DirectoryItemViewModel("C:/folder", DirectoryItemType.Folder)
            {
                Children = new ObservableCollection<DirectoryItemViewModel>()
            };
            Assert.IsFalse(vm.IsExpanded);
        }

        [TestMethod]
        public void IsExpanded_Setter_ExpandsAndCollapses()
        {
            // Arrange
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var file = Path.Combine(tempDir, "file.txt");
            File.WriteAllText(file, "test");
            try
            {
                var vm = new DirectoryItemViewModel(tempDir, DirectoryItemType.Folder)
                {
                    // Expand
                    IsExpanded = true
                };
                Assert.IsTrue(vm.Children.Any(c => c != null && c.FullPath == file));
                // Collapse
                vm.IsExpanded = false;
                Assert.AreEqual(1, vm.Children.Count);
                Assert.IsNull(vm.Children[0]);
            }
            finally
            {
                File.Delete(file);
                Directory.Delete(tempDir);
            }
        }

        [TestMethod]
        public void ExpandCommand_ExecutesExpand()
        {
            // Arrange
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var file = Path.Combine(tempDir, "file.txt");
            File.WriteAllText(file, "test");
            try
            {
                var vm = new DirectoryItemViewModel(tempDir, DirectoryItemType.Folder);
                vm.ExpandCommand.Execute(null);
                Assert.IsTrue(vm.Children.Any(c => c != null && c.FullPath == file));
            }
            finally
            {
                File.Delete(file);
                Directory.Delete(tempDir);
            }
        }

        [TestMethod]
        public void Expand_DoesNothing_ForFile()
        {
            var vm = new DirectoryItemViewModel("C:/folder/file.txt", DirectoryItemType.File)
            {
                Children = new ObservableCollection<DirectoryItemViewModel> { new DirectoryItemViewModel("C:/folder/child", DirectoryItemType.File) }
            };
            vm.ExpandCommand.Execute(null);
            // Should not change children for file
            Assert.AreEqual(1, vm.Children.Count);
        }
    }
}
