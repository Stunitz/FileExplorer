using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FileExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TreeViewItem _draggedItem;

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new DirectoryStructureViewModel();
        }

        #endregion      

        private void FolderView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeView = sender as TreeView;
            var item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (item != null)
            {
                _draggedItem = item;
                DragDrop.DoDragDrop(item, item.DataContext, DragDropEffects.Move);
            }
        }

        private void FolderView_Drop(object sender, DragEventArgs e)
        {
            var targetItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject);
            var sourceVm = e.Data.GetData(typeof(DirectoryItemViewModel)) as DirectoryItemViewModel;
            var targetVm = targetItem?.DataContext as DirectoryItemViewModel;
            if (sourceVm != null && targetVm != null && targetVm.Type != DirectoryItemType.File)
            {
                // Move operation
                sourceVm.MoveTo(targetVm.FullPath);
            }
        }

        private static T VisualUpwardSearch<T>(DependencyObject source) where T : DependencyObject
        {
            while (source != null && !(source is T))
                source = VisualTreeHelper.GetParent(source);
            return source as T;
        }
    }
}