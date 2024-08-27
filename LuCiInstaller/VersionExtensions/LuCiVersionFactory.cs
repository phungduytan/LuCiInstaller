using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace LuCiInstaller.VersionExtensions;
public class LuCiVersionFactory : ObservableObject
{
     public List<LuCiVersion> LuCiVersions { get; set; }
     private string notification;
     public string Notification
     {
          get { return notification; }
          set { notification = value; OnPropertyChanged(); }
     }
     private readonly HttpClient client = new HttpClient();
     private int downloadSpeedLimit = 1024 * 20000;
     private string downloadPath = @"C:\ProgramData\Autodesk\ApplicationPlugins\LuCi.RevitAutomation.bundle.rar";
     private string extractPath = @"C:\ProgramData\Autodesk\ApplicationPlugins";

     public async Task<IEnumerable<LuCiVersion>> GetListVersion(string Owner, string Repo)
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
     public async void DowloadFileOnGithub(LuCiVersion luCiVersion, ProgressBar progressBar)
     {
          progressBar.Visibility = Visibility.Visible;
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
               progressBar.Maximum = 100;

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
                              progressBar.Value = (double)totalBytesRead / totalBytes * 100;
                              Notification = $"Đang tải về: {totalBytesRead} / {totalBytes} bytes ({progressBar.Value:F2}%)";
                         }
                         else
                         {
                              MessageBox.Show($"Đã tải: {totalBytesRead} bytes");
                         }

                         // Giới hạn tốc độ tải
                         if (downloadSpeedLimit > 0)
                         {
                              await Task.Delay(1000 * bytesRead / downloadSpeedLimit); // Tạo khoảng nghỉ để giới hạn tốc độ
                         }
                    }
               } while (bytesRead > 0);
          }
          Notification = "Tải về thành công";
          progressBar.Value = 0;
          TimeSpan extractionTimeLimit = TimeSpan.FromSeconds(1500);
          using (var cts = new CancellationTokenSource(extractionTimeLimit))
          {
               try
               {
                    await ExtractArchiveAsync(downloadPath, extractPath, cts.Token, progressBar);
                    Console.WriteLine("Giải nén hoàn tất!");
               }
               catch (OperationCanceledException)
               {
                    Console.WriteLine("Giải nén bị hủy do vượt quá thời gian giới hạn.");
               }
          }

          Notification = "Cài đặt thành công";
     }
     private async Task ExtractArchiveAsync(string archivePath, string extractPath, CancellationToken cancellationToken, ProgressBar progressBar)
     {
          using (var archive = ArchiveFactory.Open(archivePath))
          {
               int totalEntries = archive.Entries.Count();
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
                         Notification = $"Giải nén: {entriesProcessed}/{totalEntries} ({progress:F2}%)";
                    }
                    await Task.Delay(100, cancellationToken);
               }

          }
     }
}