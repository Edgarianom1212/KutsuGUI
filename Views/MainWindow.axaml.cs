using Avalonia.Controls;
using Avalonia.Platform.Storage;
using SukiUI.Controls;
using Avalonia;
using Avalonia.Dialogs;
using System.Linq;
using System.Collections.ObjectModel;
using DynamicData;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Interactivity;
using SukiUI.Theme;
using Avalonia.Styling;
using SukiUI;
using SukiUI.Enums;
using Avalonia.Media;
using System.Drawing;
using System.Threading.Tasks;
using System;
using SukiUI.Toasts;

namespace KutsuGUI.Views
{
    public partial class MainWindow : SukiWindow
    {
        public MainWindow()
        {
			InitializeComponent();
		}

		protected override void OnLoaded(RoutedEventArgs e)
		{
			base.OnLoaded(e);
			vm = DataContext as ViewModels.MainWindowViewModel;
			SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Dark);
		}

        private ViewModels.MainWindowViewModel vm;

		private void OpenFileExplorer(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
            IStorageProvider storage = this.StorageProvider;
            FilePickerOpenOptions options = new FilePickerOpenOptions();
            options.AllowMultiple = true;
            options.Title = "Select Files to change name";
			vm.SelectedFiles = storage.OpenFilePickerAsync(options).Result;
            if(vm.SelectedFiles?.Count > 0){
                    vm.SelectedFilesStrings.Clear();
                for(int i = 0; i < vm.SelectedFiles.Count; i++){
                    vm.SelectedFilesStrings.Add(vm.SelectedFiles.ElementAt(i).Path.ToString().Remove(0, 8));
				}
            }
		}		
        private void OpenFolderExplorer(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
            IStorageProvider storage = this.StorageProvider;
            FilePickerSaveOptions options = new FilePickerSaveOptions();
            vm.SelectedFolders = storage.OpenFolderPickerAsync(new FolderPickerOpenOptions()).Result;
            if (vm.SelectedFolders.Count > 0)
                vm.SelectedFolderString = vm.SelectedFolders.ElementAt(0).Path.ToString().Remove(0, 8);
        }

		private async void ConvertFiles(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
            ConvertButton.ShowProgress();
            if(vm.SelectedFilesStrings != null)
            {
                for(int i = 0; i < vm.SelectedFilesStrings.Count; i++)
                {
                    if(File.Exists(vm.SelectedFilesStrings.ElementAt(i)))
                    {
                        File.Move(vm.SelectedFilesStrings.ElementAt(i), vm.SelectedFolderString + vm.NewFileName + (i+1).ToString());
                    }
                }
            }
            await Task.Delay(1000);
            ConvertButton.HideProgress();
            SukiToastBuilder toast =
            vm.ToastManager.CreateToast()
                            .WithTitle("Finished")
                            .WithContent("Finished converting files")
                            .Dismiss().After(TimeSpan.FromSeconds(3));

            toast.SetType(Avalonia.Controls.Notifications.NotificationType.Success);
            toast.Queue();
		}
	}
}