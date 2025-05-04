
using Mails_App;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Wasla_Auth_App.Models;
using Wasla_Auth_App.Services;
using System;
using System.Collections.Generic;
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
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email,FirstName=model.FirstName,LastName=model.LastName,TwoFactorEnabled=true };
            var result = await _userManager.CreateAsync(user, model.Password);
           
            if (result.Succeeded)
            {
                //var token = GenerateJwtToken(user);
                //send otp to email
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
               
                var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                string fileName = "OTPMail_" + model.lang + ".html";
                MailData mailData = Utils.GetOTPMailData(model.lang, user.FirstName + " " + user.LastName, otp, model.Email);
                //string htmlBody = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                //htmlBody = htmlBody.Replace("@user", model.FirstName + " " + model.LastName);
                //htmlBody = htmlBody.Replace("@otp", otp);
                //MailData mailData = new MailData
                //{
                //    EmailToId = user.Email,
                //    EmailToName = user.Email,
                //    EmailSubject = UtilsCls.GetMailSubjectByLang(model.lang, 2),
                //    EmailBody = htmlBody
                //};
                //MailData mailData = new MailData
                //{
                //    EmailBody= "<p>Thank you For Registering account</p><br /><p>"+ otp +"<p/>",
                //    EmailSubject="Mail Confirmation",
                //    EmailToId=user.Email,
                //    EmailToName=user.Email
                //};

                Mail_Service.SendMail(mailData);
                return Ok(new User
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName=user.LastName,
                    Email=user.Email,
                    isSuccessed = result.Succeeded,
                    msg = "User created successfully" ,
                    AccessToken=null,
                    RefreshToken=null,
                    Id=user.Id,
                    EmailConfirmed = user.EmailConfirmed,
                    GoogleId = user.GoogleId,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    completeprofile=user.completeprofile,
                    
                });
            }
            else
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                return Ok(new User
                {
                    UserName = "",
                    Email = "",
                    FirstName ="",
                    LastName="",
                    isSuccessed = result.Succeeded,
                    msg = errors,
                    AccessToken = "",
                    RefreshToken = "",
                    Id=null,
                    completeprofile=0
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
                    if(user.EmailConfirmed == false)
                    {
                        //send otp to email
                        var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                        MailData mailData = Utils.GetOTPMailData(model.lang, user.FirstName + " " + user.LastName, otp, model.Email);
                        //string fileName = "OTPMail_" + model.lang + ".html";
                        //string htmlBody = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                        //htmlBody = htmlBody.Replace("@user", user.FirstName + " " + user.LastName);
                        //htmlBody = htmlBody.Replace("@otp", otp);
                        //MailData mailData = new MailData
                        //{
                        //    EmailToId = user.Email,
                        //    EmailToName = user.Email,
                        //    EmailSubject = UtilsCls.GetMailSubjectByLang(model.lang, 2),
                        //    EmailBody = htmlBody
                        //};
                        //MailData mailData = new MailData
                        //{
                        //    EmailBody = "<p>Thank you For Registering account</p><br /><p>" + otp + "<p/>",
                        //    EmailSubject = "Mail Confirmation",
                        //    EmailToId = user.Email,
                        //    EmailToName = user.Email
                        //};

                        Mail_Service.SendMail(mailData);
                        return Ok(new User
                        {
                            
                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            isSuccessed = true,
                            msg = $"We have sent an OTP to your Email {user.Email}",
                            AccessToken = null,
                            RefreshToken = null,
                            Id = user.Id,
                            EmailConfirmed=user.EmailConfirmed,
                            GoogleId=user.GoogleId,
                            TwoFactorEnabled=user.TwoFactorEnabled,
                            completeprofile = user.completeprofile,
                        });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user,false);
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
                            Id = user.Id,
                            EmailConfirmed = user.EmailConfirmed,
                            GoogleId = user.GoogleId,
                            TwoFactorEnabled = user.TwoFactorEnabled,
                            completeprofile = user.completeprofile,
                        });
                    }
                   

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
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName,GoogleId="1",TwoFactorEnabled = true };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                //send otp to email
                //await _signInManager.SignOutAsync();
                //await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

                var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                MailData mailData = Utils.GetOTPMailData(model.lang, user.FirstName + " " + user.LastName, otp, model.Email);
                //string fileName = "OTPMail_" + model.lang + ".html";
                //string htmlBody = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                //htmlBody = htmlBody.Replace("@user", user.FirstName + " " + user.LastName);
                //htmlBody = htmlBody.Replace("@otp", otp);
                //MailData mailData = new MailData
                //{
                //    EmailToId = user.Email,
                //    EmailToName = user.Email,
                //    EmailSubject = UtilsCls.GetMailSubjectByLang(model.lang, 2),
                //    EmailBody = htmlBody
                //};
                //MailData mailData = new MailData
                //{
                //    EmailBody = "<p>Thank you For Registering account</p><br /><p>" + otp + "<p/>",
                //    EmailSubject = "Mail Confirmation",
                //    EmailToId = user.Email,
                //    EmailToName = user.Email
                //};

                Mail_Service.SendMail(mailData);
                return Ok(new User
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    isSuccessed = result.Succeeded,
                    msg = "User created successfully",
                    AccessToken = null,
                    RefreshToken = null,
                    Id = user.Id,
                    EmailConfirmed = user.EmailConfirmed,
                    GoogleId = user.GoogleId,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    completeprofile = user.completeprofile,
                });
            }
            else
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                return Ok(new User
                {
                    UserName = "",
                    Email = "",
                    FirstName = "",
                    LastName = "",
                    isSuccessed = result.Succeeded,
                    msg = errors,
                    AccessToken = "",
                    RefreshToken = "",
                    Id=null,
                    completeprofile=0
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
                    if (user.EmailConfirmed == false)
                    {
                        //send otp to email
                        var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                        MailData mailData = Utils.GetOTPMailData(model.lang, user.FirstName + " " + user.LastName,otp,model.Email);
                        //string fileName = "OTPMail_" + model.lang + ".html";
                        //string htmlBody = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                        //htmlBody = htmlBody.Replace("@user", user.FirstName + " " + user.LastName);
                        //htmlBody = htmlBody.Replace("@otp", otp);
                        //MailData mailData = new MailData
                        //{
                        //    EmailToId = user.Email,
                        //    EmailToName = user.Email,
                        //    EmailSubject = UtilsCls.GetMailSubjectByLang(model.lang, 2),
                        //    EmailBody = htmlBody
                        //};
                        //MailData mailData = new MailData
                        //{
                        //    EmailBody = "<p>Thank you For Registering account</p><br /><p>" + otp + "<p/>",
                        //    EmailSubject = "Mail Confirmation",
                        //    EmailToId = user.Email,
                        //    EmailToName = user.Email
                        //};

                        Mail_Service.SendMail(mailData);
                        return Ok(new User
                        {

                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            isSuccessed = true,
                            msg = $"We have sent an OTP to your Email {user.Email}",
                            AccessToken = null,
                            RefreshToken = null,
                            Id = user.Id,
                            EmailConfirmed = user.EmailConfirmed,
                            GoogleId = user.GoogleId,
                            TwoFactorEnabled = user.TwoFactorEnabled,
                            completeprofile = user.completeprofile,
                        });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, false);
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
                            Id = user.Id,
                            EmailConfirmed = user.EmailConfirmed,
                            GoogleId = user.GoogleId,
                            TwoFactorEnabled = user.TwoFactorEnabled,
                            completeprofile = user.completeprofile,
                        });
                    }
                }
                else
                    return StatusCode(StatusCodes.Status401Unauthorized,
                      new User
                      {
                          isSuccessed = false,
                          msg = "user Not Found",

                      });
                //return Unauthorized(new User
                //    {
                //        isSuccessed = false,
                //        msg = "user Not Found",

                //    });


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                      new User
                      {
                          isSuccessed = false,
                          msg = e.Message,

                      });
                //return Unauthorized(new User
                //{

                //    isSuccessed = false,
                //    msg = "user Not Found",

                //});
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
                            Id=user.Id,
                            completeprofile = user.completeprofile,
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

        [HttpPost("ConfirmOTP")]
        public async Task<IActionResult> confirmOTP([FromBody] OTPConfirmCls model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var isCodeValid = await _userManager.VerifyTwoFactorTokenAsync(user,"Email",model.otp);
                    // var signIn = await _signInManager.TwoFactorSignInAsync("Email", model.otp, false, false);
                    //if (signIn.Succeeded)
                    if (isCodeValid)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
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
                            Id = user.Id,
                            EmailConfirmed = user.EmailConfirmed,
                            GoogleId = user.GoogleId,
                            TwoFactorEnabled = user.TwoFactorEnabled,
                            completeprofile = user.completeprofile,
                        });
                    }
                    else
                    {
                        return Ok(new User
                        {
                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Id = user.Id,
                            EmailConfirmed = user.EmailConfirmed,
                            GoogleId = user.GoogleId,
                            TwoFactorEnabled = user.TwoFactorEnabled,
                            isSuccessed = false,
                            msg = $"Invalid Code",
                            AccessToken = "",
                            RefreshToken = "",
                            completeprofile = user.completeprofile,
                        });
                      
                    }
                }
                return Unauthorized(new User
                {

                    isSuccessed = false,
                    msg = "user Not Found",

                });


            }
            catch (Exception e)
            {
                return Ok(new User
                {
                    UserName = "",
                    Email = "",
                    FirstName = "",
                    LastName = "",
                    isSuccessed = false,
                    msg = e.Message,
                    AccessToken = "",
                    RefreshToken = "",
                    Id = null,
                    completeprofile=0
                    
                });
            }
        }
        [HttpPost("CompleteMyProfile")]
        public async Task<IActionResult> completeMyProfile([FromBody] ModelCls model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    user.completeprofile = 1;
                    await _userManager.UpdateAsync(user);
                    var token = GenerateJwtToken(user);
                    return Ok(new User
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        isSuccessed = true,
                        msg = "updated successfully",
                        AccessToken = token,
                        RefreshToken = token,
                        Id = user.Id,
                        EmailConfirmed = user.EmailConfirmed,
                        GoogleId = user.GoogleId,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        completeprofile = user.completeprofile,
                    });
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
                return Ok(new User { isSuccessed = false, msg = e.Message, });
            }
        }
    }
}
