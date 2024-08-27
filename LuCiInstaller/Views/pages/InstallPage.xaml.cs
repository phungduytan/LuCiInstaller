﻿using LuCiInstaller.VersionExtensions;
using LuCiInstaller.ViewModel;
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
    /// Interaction logic for InstallPage.xaml
    /// </summary>
    public partial class InstallPage : Page
    {
        public InstallPage( LuCiVersion cloudVersion, LuCiVersionFactory luCiVersionFactory)
        {
            InitializeComponent();
            DataContext = new InstallViewModel(cloudVersion, luCiVersionFactory, new DownloadAndInstallPage());
        }
    }
}
