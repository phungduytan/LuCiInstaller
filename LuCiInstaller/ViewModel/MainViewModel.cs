using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuCiInstaller.VersionExtensions;
using SharpCompress;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LuCiInstaller.ViewModel;

public partial class MainViewModel : ObservableObject, INotifyPropertyChanged
{
     private Visibility isRemove;
     public Visibility IsRemove
     {
          get => isRemove;
          set { isRemove = value; OnPropertyChanged(); }
     }

     private Visibility isCanUpdate;
     public Visibility IsCanUpdate
     {
          get => isCanUpdate;
          set { isCanUpdate = value; OnPropertyChanged(); }
     }

     private Visibility inverseIsCanInstall;
     public Visibility InverseIsCanInstall
     {
          get => inverseIsCanInstall;
          set { inverseIsCanInstall = value; OnPropertyChanged(); }
     }

     private Visibility isCanInstall;
     public Visibility IsCanInstall
     {
          get => isCanInstall;
          set { isCanInstall = value; OnPropertyChanged(); }
     }

     private Visibility isDownloading;
     public Visibility IsDownloading
     {
          get => isDownloading;
          set { isDownloading = value; OnPropertyChanged(); }
     }

     private Visibility inverseIsDownloading;
     public Visibility InverseIsDownloading
     {
          get => inverseIsDownloading;
          set { inverseIsDownloading = value; OnPropertyChanged(); }
     }

     private Visibility inverseIsLoading;
     public Visibility InverseIsLoading
     {
          get => inverseIsLoading;
          set { inverseIsLoading = value; OnPropertyChanged(); }
     }

     private Visibility isLoading;
     public Visibility IsLoading
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
          set { currentVersion = value; OnPropertyChanged(); }
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

     private ProgressBar progressBarDownload;
     public ProgressBar ProgressBarDownload
     {
          get { return progressBarDownload; }
          set { progressBarDownload = value; OnPropertyChanged(); }
     }

     private ProgressBar progressBarExtrac;
     public ProgressBar ProgressBarExtrac
     {
          get { return progressBarExtrac; }
          set { progressBarExtrac = value; OnPropertyChanged(); }
     }

     private ProgressBar removeBarExtrac;
     public ProgressBar RemoveBarExtrac
     {
          get { return removeBarExtrac; }
          set { removeBarExtrac = value; OnPropertyChanged(); }
     }
     
          private ProgressBar progressBarRemove;
     public ProgressBar ProgressBarRemove
     {
          get { return progressBarRemove; }
          set { progressBarRemove = value; OnPropertyChanged(); }
     }

     public MainViewModel(ProgressBar progressBar1, ProgressBar progressBar2, ProgressBar progressBar0)
     {
          progressBarRemove = progressBar0;
          ProgressBarDownload = progressBar1;
          ProgressBarExtrac = progressBar2;

          this.currentVersion = new LuCiVersion();
     }
     [RelayCommand]
     public async void WindDowsLoad()
     {
          try
          {
               IsRemove = Visibility.Collapsed;
               IsLoading = Visibility.Visible;
               InverseIsLoading = Visibility.Collapsed;
               InverseIsDownloading = Visibility.Visible;
               IsDownloading = Visibility.Collapsed;
               IsCanUpdate = Visibility.Collapsed;
               IsCanInstall = Visibility.Collapsed;
               InverseIsCanInstall = Visibility.Visible;
               //this.currentVersion = new LuCiVersion(  LuCiVersion.ReadCurrentVersion().Version, LuCiVersion.ReadCurrentVersion().Discreption, LuCiVersion.ReadCurrentVersion().TimeUpdate);

               this.CurrentVersion = new LuCiVersion().ReadCurrentVersion();
               VersionFactory = new LuCiVersionFactory();
               this.CloudVersion = await VersionFactory.GetLastVersion();

               if (CurrentVersion == null)
               {
                    this.Notyfication = "Application not found. Please install the app to proceed.";
                    //Show page cài đặt khi chưa có phiên bản nào đã cài đặt
                    IsCanUpdate = Visibility.Visible;
                    IsCanInstall = Visibility.Visible;
                    InverseIsCanInstall = Visibility.Collapsed;
               }
               else
               {
                    if (CurrentVersion.IsOldVersion(cloudVersion))
                    {
                         this.Notyfication = "Update availble.";
                         IsCanUpdate = Visibility.Visible;

                    }
                    else
                    {
                         this.Notyfication = "All installations are up to date.";
                         IsCanUpdate = Visibility.Collapsed;

                    }
               }
          }
          catch (Exception e)
          {

               Notyfication = $"{e.Message}";
          }
          finally
          {
               IsLoading = Visibility.Collapsed;
               InverseIsLoading = Visibility.Visible;
               ProgressBarDownload.Value = 0;
               ProgressBarExtrac.Value = 0;
          }
     }

     [RelayCommand]
     public async void Update()
     {
          Notyfication = "Installing...";
          try
          {
               IsDownloading = Visibility.Visible;
               InverseIsDownloading = Visibility.Collapsed;
               await this.VersionFactory.DowloadFileOnGithub(CloudVersion, ProgressBarDownload, ProgressBarExtrac);
               CloudVersion.WiteToCurrentVersion();
          }
          catch (Exception e)
          {

               Notyfication = $"{e.Message}";
          }
          finally
          {
               WindDowsLoad();
          }
     }
     [RelayCommand]
     public async void Repair()
     {
          VersionFactory.DeleteDirectoryRecursively(versionFactory.installPath);
          Notyfication = "Repairing...";
          try
          {
               IsDownloading = Visibility.Visible;
               InverseIsDownloading = Visibility.Collapsed;
               LuCiVersion cloudCurrentVersion = VersionFactory.LuCiVersions.Where(p => p.Version.Equals(CurrentVersion.Version)).First();
               await this.VersionFactory.DowloadFileOnGithub(cloudCurrentVersion, ProgressBarDownload, ProgressBarExtrac);
               cloudCurrentVersion.WiteToCurrentVersion();
          }
          catch (Exception e)
          {

               Notyfication = $"{e.Message}";
          }
          finally
          {
               WindDowsLoad();
          }
     }
     [RelayCommand]
     public async void UnInstall()
     {
          CheckRevitIsOpening();
          Notyfication = "Uninstalling...";
          try
          {
               IsRemove = Visibility.Visible;
               IsDownloading = Visibility.Collapsed;
               InverseIsDownloading = Visibility.Collapsed;
               LuCiVersion cloudCurrentVersion = VersionFactory.LuCiVersions.Where(p => p.Version.Equals(CurrentVersion.Version)).First();
               await this.VersionFactory.RemoveFileAsync(  ProgressBarRemove);
               cloudCurrentVersion.WiteToCurrentVersion();
          }
          catch (Exception e)
          {

               Notyfication = $"{e.Message}";
          }
          finally
          {
               WindDowsLoad();
          }
     }
     private void CheckRevitIsOpening()
     {
          // Tìm tất cả các tiến trình có tên "Revit"
          var revitProcesses = Process.GetProcessesByName("Revit");

          if (revitProcesses.Length > 0)
          {
               // Hiển thị thông báo với các nút OK và Cancel
               var result = MessageBox.Show("Revit is currently running. Would you like to close Revit to proceed?",
                                            "Close Revit", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

               if (result == MessageBoxResult.OK)
               {
                    try
                    {
                         foreach (var process in revitProcesses)
                         {
                              process.Kill();
                              process.WaitForExit(); // Đợi tiến trình đóng hoàn toàn
                         }

                         MessageBox.Show("Revit has been closed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                         // Tiếp tục các tác vụ khác sau khi Revit đã đóng
                         // ...
                    }
                    catch (Exception ex)
                    {
                         // Hiển thị lỗi nếu không thể đóng Revit
                         MessageBox.Show("Failed to close Revit: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
               }
               else if (result == MessageBoxResult.Cancel)
               {
                    Application.Current.Shutdown();
                    Application.Current.Run();
               }
               
          }
          
     }


}

