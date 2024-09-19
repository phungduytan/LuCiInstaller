using CommunityToolkit.Mvvm.ComponentModel;
using SharpCompress.Common;
using System.IO;

namespace LuCiInstaller.VersionExtensions;

public class LuCiVersion : ObservableObject
{
     public string Version {  get; set; }
     public int VersionNumber1 { get; set; }
     public int VersionNumber2 { get; set; }
     public int VersionNumber3 { get; set; }
     public string Discreption { get; set; }
     public string TimeUpdate { get; set; }
     public LuCiVersion() 
     {
          Version = "0.0.0"; VersionNumber1 = 0; VersionNumber2 = 0;VersionNumber3 = 0; Discreption = "";TimeUpdate = "";
     }
     public LuCiVersion(string version, string discreption, string timeUpdae)
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
          return new List<int> { VersionNumber1, VersionNumber2, VersionNumber3 };
     }
     public bool IsOldVersion(LuCiVersion Cloudversion)
     {
          bool a = false;
          if (Cloudversion.VersionNumber1 > VersionNumber1)
          {
               return true;
          }
          else
          {
               if (Cloudversion.VersionNumber2 > VersionNumber2)
               {
                    return true;
               }
               else
               {
                    if (Cloudversion.VersionNumber3 > VersionNumber3)
                    {
                         return true;
                    }
               }
          }
          return a;
     }
     public void WiteToCurrentVersion()
     {
          string filePath = @"C:\ProgramData\Autodesk\ApplicationPlugins\LuCi.RevitAutomation.bundle\Verion.txt";
          using (StreamWriter writer = new StreamWriter(filePath, false))
          {
               writer.WriteLine(Version);
               writer.WriteLine(Discreption);
               writer.WriteLine(TimeUpdate);
               writer.Close();
          }
     }
     public LuCiVersion ReadCurrentVersion()
     {
          string filePath = @"C:\ProgramData\Autodesk\ApplicationPlugins\LuCi.RevitAutomation.bundle\Verion.txt";
          string version = null;
          string discreption = null;
          string timeUpdate = null;
          if (File.Exists(filePath))
          {
               using (StreamReader reader = new StreamReader(filePath))
               {
                    version = reader.ReadLine();
                    discreption = reader.ReadLine();
                    timeUpdate = reader.ReadLine();
               }
          }
          else
          {
               return null;
          }
          return new LuCiVersion(version, discreption, timeUpdate);
     }
}
