using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Documents;

namespace LuCiInstaller.VersionExtensions;

public class LuCiVersion
{
     public string Version { get; set; }
     public int VersionNumber1 { get; set; }
     public int VersionNumber2 { get; set; }
     public int VersionNumber3 { get; set; }
     public string Discreption { get; set; } 
     public string TimeUpdate { get; set; }
     public LuCiVersion(string version, string discreption ,string timeUpdae)
     {
          Version = version;
          FindVersionNumber();
          Discreption = discreption;
          TimeUpdate = timeUpdae;
     }
     private List<int> FindVersionNumber()
     {
          var slipVersion = Version.Split('.');
          VersionNumber1 = Convert.ToInt32(slipVersion[0]);
          VersionNumber2 = Convert.ToInt32(slipVersion[1]);
          VersionNumber3 = Convert.ToInt32(slipVersion[2]);
          return new List<int>{VersionNumber1,VersionNumber2,VersionNumber3 };
     }
     public bool IsNewVersion(LuCiVersion Cloudversion)
     {
          bool a = false;
          if (Cloudversion.VersionNumber1 > VersionNumber1)
          {
               return true;
          }
          else {
               if (Cloudversion.VersionNumber2 > VersionNumber2)
               {
                    return true;
               }
               else {
                    if (Cloudversion.VersionNumber3 > VersionNumber3)
                    {
                         return true;
                    }
               }
          }
          return a;
     }
}
public class LuCiVersionFactory
{
     public List<LuCiVersion> LuCiVersions { get; set; }
     public LuCiVersion LastVersion { get; set; }
     private static readonly HttpClient client = new HttpClient();
     public async Task<List<LuCiVersion>> GetListVersion(string Owner, string Repo)
     {
          LuCiVersions = new List<LuCiVersion>();
          string url = $"https://api.github.com/repos/{Owner}/{Repo}/releases";
          client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
          try
          {
               HttpResponseMessage response = await client.GetAsync(url);
               response.EnsureSuccessStatusCode();
               string responseBody = await response.Content.ReadAsStringAsync();

               // Parse JSON
               JArray releases = JArray.Parse(responseBody);

               foreach (var release in releases)
               {

                    string versionName = release["name"]!.ToString();
                    string discription = release["body"]!.ToString();
                    string timeUpdate = release["published_at"]!.ToString();
                    LuCiVersions.Add(new LuCiVersion(versionName, discription, timeUpdate));
               }
          }
          catch (HttpRequestException e)
          {
               MessageBox.Show($"Request error: {e.Message}");
               return null;
          }
          return LuCiVersions;
     }
     public async Task SaveReleasesToFileAsync(string filePath, List<LuCiVersion> releases)
     {
          var options = new JsonSerializerOptions { WriteIndented = true };
          string jsonString = JsonSerializer.Serialize(releases, options);

          await File.WriteAllTextAsync(filePath, jsonString);
     }

}
