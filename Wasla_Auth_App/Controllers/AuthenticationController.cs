using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using NuGet.Packaging.Signing;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Wasla_Auth_App.Models;
using Wasla_Auth_App.Services;

namespace Wasla_Auth_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IMailService Mail_Service = null;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthenticationController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IConfiguration configuration , IMailService _MailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            Mail_Service = _MailService;

        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email,FirstName=model.FirstName,LastName=model.LastName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {

             
                var token = GenerateJwtToken(user);
                return Ok(new User
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName=user.LastName,
                    Email=user.Email,
                    isSuccessed = result.Succeeded,
                    msg = "User created successfully" ,
                    AccessToken=token,
                    RefreshToken=token,
                    Id=user.Id
                });
            }
            else
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                return BadRequest(new User
                {
                    UserName = "",
                    Email = "",
                    FirstName ="",
                    LastName="",
                    isSuccessed = result.Succeeded,
                    msg = errors,
                    AccessToken = "",
                    RefreshToken = "",
                    Id=null
                });
            }
            
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            try
            {
                var isAuth = await _userManager.CheckPasswordAsync(user, model.Password);
                if (user != null && isAuth)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new User
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        isSuccessed = true,
                        msg = "User login successfully",
                        AccessToken = token,
                        RefreshToken = token,
                        Id=user.Id
                    });
                }
                else
                    return Unauthorized(new User
                    {

                        isSuccessed = false,
                        msg = "mail or password is incorrect",

                    });


            }
            catch (Exception e) {
                return Unauthorized(new User
                {

                    isSuccessed = false,
                    msg = "mail or password is incorrect",

                });
            }
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            //    var claims = new[]
            //    {
            //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};
            DateTime timestamp = DateTime.Now;
            string fullName = user.FirstName + " " + user.LastName;
            var authClaims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("ClientId", user.Id.ToString()),
                        new Claim("FullName", fullName),
                        new Claim("Email", user.Email),
                        //new Claim("ClientId", user.Id.ToString()),
                        new Claim("TimeStamp",timestamp.ToString()),
                        new Claim("ActivtationTokenExpiredAt",timestamp.AddDays(14).ToString()),
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("ExternalRegister")]
        public async Task<IActionResult> ExternalRegister([FromBody] AppsRegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName,GoogleId="1" };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new User
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    isSuccessed = result.Succeeded,
                    msg = "User created successfully",
                    AccessToken = token,
                    RefreshToken = token,
                    Id=user.Id
                });
            }
            else
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                return BadRequest(new User
                {
                    UserName = "",
                    Email = "",
                    FirstName = "",
                    LastName = "",
                    isSuccessed = result.Succeeded,
                    msg = errors,
                    AccessToken = "",
                    RefreshToken = "",
                    Id=null
                });
            }

        }



        [HttpPost("LoginGmail")]
        public async Task<IActionResult> LoginGmail([FromBody] AppsRegisterModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            try
            {
            
                if (user != null)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new User
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        isSuccessed = true,
                        msg = "User login successfully",
                        AccessToken = token,
                        RefreshToken = token,
                        Id=user.Id
                    });
                }
                else
                    return Unauthorized(new User
                    {

                        isSuccessed = false,
                        msg = "user Not Found",

                    });


            }
            catch (Exception e)
            {
                return Unauthorized(new User
                {

                    isSuccessed = false,
                    msg = "user Not Found",

                });
            }
        }

        [HttpPost("SendMail")]
        public bool SendMail(MailData Mail_Data)
        {
            return Mail_Service.SendMail(Mail_Data);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> changePassword([FromBody] PasswordCls model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.userId);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var token = GenerateJwtToken(user);
                        return Ok(new User
                        {
                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            isSuccessed = true,
                            msg = "Password is changed successfully",
                            AccessToken = token,
                            RefreshToken = token,
                            Id=user.Id
                        });
                    }
                    else
                    {
                        List<IdentityError> errorList = result.Errors.ToList();
                        var errors = string.Join(", ", errorList.Select(e => e.Description));
                        return BadRequest(new User
                        {

                            isSuccessed = false,
                            msg = errors,

                        });
                    }


                }
                else
                {
                    return Unauthorized(new User
                    {

                        isSuccessed = false,
                        msg = "User Not Found",

                    });
                }


            }
            catch (Exception e)
            {
                return Unauthorized(new User
                {

                    isSuccessed = false,
                    msg = e.Message,

                });
            }
        }

    }
}
