using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LuCiInstaller.VersionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LuCiInstaller.ViewModel.PageViewModel;
public class UpdatedViewModel : ObservableObject
{
     private LuCiVersion currentVersion;
     public LuCiVersion CurrentVersion
     {
          get { return currentVersion; }
          set { value = currentVersion; OnPropertyChanged(); }
     }
     public ICommand CheckUpdateCommand { get; set; }
     public ICommand RepairCommand { get; set; }
     public ICommand UnInstallCommand { get; set; }
     public UpdatedViewModel(LuCiVersion CurrentVersion)
     {
          CurrentVersion = currentVersion;
     }
}
