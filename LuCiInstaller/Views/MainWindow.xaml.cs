using LuCiInstaller.ViewModel;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuCiInstaller.Views
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          public MainWindow()
          {
               InitializeComponent();
               RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\LuCiInstall");
               //mo registry khoi dong cung win
               RegistryKey regstart = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
               string keyvalue = "1";
               //string subkey = "Software\\ManhQuyen";
               try
               {
                    //chen gia tri key
                    regkey.SetValue("Index", keyvalue);
                    //regstart.SetValue("taoregistrytronghethong", "E:\\Studing\\Bai Tap\\CSharp\\Channel 4\\bai temp\\tao registry trong he thong\\tao registry trong he thong\\bin\\Debug\\tao registry trong he thong.exe");
                    regstart.SetValue("LuCiInstall", Application.Current + "\\LuCiInstaller.exe");
                    ////dong tien trinh ghi key
                    //regkey.Close();
               }
               catch (System.Exception ex)
               {
               }

               DataContext = new MainViewModel();
          }
    }
}