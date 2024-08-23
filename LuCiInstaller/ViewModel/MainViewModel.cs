using CommunityToolkit.Mvvm.ComponentModel;
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
     private string currentVersion;
     public string CurrentVersion
     {
          get { return currentVersion; }
          set
          {
               currentVersion = value;
               OnPropertyChanged();
          }
     }
     private string discription;
     public string Discription
     {
          get { return discription; }
          set
          {
               discription = value;
               OnPropertyChanged();
          }
     }
     private string cloudVersion;
     public string CloudVersion
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
          GetCloudVersion();
     }

     private async void GetCloudVersion()
     {
          var test = new LuCiVersionFactory();
         var gitHubReleases = await test.GetListVersion("phungduytan", "LuCiInstaller");
     }
}
