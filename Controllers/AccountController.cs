using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKTravelsApp.BusinessServices;
using SKTravelsApp.DTOs;
using SKTravelsApp.Entities;
using SKTravelsApp.Helpers;
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

                int status = _userService.Register(user);

                if(status == 1)
                {
                    return new UserDto
                    {
                        FullName = user.FullName,
                        Token = _tokenService.CreateToken(user)
                    };
                }
                else
                {
                    return BadRequest("Problem In Registration");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }           
        }

        [HttpPost("Login")]
        public ActionResult<UserDto> Login(LoginDto loginDto)
        {
            AppUser user = _userService.GetUserDetails(loginDto.Email);

            if(user.PasswordHash == null && user.PasswordSalt == null)
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
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            };
        }

        public bool SendMail(string email, string username)
        {
            var message = new Message(new string[] { email }, "Welcome To ShopMe Portal",
                                      "Greetings from ShopMe");

            try
            {
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
