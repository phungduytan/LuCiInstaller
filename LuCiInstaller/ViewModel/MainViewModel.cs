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

namespace LuCiInstaller.ViewModel;

public partial class MainViewModel : ObservableObject
{

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
     private LuCiVersionFactory versionFac;
     public LuCiVersionFactory VersionFac
     {
          get { return versionFac; }
          set { versionFac = value; OnPropertyChanged(); }
     }
     ProgressBar progressBar;
     public MainViewModel(ProgressBar progressBar)
     {
     }

     //public ICommand WindDowLoadCommand { get; set; }
     public ICommand InstallCommad { get; set; }
     public Page CurrentPage { get; set; }
     public MainViewModel()
     {

          WindDowLoad();

     }
     [RelayCommand]
     private async void WindDowLoad()
     {
          this.currentVersion = LuCiVersion.ReadCurrentVersion();
          var lol = GetLastVersion();
          this.cloudVersion = await lol;
          if (currentVersion == null)
          {
               this.notyfication = "Không tìm thấy phiên bản đã cài đặt!";
               //Show page cài đặt khi chưa có phiên bản nào đã cài đặt
          }
          else
          {
               if (currentVersion.IsOldVersion(cloudVersion))
               {
                    this.notyfication = "Phiên bản hiện tại đã cũ";
                    //Show page thể hiện phiên bản đã cũ
               }
               else
               {
                    this.notyfication = "Phiên bản cài đặt là phiên bản mới nhất";
                    //Show page để có thể repair hoặc uninstall
               }
          }
     }
     static async Task<LuCiVersion> GetLastVersion()
     {
          List<LuCiVersion> luCiVersions = new List<LuCiVersion>();
          HttpClient client = new HttpClient();
          string url = $"https://api.github.com/repos/phungduytan/LuCiInstaller/releases";
          client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
          HttpResponseMessage response = await client.GetAsync(url);
          //response.EnsureSuccessStatusCode();
          string responseBody = await response.Content.ReadAsStringAsync();
          JArray releases = JArray.Parse(responseBody);
          foreach (var release in releases)
          {

               string versionName = release["name"]!.ToString();
               string discription = release["body"]!.ToString();
               string timeUpdate = release["published_at"]!.ToString();
               luCiVersions.Add(new LuCiVersion(versionName, discription, timeUpdate));
          }
          return luCiVersions.First();
     }

}

