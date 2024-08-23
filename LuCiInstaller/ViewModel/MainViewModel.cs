using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuCiInstaller.VersionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
     public MainViewModel()
     {
          CheckUpdateCommand = new RelayCommand(CheckUpdate);
     }
     private void CheckUpdate()
     {
          GetCloudVersion();
     }
     private async void GetCloudVersion()
     {
          var test = new LuCiVersionFactory();
          var gitHubReleases = await test.GetListVersion("phungduytan", "LuCiInstaller");
          CloudVersion = gitHubReleases.First();
          Notification = CloudVersion.Version;
     }
}
