
﻿using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
﻿using LuCiInstaller.VersionExtensions;
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
          public static string ConnectionString = "Data Source=mssql-183717-0.cloudclusters.net,10085;Persist Security Info=True;User ID=admin;Password=Duytan59nuce;Trust Server Certificate=True";

          protected override void OnStartup(StartupEventArgs e)
          {
               base.OnStartup(e);

               // Bạn cũng có thể cấu hình chuỗi kết nối ở đây nếu cần
               // string connectionString = "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;";

               using (SqlConnection connection = new SqlConnection(ConnectionString))
               {
                    try
                    {
                         connection.Open();
                         System.Windows.MessageBox.Show("Connected to Server");
                    }
                    catch (Exception ex)
                    {
                         System.Windows.MessageBox.Show("Failed to connect to database: " + ex.Message);
                    }
               }
          }
          protected override void OnExit(ExitEventArgs e)
          {
               base.OnExit(e);
          }
     }

}
