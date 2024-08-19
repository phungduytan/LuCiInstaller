using LuCiInstaller.ViewModel;
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
               DataContext = new MainViewModel();
          }
          private void btnMore_Click(object sender, RoutedEventArgs e)
          {
               ContextMenu contextMenu = btnMore.ContextMenu;
               contextMenu.PlacementTarget = btnMore;
               contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom; // Hiển thị dưới nút
               contextMenu.HorizontalOffset = 0; // Điều chỉnh vị trí theo chiều ngang nếu cần
               contextMenu.VerticalOffset = 5;
               btnMore.ContextMenu.IsOpen = true;
        }
    }
}