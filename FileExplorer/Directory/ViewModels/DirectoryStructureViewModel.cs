using System.Collections.ObjectModel;
using System.Linq;

namespace FileExplorer
{
    /// <summary>
    /// A view model for the applications main Directory view
    /// </summary>
    public class DirectoryStructureViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// A list of all directories on the machine
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DirectoryStructureViewModel()
        {
            // Get the logical drives
            var children = DirectoryStructure.GetLogicalDrives().Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive));

            // Create the view model from the data
            this.Items = new ObservableCollection<DirectoryItemViewModel>(children);
        }

        #endregion
    }
}
