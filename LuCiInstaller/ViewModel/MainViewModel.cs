using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuCiInstaller.VersionExtensions;
using LuCiInstaller.Views;
using LuCiInstaller.Views.pages;
using Newtonsoft.Json.Linq;
using SharpCompress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace LuCiInstaller.ViewModel;

public partial class MainViewModel : ObservableObject
{
     private bool isLoading;
     public bool IsLoading
     {
          get => isLoading;
          set { isLoading = value; OnPropertyChanged(); }
     }

     private LuCiVersion cloudVersion;
     public LuCiVersion CloudVersion
     {
          get { return cloudVersion; }
          set { cloudVersion = value; OnPropertyChanged(); }
     }

     private LuCiVersion currentVersion;
     public LuCiVersion CurrentVersion
     {
          get { return currentVersion; }
          set { value = currentVersion; OnPropertyChanged(); }
     }
     private string notyfication;
     public string Notyfication
     {
          get => notyfication;
          set { notyfication = value; OnPropertyChanged(); }
     }
     private LuCiVersionFactory versionFactory;
     public LuCiVersionFactory VersionFactory
     {
          get => versionFactory;
          set { versionFactory = value; OnPropertyChanged(); }
     }
     private Page currentPage;
     public Page CurrentPage
     {
          get { return currentPage; }
          set { currentPage = value; OnPropertyChanged(); }
     }
     public MainViewModel()
     {

         
     }
     [RelayCommand]
     private async void WindDowsLoad()
     {
          try
          {
               IsLoading = true;
               this.currentVersion = new LuCiVersion(  LuCiVersion.ReadCurrentVersion().Version, LuCiVersion.ReadCurrentVersion().Discreption, LuCiVersion.ReadCurrentVersion().TimeUpdate);
               VersionFactory = new LuCiVersionFactory();
               this.CloudVersion = await VersionFactory.GetLastVersion();

               if (CurrentVersion == null)
               {
                    this.Notyfication = "Không tìm thấy phiên bản đã cài đặt!";
                    //Show page cài đặt khi chưa có phiên bản nào đã cài đặt
                    CurrentPage = new InstallPage(CloudVersion, VersionFactory);
               }
               else
               {
                    if (currentVersion.IsOldVersion(cloudVersion))
                    {
                         this.Notyfication = "Phiên bản hiện tại đã cũ";
                         //Show page thể hiện phiên bản đã cũ
                         CurrentPage = new UpdateAvailablePage();
                    }
                    else
                    {
                         this.Notyfication = "Phiên bản cài đặt là phiên bản mới nhất";
                         //Show page để có thể repair hoặc uninstall
                         CurrentPage = new UpdatedPage(this.CurrentVersion);
                    }
               }
          }
          catch (Exception e)
          {

               Notyfication = $"Lỗi {e.Message}";
          }
          finally {
               IsLoading = false;
          }
     }

     
}

