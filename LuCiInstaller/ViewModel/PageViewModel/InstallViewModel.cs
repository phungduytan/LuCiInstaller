using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuCiInstaller.VersionExtensions;
using LuCiInstaller.Views.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LuCiInstaller.ViewModel.PageViewModel;

public partial class InstallViewModel : ObservableObject
{
     public LuCiVersion CloudVersion { get; set; }
     public LuCiVersionFactory LuCiVersionFactory { get; set; }
     public DownloadAndInstallPage DownloadPage { get; set; }

     private ProgressBar _progressBar1;
     public ProgressBar ProgressBar1 {
          get { return _progressBar1; }
          set { _progressBar1 = value; OnPropertyChanged(); }
     }

     private ProgressBar _progressBar2;
     public ProgressBar ProgressBar2
     {
          get { return _progressBar2; }
          set { _progressBar2 = value; OnPropertyChanged(); }
     }

     private bool isDownloading;
     public bool IsDownloading {
          get { return isDownloading; }
          set { isDownloading = value; OnPropertyChanged(); }
     
     }
     public InstallViewModel(LuCiVersion cloudVersion, LuCiVersionFactory luCiVersionFactory ,DownloadAndInstallPage downloadAndInsstallPage)
     {
          IsDownloading = false;
          CloudVersion = cloudVersion;
          LuCiVersionFactory = luCiVersionFactory;
          DownloadPage = downloadAndInsstallPage;
          ProgressBar1 = downloadAndInsstallPage.bar1;
          ProgressBar2 = downloadAndInsstallPage.bar2;
     }
     [RelayCommand]

     private async void Install()
     {
         IsDownloading = true;

         await LuCiVersionFactory.DowloadFileOnGithub(CloudVersion, ProgressBar1, ProgressBar2);


         IsDownloading = false;
     }
}
