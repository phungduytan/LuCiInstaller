using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuCiInstaller.Models
{
     public static class VisibilityConvert
     {
          public static Visibility BooleanToVisibility(this bool value) =>   value ? Visibility.Visible : Visibility.Collapsed;
          
          public static Visibility InverseBooleanToVisibility(this bool value) => value ? Visibility.Collapsed : Visibility.Visible;
     }
}
