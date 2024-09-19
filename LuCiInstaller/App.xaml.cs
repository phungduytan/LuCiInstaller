using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LuCiInstaller
{
     /// <summary>
     /// Interaction logic for App.xaml
     /// </summary>
     public partial class App : Application
     {
          public static string ConnectionString = "Server=mssql-183717-0.cloudclusters.net,10085;Database=LuCiBimData;User Id=admin;Password=Duytan59nuce;";

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
                         // Thực hiện các thao tác với cơ sở dữ liệu tại đây nếu cần
                    }
                    catch (Exception ex)
                    {
                         MessageBox.Show("Failed to connect to database: " + ex.Message);
                    }
               }
          }
     }

}
