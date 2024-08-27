using LuCiInstaller.ViewModel.PageViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuCiInstaller.Views.pages
{
     /// <summary>
     /// Interaction logic for UpdateAvailablePage.xaml
     /// </summary>
     public partial class UpdateAvailablePage : Page
     {
          public UpdateAvailablePage()
          {
               InitializeComponent();
               DataContext = new  UpdateAvailableViewModel();
            
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
