using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.Windows;
namespace LuCiInstaller.ViewModel;
public class GoogleDriveFileRespository
{
     private string[] Scopes = { DriveService.Scope.Drive };
     static string ApplicationName = "LuCiInstaller";
     private const string PathKeyGoogle = "./Item/FileJison.json";
     public GoogleDriveFileRespository() {


          //var Credential = GoogleCredential.FromFile(PathKeyGoogle);
          UserCredential credential;
          using (var streanm = new FileStream(PathKeyGoogle, FileMode.Open,FileAccess.Read))
          {
               string creadPath = "token.json";
               credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(streanm).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(creadPath,true)).Result;
          }
          var service = new DriveService(new BaseClientService.Initializer()
          {
               HttpClientInitializer = credential,
               ApplicationName = ApplicationName,
          });

          var fileID = "999010503673-kr2mulc0o8fn5rpeg1j2o182p78hajm1.apps.googleusercontent.com";
          var request = service.Files.Get(fileID);

          var stream1 = new FileStream("Version.txt",FileMode .Create,FileAccess.Write);

          request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
          {
               switch (progress.Status)
               {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                         MessageBox.Show(progress.BytesDownloaded.ToString());
                         break;
                    case Google.Apis.Download.DownloadStatus.Completed:
                         MessageBox.Show("Download complete.");
                         break;
                    case Google.Apis.Download.DownloadStatus.Failed:
                         MessageBox.Show("Download failed.");
                         break;
               }
          };

          request.Download(stream1);
          stream1.Close();
     }
}
