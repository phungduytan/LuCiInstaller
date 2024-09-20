using ControlzEx.Standard;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuCiInstaller.Models
{
     public class LuCiBimDatabaseService
     {
          private string connectionString;

          public LuCiBimDatabaseService(string connectionString)
          {
               this.connectionString = connectionString;
          }

          // Thêm bản ghi vào Table1
          public void AddToTable1(LuCiBimAccount ac)
          {
               using (SqlConnection connection = new SqlConnection(connectionString))
               {
                    connection.Open();
                    string query = "INSERT INTO Account (Username, Password, Email, FullName,PhoneNumber,IsAdmin,CreateDate,LastLoginDate,IsActive) VALUES (@Username, @Password, @Email, @FullName,@PhoneNumber,@IsAdmin,@CreateDate,@LastLoginDate,@IsActive)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", ac.Username);
                    command.Parameters.AddWithValue("@Password", ac.PasswordHash);
                    command.Parameters.AddWithValue("@Email", ac.Email);
                    command.Parameters.AddWithValue("@FullName", ac.FullName);
                    command.Parameters.AddWithValue("@PhoneNumber", ac.PhoneNumber);
                    command.Parameters.AddWithValue("@IsAdmin", ac.IsAdmin);
                    command.Parameters.AddWithValue("@CreateDate", ac.CreateDate);
                    command.Parameters.AddWithValue("@LastLoginDate", ac.LastLoginDate);
                    command.Parameters.AddWithValue("@IsActive", ac.IsActive);

                    command.ExecuteNonQuery();
               }
          }

          // Sửa bản ghi trong Table1
          public void UpdateTable1(LuCiBimAccount ac)
          {
               using (SqlConnection connection = new SqlConnection(connectionString))
               {
                    connection.Open();
                    string query = "UPDATE Account SET Username = @Username, Password = @Password, Password = @Password, Email = @Email, FullName = @FullName, PhoneNumber = @PhoneNumber, IsAdmin = @IsAdmin, CreateDate = @CreateDate, IsActive = @IsActive WHERE AccountId = @AccountId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", ac.Username);
                    command.Parameters.AddWithValue("@Password", ac.PasswordHash);
                    command.Parameters.AddWithValue("@Email", ac.Email);
                    command.Parameters.AddWithValue("@FullName", ac.FullName);
                    command.Parameters.AddWithValue("@PhoneNumber", ac.PhoneNumber);
                    command.Parameters.AddWithValue("@IsAdmin", ac.IsAdmin);
                    command.Parameters.AddWithValue("@CreateDate", ac.CreateDate);
                    command.Parameters.AddWithValue("@LastLoginDate", ac.LastLoginDate);
                    command.Parameters.AddWithValue("@IsActive", ac.IsActive);
                    command.ExecuteNonQuery();
               }
          }

          // Xóa bản ghi khỏi Table1
          public void DeleteFromTable1(int id)
          {
               using (SqlConnection connection = new SqlConnection(connectionString))
               {
                    connection.Open();
                    string query = "DELETE FROM Table1 WHERE AccountId = @AccountId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AccountId", id);
                    command.ExecuteNonQuery();
               }
          }
     }
}
