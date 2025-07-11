# FileExplorer

A simple WPF-based File Explorer application for Windows, built using the MVVM (Model-View-ViewModel) pattern and targeting .NET Framework 4.8.

## Features

- Browse all logical drives on your computer
- Expand drives and folders to view their contents (folders and files)
- Lazy loading of directory contents for performance
- Clean separation of UI and logic using MVVM

## Project Structure

- `App.xaml / App.xaml.cs`: Application entry point
- `MainWindow.xaml / MainWindow.xaml.cs`: Main window and code-behind, sets up the main view model
- `Directory/Data/DirectoryItem.cs`: Data model representing a drive, folder, or file
- `Directory/DirectoryStructure.cs`: Static helper class for querying the file system
- `Directory/ViewModels/Base/BaseViewModel.cs`: Base class for view models, implements `INotifyPropertyChanged`
- `Directory/ViewModels/Base/RelayCommand.cs`: Simple `ICommand` implementation for MVVM command binding
- `Directory/ViewModels/DirectoryStructureViewModel.cs`: Main view model, exposes the root directory items
- `Directory/ViewModels/DirectoryItemViewModel.cs`: View model for each directory item, handles expansion and child loading

## Requirements

- Windows OS
- .NET Framework 4.8
- Visual Studio 2019 or later (recommended)

## Building and Running

1. Open the solution in Visual Studio.
2. Restore any missing NuGet packages (if prompted).
3. Build the solution.
4. Run the application (F5 or Ctrl+F5).

## License

This project is provided as-is for educational purposes.
