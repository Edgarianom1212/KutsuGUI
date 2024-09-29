using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using SukiUI;
using SukiUI.Controls;
using SukiUI.Models;
using SukiUI.Theme;
using SukiUI.Toasts;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KutsuGUI.Views
{
	public partial class MainWindow : SukiWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private SukiToastBuilder successToast;
		private SukiToastBuilder noFilesSelectedToast;
		private SukiToastBuilder noDestinationSelectedToast;
		private SukiToastBuilder noNewFileNameToast;
		private ViewModels.MainWindowViewModel vm;
		private bool wasSuccess = false;

		protected override void OnLoaded(RoutedEventArgs e)
		{
			base.OnLoaded(e);

			vm = DataContext as ViewModels.MainWindowViewModel;



			var PurpleTheme = new SukiColorTheme("Purple", Colors.MediumPurple, Colors.BlueViolet);
			SukiTheme.GetInstance().AddColorTheme(PurpleTheme);
			SukiTheme.GetInstance().ChangeColorTheme(PurpleTheme);
		}

		private void OpenFileExplorer(object? sender, RoutedEventArgs e)
		{
			IStorageProvider storage = this.StorageProvider;
			FilePickerOpenOptions options = new FilePickerOpenOptions();
			options.AllowMultiple = true;
			options.Title = "Select Files to change name";
			vm.SelectedFiles = storage.OpenFilePickerAsync(options).Result;
			if (vm.SelectedFiles?.Count > 0)
			{
				vm.SelectedFilesStrings.Clear();
				for (int i = 0; i < vm.SelectedFiles.Count; i++)
					vm.SelectedFilesStrings.Add(vm.SelectedFiles.ElementAt(i).Path.ToString().Remove(0, 8));
			}
		}
		private void OpenFolderExplorer(object? sender,RoutedEventArgs e)
		{
			IStorageProvider storage = this.StorageProvider;
			FilePickerSaveOptions options = new FilePickerSaveOptions();
			vm.SelectedFolders = storage.OpenFolderPickerAsync(new FolderPickerOpenOptions()).Result;
			if (vm.SelectedFolders.Count > 0)
				vm.SelectedFolderString = vm.SelectedFolders.ElementAt(0).Path.ToString().Remove(0, 8);
		}

		private async void ConvertFiles(object? sender, RoutedEventArgs e)
		{
			ConvertButton.ShowProgress();

			successToast = vm.ToastManager.CreateToast().WithTitle("Success").WithContent("Finished converting files").Dismiss().After(TimeSpan.FromSeconds(3)).Dismiss().ByClicking();
			successToast.SetType(Avalonia.Controls.Notifications.NotificationType.Success);

			noFilesSelectedToast = vm.ToastManager.CreateToast().WithTitle("Warning").WithContent("No files were selected").Dismiss().After(TimeSpan.FromSeconds(3)).Dismiss().ByClicking();
			noFilesSelectedToast.SetType(Avalonia.Controls.Notifications.NotificationType.Error);

			noDestinationSelectedToast = vm.ToastManager.CreateToast().WithTitle("Warning").WithContent("No destination was selected").Dismiss().After(TimeSpan.FromSeconds(3)).Dismiss().ByClicking();
			noDestinationSelectedToast.SetType(Avalonia.Controls.Notifications.NotificationType.Error);

			noNewFileNameToast = vm.ToastManager.CreateToast().WithTitle("Warning").WithContent("No new file name was selected").Dismiss().After(TimeSpan.FromSeconds(3)).Dismiss().ByClicking();
			noNewFileNameToast.SetType(Avalonia.Controls.Notifications.NotificationType.Error);

			if (vm.NewFileName.Trim() != "" && vm.SelectedFilesStrings?[0] != vm.initSelectedFileString && vm.SelectedFolderString != vm.initSelectedFolderString)
			{
				for (int i = 0; i < vm.SelectedFilesStrings?.Count; i++)
				{
					if (File.Exists(vm.SelectedFilesStrings.ElementAt(i)))
					{
						File.Move(vm.SelectedFilesStrings.ElementAt(i), vm.SelectedFolderString + vm.NewFileName.Trim() + (i + 1).ToString());
						vm.SetDefaultValues();
						wasSuccess = true;
						for (int y = 0; y < 20; y++)
						{
							vm.ProgressValue += 5;
							await Task.Delay(40); //put some loading time so user sees the process
						}
					}
				}
			}
			if (!wasSuccess)
				await Task.Delay(800);

			ConvertButton.HideProgress();

			if (wasSuccess)
			{
				successToast.Queue();
				wasSuccess = false;
			}
			else
			{
				if (vm.NewFileName.Trim() == "")
					noNewFileNameToast.Queue();

				if (vm.SelectedFolderString == vm.initSelectedFolderString)
					noDestinationSelectedToast.Queue();

				if (vm.SelectedFilesStrings?[0] == vm.initSelectedFileString)
					noFilesSelectedToast.Queue();
			}
			await Task.Delay(3000);
			if(vm.ProgressValue == 100)
			{
				for (int y = 0; y < 20; y++)
				{
					vm.ProgressValue -= 5;
					await Task.Delay(40); //put some loading time so user sees the process
				}
			}
		}

		private void HowToUseStepNext(object? sender, RoutedEventArgs e)
		{
			if (vm.HowToUseIndex < vm.HowToUseSteps.Count()-1)
				vm.HowToUseIndex++;
		}		
		private void HowToUseStepPrevious(object? sender, RoutedEventArgs e)
		{
			if (vm.HowToUseIndex > 0)
				vm.HowToUseIndex--;
		}
	}
}