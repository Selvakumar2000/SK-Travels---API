using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKTravelsApp.Entities
{
    public class AppUser
    {
        public int UserID{ get; set; }
        public string FullName{ get; set; }
        public string Email{ get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int IsEmailSent{ get; set; }
    }
}
