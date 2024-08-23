using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace LuCiInstaller.VersionExtensions;
public class LuCiVersionFactory
{
     public List<LuCiVersion> LuCiVersions { get; set; }
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
