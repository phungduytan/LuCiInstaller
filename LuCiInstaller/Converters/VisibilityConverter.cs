using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace LuCiInstaller.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     {
          if (value is bool boolean)
          {
               return boolean ? Visibility.Visible : Visibility.Collapsed;
          }
          return Visibility.Collapsed;
     }

     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
     {
          if (value is Visibility visibility)
          {
               return visibility == Visibility.Visible;
          }
          return false;
     }
}
public class InverseBooleanToVisibilityConverter : IValueConverter
{
     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     {
          if (value is bool boolean)
          {
               return boolean ? Visibility.Collapsed : Visibility.Visible;
          }
          return Visibility.Visible;
     }

     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
     {
          if (value is Visibility visibility)
          {
               return visibility != Visibility.Visible;
          }
          return true;
     }
}
