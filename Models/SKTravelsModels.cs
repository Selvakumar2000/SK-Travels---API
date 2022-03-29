using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKTravelsApp.Models
{
    public class AppUser
    {
        public long UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int IsEmailSent { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IsEmailSent { get; set; }
    }

    public class UserDto
    {
        public long UserID { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class ResetPasswordLinkGenerateDto
    {
        public string Email { get; set; }
        public string ClientUrl { get; set; }
    }

    public class TravelDaysDto
    {
        public long UserID { get; set; }
        public int NumberOfDays { get; set; }
        public long MainCityID { get; set; }
    }

    public class VisitingCitiesDto
    {
        public long UserID { get; set; }
        public long MainCityID { get; set; }
        public string[] VisitingcityIDs { get; set; }       

    }
}
