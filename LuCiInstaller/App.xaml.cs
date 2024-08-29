using LuCiInstaller.VersionExtensions;
using LuCiInstaller.ViewModel;
using LuCiInstaller.Views;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace LuCiInstaller
{
     /// <summary>
     /// Interaction logic for App.xaml
     /// </summary>
     public partial class App : Application
     {
          protected override void OnStartup(StartupEventArgs e)
          {
               base.OnStartup(e);
          }
          protected override void OnExit(ExitEventArgs e)
          {
               base.OnExit(e);
          }
     }

}
