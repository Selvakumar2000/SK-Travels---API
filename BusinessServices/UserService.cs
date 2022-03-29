using Microsoft.Extensions.Configuration;
using SKTravelsApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SKTravelsApp.BusinessServices
{
    public class UserService
    {
        private readonly IConfiguration _config;
        public UserService(IConfiguration config)
        {
            _config = config;
        }

        public int GetUserExistance(string email)
        {
            int status = 0;
            try
            {
                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("GetUserExistance", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    status = (int)sdr["Out"];
                }

                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public long Register(AppUser user)
        {
            long i = 0;
            try
            {
                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("Register", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                cmd.Parameters.AddWithValue("@IsEmailSent", user.IsEmailSent);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    i = (long)sdr["UserID"];
                }

                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }

        public AppUser GetUserDetails(string email)
        {
            try
            {
                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("GetUserDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader sdr = cmd.ExecuteReader();

                AppUser user = new AppUser();
                while(sdr.Read())
                {
                    user.UserID = (long)sdr["UserID"];
                    user.FullName = (string)sdr["FullName"];
                    user.Email = (string)sdr["Email"];
                    user.PasswordHash = (byte[])sdr["PasswordHash"];
                    user.PasswordSalt = (byte[])sdr["PasswordSalt"];
                }

                con.Close();
                return user;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveToken(string token, string email)
        {
            string fullname = "";
            try
            {
                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SaveUserToken", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Token", token);

                SqlDataReader sdr = cmd.ExecuteReader();

                AppUser user = new AppUser();
                while (sdr.Read())
                {
                    fullname = (string)sdr["FullName"];                    
                }

                con.Close();
                return fullname;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ValidateUser(byte[] passwordHash, byte[] passwordSalt, string email, string token)
        {
            int status = 0;
            try
            {
                var connectionString = _config.GetConnectionString("SKTravelsCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("ValidateUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    status = (int)sdr["Out"];
                }

                con.Close();
                return status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
