using Avalonia.Platform.Storage;
using ReactiveUI;
using SukiUI.Toasts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace KutsuGUI.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ISukiToastManager ToastManager { get; } = new SukiToastManager();

		public MainWindowViewModel()
		{
			SelectedFilesStrings = new ObservableCollection<string>()
			{
				initSelectedFileString
			};
			SelectedFolderString = initSelectedFolderString;
			NewFileName = "";
			HowToUseIndex = 0;
			SelectedFileEndings = new ObservableCollection<string>()
			{
				initSelectedFileString
			};
		}

		public void SetDefaultValues()
		{
			SelectedFiles = null;
			SelectedFolders = null;
			SelectedFilesStrings.Clear();
			SelectedFilesStrings.Add(initSelectedFileString);
			NewFileName = "";
			SelectedFolderString = initSelectedFolderString;
			SelectedFileEndings.Clear();
			SelectedFileEndings.Add(initSelectedFileString);
		}

		public string initSelectedFileString = "Your input files will appear here";
		public string initSelectedFolderString = "Your output folder will appear here";
		public IReadOnlyList<IStorageFile>? SelectedFiles { get; set; }
		public ObservableCollection<string> SelectedFileEndings { get; set; }
		public IReadOnlyList<IStorageFolder>? SelectedFolders { get; set; }
		public ObservableCollection<string> SelectedFilesStrings { get; set; }
		private int progressValue;
		public int ProgressValue
		{
			get
			{
				return progressValue;
			}
			set
			{
				this.RaiseAndSetIfChanged(ref progressValue, value);
			}
		}

		private string newFileName;
		public string NewFileName
		{
			get
			{
				return newFileName;
			}
			set
			{
				this.RaiseAndSetIfChanged(ref newFileName, value);
			}
		}
		private string selectedFolderString;
		public string SelectedFolderString
		{
			get
			{
				return selectedFolderString;
			}
			set
			{
				this.RaiseAndSetIfChanged(ref selectedFolderString, value);
			}
		}
		private int howToUseIndex;
		public int HowToUseIndex
		{
			get
			{
				return howToUseIndex;
			}
			set
			{
				this.RaiseAndSetIfChanged(ref howToUseIndex, value);
			}
		}
		public IEnumerable<string> HowToUseSteps
		{
			get
			{
				return ["Choose Input File(s)", "Select Output Folder", "Input New File Name"];
			}
		}
	}
}
