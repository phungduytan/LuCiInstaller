using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuCiInstaller.Models
{
     public class LuCiBimAccount
     {
          public int AccountId { get; set; }
          public string Username { get; set; }
          public string Password { get; set; }
          public string Email {  get; set; }
          public string FullName { get; set; }
          public int PhoneNumber { get; set; }
          public bool IsAdmin { get; set; }
          public DateTime CreateDate { get; set; }
          public DateTime LastLoginDate { get; set; }
          public bool IsActive { get; set; }
     }
     public class LuCiBimLicense
     {
          public int LicenseID { get; set; }
          public int AccountId { get; set; }
          public string LicenseType { get; set; }
          public DateTime LicenseExpiryDate { get; set; }
          public bool IsActive { get; set; }
          public DateTime CreatedDate { get; set; }
          public DateTime UpdatedDate { get; set; }
     }
}
