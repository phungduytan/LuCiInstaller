using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.IO;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace LuCiInstaller.VersionExtensions;
public class LuCiVersionFactory : ObservableObject
{ 
     public List<LuCiVersion> LuCiVersions { get; set; }
     public LuCiVersion LastVersion { get; set; }
     private string downloadNotify;
     public string DownloadNotify
     {
          get { return downloadNotify; }
          set { downloadNotify = value; OnPropertyChanged(); }
     }
     private string extractNotify;
     public string ExtractNotify
     {
          get { return extractNotify; }
          set { extractNotify = value; OnPropertyChanged(); }
     }

     private string removetNotify;
     public string RemovetNotify
     {
          get { return removetNotify; }
          set { removetNotify = value; OnPropertyChanged(); }
     }

     private readonly HttpClient client = new HttpClient();
     private int downloadSpeedLimit = 1024 * 5000;
     private string downloadPath = @"C:\ProgramData\Autodesk\ApplicationPlugins\LuCi.RevitAutomation.bundle.rar";
     private string extractPath = @"C:\ProgramData\Autodesk\ApplicationPlugins";
     public string installPath = @"C:\ProgramData\Autodesk\ApplicationPlugins\LuCi.RevitAutomation.bundle";
     public async Task<List<LuCiVersion>> GetListVersion(string Owner, string Repo)
     {
          LuCiVersions = new List<LuCiVersion>();
          string url = $"https://api.github.com/repos/{Owner}/{Repo}/releases";
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
               LuCiVersions.Add(new LuCiVersion(versionName, discription, timeUpdate));
          }
          return LuCiVersions;
     }
     public async Task<LuCiVersion> GetLastVersion()
     {
          LuCiVersions = new List<LuCiVersion>();
          HttpClient client = new HttpClient();
          string url = $"https://api.github.com/repos/phungduytan/LuCiInstaller/releases";
          client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
          HttpResponseMessage response = await client.GetAsync(url);
          response.EnsureSuccessStatusCode();
          string responseBody = await response.Content.ReadAsStringAsync();
          JArray releases = JArray.Parse(responseBody);
          foreach (var release in releases)
          {

               string versionName = release["name"]!.ToString();
               string discription = release["body"]!.ToString();
               string timeUpdate = release["published_at"]!.ToString();
               LuCiVersions.Add(new LuCiVersion(versionName, discription, timeUpdate));
          }
          LastVersion = LuCiVersions.ElementAt(0);
          await Task.Delay(2000);
          return LuCiVersions.ElementAt(0);
     }
     public async Task DowloadFileOnGithub(LuCiVersion luCiVersion, ProgressBar progressBarDowload, ProgressBar progressBarExt)
     {
          progressBarDowload.Visibility = Visibility.Visible;
          string fileUrl = $"https://github.com/phungduytan/LuCiInstaller/releases/download/{luCiVersion.Version}/LuCi.RevitAutomation.bundle.rar";
          using (HttpClient client = new HttpClient())
          using (HttpResponseMessage response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
          using (Stream contentStream = await response.Content.ReadAsStreamAsync())
          using (FileStream fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
          {
               var totalBytes = response.Content.Headers.ContentLength ?? -1L;
               var totalBytesRead = 0L;
               var buffer = new byte[8192];
               int bytesRead;
               progressBarDowload.Maximum = 100;
               do
               {
                    bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                         await fileStream.WriteAsync(buffer, 0, bytesRead);
                         totalBytesRead += bytesRead;

                         // Hiển thị tiến trình
                         if (totalBytes != -1)
                         {
                              progressBarDowload.Value = (double)totalBytesRead / totalBytes * 100;
                              DownloadNotify = $"{totalBytesRead/1024} of {totalBytes/1024} Kb ({progressBarDowload.Value:F2}%)";
                         }
                         else
                         {
                              DownloadNotify = $"{totalBytesRead/1024} Kb";
                         }

                         // Giới hạn tốc độ tải
                         if (downloadSpeedLimit > 0)
                         {
                              await Task.Delay(1000 * bytesRead / downloadSpeedLimit); // Tạo khoảng nghỉ để giới hạn tốc độ
                         }
                    }
               } while (bytesRead > 0);
          }
          DownloadNotify = "Done";
          progressBarExt.Value = 0;
          progressBarExt.Visibility = Visibility.Visible;
         
          TimeSpan extractionTimeLimit = TimeSpan.FromSeconds(1500);
          using (var cts = new CancellationTokenSource(extractionTimeLimit))
          {
               try
               {
                    await ExtractArchiveAsync(downloadPath, extractPath, cts.Token, progressBarExt);
               }
               catch (OperationCanceledException)
               {
                    Console.WriteLine("Time out");
               }
          }

          ExtractNotify = "Done";
     }
     private async Task ExtractArchiveAsync(string archivePath, string extractPath, CancellationToken cancellationToken, ProgressBar progressBar)
     {
          using (var archive = ArchiveFactory.Open(archivePath))
          {
               int totalEntries = archive.Entries.Count() - archive.Entries.Where(p=>p.IsDirectory).ToList().Count;
               int entriesProcessed = 0;

               foreach (var entry in archive.Entries)
               {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (!entry.IsDirectory)
                    {
                         entry.WriteToDirectory(extractPath, new ExtractionOptions()
                         {
                              ExtractFullPath = true,
                              Overwrite = true
                         });

                         entriesProcessed++;

                         // Hiển thị tiến trình giải nén
                         progressBar.Value = (double)entriesProcessed / totalEntries * 100;
                         double progress = (double)entriesProcessed / totalEntries * 100;
                         ExtractNotify = $"{entriesProcessed} of {totalEntries} ({progress:F2}%)";
                    }
                    await Task.Delay(50);
               }

          }
          File.Delete(archivePath);
          await Task.Delay(1000);
     }
     public async Task RemoveFileAsync( ProgressBar progressBar)
     {
          progressBar.Value = 0;
          if (Directory.Exists(installPath))
          {
               // Lấy tất cả file và thư mục con trong thư mục
               DirectoryInfo directory = new DirectoryInfo(installPath);
               FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories); // Lấy tất cả file bao gồm file trong thư mục con
               DirectoryInfo[] subDirectories = directory.GetDirectories("*", SearchOption.AllDirectories); // Lấy tất cả thư mục con

               // Tổng số lượng file và thư mục con
               int totalItems = files.Length + subDirectories.Length;
               int currentItem = 0;

              
               foreach (FileInfo file in files)
               {
                    try
                    {
                         currentItem++;
                         progressBar.Value = (double)currentItem / totalItems * 100;
                         RemovetNotify = $"{currentItem} of {totalItems} ({(int)progressBar.Value}%)" ;
                          file.Delete();
                    }
                    catch (Exception ex)
                    {
                         RemovetNotify = $"Error {file.FullName}: {ex.Message}";
                    }
                    await Task.Delay(50);
               }
               // Xóa thư mục gốc sau cùng
               foreach (DirectoryInfo subDirectory in subDirectories)
               {
                    try
                    {
                         currentItem++;
                         progressBar.Value = (double)currentItem / totalItems * 100;
                         RemovetNotify = $"{currentItem} of {totalItems} ({ (int)progressBar.Value}%)";
                         subDirectory.Delete(true); 
                    }
                    catch (Exception ex)
                    {
                         Console.WriteLine($"Error {ex.Message}");
                    }
                    await Task.Delay(50);
               }
               try
               {
                    directory.Delete();
               }
               catch (Exception ex)
               {
                    RemovetNotify= ex.Message;
               }
          }
          else
          {
               RemovetNotify = "0/0";
          }

          RemovetNotify = "Done";
          await Task.Delay(1000);
     }

     public void DeleteDirectoryRecursively(string targetDir)
     {
          string[] files = Directory.GetFiles(targetDir);
          string[] dirs = Directory.GetDirectories(targetDir);

          foreach (string file in files)
          {
               File.SetAttributes(file, FileAttributes.Normal);
               File.Delete(file);
          }

          foreach (string dir in dirs)
          {
               DeleteDirectoryRecursively(dir);
          }

          Directory.Delete(targetDir, false); // Finally, delete the empty directory
     }
}