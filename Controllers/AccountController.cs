using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SKTravelsApp.BusinessServices;
using SKTravelsApp.Helpers;
using SKTravelsApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SKTravelsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseAPIController
    {

        private readonly TokenService _tokenService;
        private readonly EmailSender _emailSender;
        private readonly UserService _userService;
        public AccountController(TokenService tokenService, EmailSender emailSender, UserService userService)
        {
            _tokenService = tokenService;
            _emailSender = emailSender;
            _userService = userService;
        }

        [HttpPost("Register")]
        public ActionResult<UserDto> Register(RegisterDto registerDto)
        {
            try
            {
                if (_userService.GetUserExistance(registerDto.Email) == 1)
                {
                    return BadRequest("Email is already taken");
                }

                if (SendMail(registerDto.Email, registerDto.FullName))
                {
                    registerDto.IsEmailSent = 1;
                }
                else
                {
                    registerDto.IsEmailSent = 0;
                }

                using var hmac = new HMACSHA512();
                var user = new AppUser
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key,
                    IsEmailSent = registerDto.IsEmailSent

                };

                long userID = _userService.Register(user);

                return new UserDto
                {
                    UserID = userID,
                    FullName = user.FullName,
                    Token = _tokenService.CreateToken(user)
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }           
        }

        [HttpPost("Login")]
        public ActionResult<UserDto> Login(LoginDto loginDto)
        {
            try
            {
                AppUser user = _userService.GetUserDetails(loginDto.Email);

                if (user.Email == null)
                {
                    return BadRequest("Email ID is Incorrect");
                }

                var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i])
                    {
                        return Unauthorized("Password is Incorrect");
                    }
                }
                return new UserDto
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    Token = _tokenService.CreateToken(user)
                };

                //The SequenceEqual method compares the number of items and their values for primitive data types.
                //if (computedHash.SequenceEqual(user.PasswordHash))
                //{
                //    return new UserDto
                //    {
                //        FullName = user.FullName,
                //        Token = _tokenService.CreateToken(user)
                //    };
                //}
                //else
                //{
                //    return Unauthorized("Password is Incorrect");
                //}

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("ResetPasswordLink")]
        public ActionResult<string> GenerateResetPasswordLink(ResetPasswordLinkGenerateDto details)
        {
            try
            {
                if (_userService.GetUserExistance(details.Email) == 1)
                {
                    string token = Guid.NewGuid().ToString();
                    string fullname = _userService.SaveToken(token, details.Email);
                    var param = new Dictionary<string, string>
                    {
                        {"token", token },
                        {"email", details.Email }
                    };

                    //For QueryHelpers Install --> Microsoft.AspNetCore.WebUtilities
                    var callback = QueryHelpers.AddQueryString(details.ClientUrl, param);
                    var message = new Message(new string[] { details.Email }, "Password Reset Link For Sk Travels India Portal", callback);

                    _emailSender.SendResetPasswordLink(message, fullname);

                    return Ok("Password Reset link has been sent, please check your email.");
                }
                else
                {
                    return BadRequest("User Doesnot Exists");
                }               
            }
            catch(Exception ex)
            {
                throw ex;
            }   
        }

        [HttpPost("ResetPassword")]
        public ActionResult<string> ResetPassword(ResetPasswordDto details)
        {
            int isValidUser;
            try
            {
                using var hmac = new HMACSHA512();
                byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(details.Password));
                byte[] passwordSalt = hmac.Key;
                isValidUser = _userService.ValidateUser(passwordHash, passwordSalt, details.Email, details.Token);
                if(isValidUser == 1)
                {
                    return Ok("Password Changed Successfully");
                }
                else
                {
                    return BadRequest("User Credentials Are Invalid");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool SendMail(string email, string username)
        {
            try
            {
                var message = new Message(new string[] { email }, "Welcome To SK Travels India",
                                      "Greetings from SK Travels");
                _emailSender.SendEmail(message, username);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
