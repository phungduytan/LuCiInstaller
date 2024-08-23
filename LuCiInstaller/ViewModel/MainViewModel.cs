using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuCiInstaller.VersionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LuCiInstaller.ViewModel;

public class MainViewModel : ObservableObject
{
     public ICommand CheckUpdateCommand { get; set; }
     public ICommand RepairCommand { get; set; }
     public ICommand InstallCommand { get; set; }
     public ICommand UnInstallCommand { get; set; }
     public ICommand LoadWindowCommand { get; set; }
     private string nameProject;
     public string NameProject
     {
          get {return nameProject;}
          set {nameProject = value;
               OnPropertyChanged();
          }
     }
     private LuCiVersion currentVersion;
     public LuCiVersion CurrentVersion
     {
          get { return currentVersion; }
          set
          {
               currentVersion = value;
               OnPropertyChanged();
          }
     }
     private LuCiVersion cloudVersion;
     public LuCiVersion CloudVersion
     {
          get { return cloudVersion; }
          set { cloudVersion = value;
               OnPropertyChanged(); 
          } 
     }
     private string notification;
     public string Notification
     {
          get { return notification; }
          set { notification = value; OnPropertyChanged(); }
     }
     private LuCiVersionFactory versionFac;
     public LuCiVersionFactory VersionFac
     {
          get { return versionFac; }
          set { versionFac = value; OnPropertyChanged(); }
     }
     ProgressBar progressBar;
     public MainViewModel( ProgressBar progressBar)
     {
          CheckUpdateCommand = new RelayCommand(CheckUpdate);
          this.progressBar = progressBar;
     }
     private void CheckUpdate()
     {
          GetCloudVersion();
     }
     private async void GetCloudVersion()
     {
          VersionFac = new LuCiVersionFactory();
          var gitHubReleases = await VersionFac.GetListVersion("phungduytan", "LuCiInstaller");
          CloudVersion = gitHubReleases.First();
          VersionFac.DowloadFileOnGithub(CloudVersion, this.progressBar);
     }
}
