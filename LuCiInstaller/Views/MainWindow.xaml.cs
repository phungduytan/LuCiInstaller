using LuCiInstaller.ViewModel;
using LuCiInstaller.Views.TrayIconView;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using Windows = System.Windows;
using Forms = System.Windows.Forms;
using System.Drawing;

namespace LuCiInstaller.Views
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          private TrayViewTest _viewTest;
          private NotifyIcon _notifyIcon;
          public MainWindow()
          {
               InitializeComponent();
               RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\LuCiInstall");
               RegistryKey regstart = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
               string keyvalue = "1";
               try
               {
                    regkey.SetValue("Index", keyvalue);
                    regstart.SetValue("LuCiInstall", System.Windows.Application.Current + "\\LuCiInstaller.exe");
                    regkey.Close();
               }
               catch (System.Exception ex)
               {
               }
               MainViewModel mainViewModel = new MainViewModel(bar1, bar2, bar0);
               DataContext = mainViewModel;
             
               
               //CreateSystemTrayIcon();
          }
          private void btnMore_Click(object sender, RoutedEventArgs e)
          {
               System.Windows.Controls.ContextMenu contextMenu = btnMore.ContextMenu;
               contextMenu.PlacementTarget = btnMore;
               contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom; // Hiển thị dưới nút
               contextMenu.HorizontalOffset = 0; // Điều chỉnh vị trí theo chiều ngang nếu cần
               contextMenu.VerticalOffset = 5;
               btnMore.ContextMenu.IsOpen = true;
          }
          private void CreateSystemTrayIcon()
          {
               _notifyIcon = new NotifyIcon
               {
                    Icon = new Icon("E:\\000_LuCi\\InstallApp\\LuCiInstaller\\Img\\LogoTest.ico"), // Set your icon here
                    Text = "Your App Name",
                    Visible = true
               };

               // Create and configure the context menu
               var contextMenu = new ContextMenuStrip();
               var imgtest = System.Drawing.Image.FromFile("E:\\000_LuCi\\InstallApp\\LuCiInstaller\\Img\\LogoTest.png");
               var quitimg = System.Drawing.Image.FromFile("E:\\000_LuCi\\InstallApp\\LuCiInstaller\\Img\\X.png");
               contextMenu.Items.Add("Show", imgtest, (sender, args) => ShowWindow());
               contextMenu.Items.Add("Exit", quitimg, (sender, args) => ExitApplication());

               _notifyIcon.ContextMenuStrip = contextMenu;

               // Handle double-click to show the main window
               _notifyIcon.DoubleClick += (sender, args) => ShowWindow();
          }
          private void ShowWindow()
          {
               this.Show();
               this.WindowState = WindowState.Normal;
          }

          private void ExitApplication()
          {
               _notifyIcon.Dispose();
               System.Windows.Application.Current.Shutdown();
          }

          // Optional: Hide the window instead of closing it
          protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
          {
               e.Cancel = true;
               this.Hide();
               base.OnClosing(e);
          }
     }
}