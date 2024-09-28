using Avalonia.Platform.Storage;
using ReactiveUI;
using SukiUI.Toasts;
using System.Collections.ObjectModel;
using System.Drawing;

namespace KutsuGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
		public ISukiToastManager ToastManager { get; } = new SukiToastManager();

		public MainWindowViewModel()
		{
			SelectedFilesStrings = new ObservableCollection<string>();
			SelectedFilesStrings.Add("Your input files will appear here");
		}
		public System.Collections.Generic.IReadOnlyList<IStorageFile>? SelectedFiles { get; set; }
		public System.Collections.Generic.IReadOnlyList<IStorageFolder>? SelectedFolders { get; set; }

		private string newFileName;
		public string NewFileName{
			get{
				return newFileName;
			}
			set{
				newFileName = value;
			}
		}

		public ObservableCollection<string> SelectedFilesStrings { get; set; }


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
	}
}
