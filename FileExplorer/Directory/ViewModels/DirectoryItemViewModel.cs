using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FileExplorer
{
    /// <summary>
    /// A view model for each directory item
    /// </summary>
    public class DirectoryItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The type of this item
        /// </summary>
        public DirectoryItemType Type { get; set; }

        /// <summary>
        /// The full path to the item
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The name of this directory item
        /// </summary>
        public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFolderOrFileName(this.FullPath); } }

        /// <summary>
        /// A list of all children contained inside this item
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }

        /// <summary>
        /// Indicates if this item can be exapanded
        /// </summary>
        public bool CanExpand { get { return this.Type != DirectoryItemType.File; } }

        /// <summary>
        /// Indicates if the current item is expanded or not
        /// </summary>
        public bool IsExpanded
        {
            get { return this.Children?.Count(f => f != null) > 0; }
            set { if (value == true) Expand(); else this.ClearChildren(); }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to expand this item
        /// </summary>
        public ICommand ExpandCommand { get; set; }

        /// <summary>
        /// The command to copy this item
        /// </summary>
        public ICommand CopyCommand { get; set; }

        /// <summary>
        /// The command to move this item
        /// </summary>
        public ICommand MoveCommand { get; set; }

        /// <summary>
        /// The command to delete this item
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// The command to rename this item
        /// </summary>
        public ICommand RenameCommand { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fullPath">The full path of this item</param>
        /// <param name="type">The type of this item</param>
        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            // Create commands
            this.ExpandCommand = new RelayCommand(Expand);
            this.CopyCommand = new RelayCommand(Copy);
            this.MoveCommand = new RelayCommand(Move);
            this.DeleteCommand = new RelayCommand(Delete);
            this.RenameCommand = new RelayCommand(Rename);

            //Set full path and the type
            this.FullPath = fullPath;
            this.Type = type;

            // Set the children as needed
            this.ClearChildren();
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Removes all the children from the list, adding a dummy item to show the expand icon if required
        /// </summary>
        private void ClearChildren()
        {
            // Clear items
            this.Children = new ObservableCollection<DirectoryItemViewModel>();

            // Show the expand arrow if we are not a file
            if (this.Type != DirectoryItemType.File)
                this.Children.Add(null);
        }
        #endregion

        #region Expand
        /// <summary>
        /// Expands this directory and finds all children
        /// </summary>
        private void Expand()
        {
            // We cannot expand a file
            if (this.Type == DirectoryItemType.File)
                return;

            // Find all children
            var childrens = DirectoryStructure.GetDirectoryContents(this.FullPath).Select(content => new DirectoryItemViewModel(content.FullPath, content.Type));

            // Stores then into the Observable Collection
            this.Children = new ObservableCollection<DirectoryItemViewModel>(childrens);
        }
        #endregion

        #region File Operations
        private void Copy()
        {
            // Example: Copy to clipboard (implementation can be extended)
            Clipboard.SetText(this.FullPath);
        }
        private void Move()
        {
            // Move logic can be handled via drag-and-drop or context menu
            // Placeholder for move operation
        }
        public void MoveTo(string targetPath)
        {
            try
            {
                if (this.Type == DirectoryItemType.File)
                {
                    var fileName = Path.GetFileName(this.FullPath);
                    var destFile = Path.Combine(targetPath, fileName);
                    File.Move(this.FullPath, destFile);
                    this.FullPath = destFile;
                }
                else if (this.Type == DirectoryItemType.Folder)
                {
                    var folderName = Path.GetFileName(this.FullPath.TrimEnd(Path.DirectorySeparatorChar));
                    var destFolder = Path.Combine(targetPath, folderName);
                    Directory.Move(this.FullPath, destFolder);
                    this.FullPath = destFolder;
                }
            }
            catch (Exception ex)
            {
                // Handle error (e.g., show message)
            }
        }
        private void Delete()
        {
            try
            {
                if (this.Type == DirectoryItemType.File && File.Exists(this.FullPath))
                    File.Delete(this.FullPath);
                else if (this.Type == DirectoryItemType.Folder && Directory.Exists(this.FullPath))
                    Directory.Delete(this.FullPath, true);
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
        private void Rename()
        {
            // Placeholder for rename logic (e.g., show dialog to get new name)
        }
        #endregion
    }
}
