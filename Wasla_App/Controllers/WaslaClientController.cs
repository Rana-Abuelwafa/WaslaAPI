﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wasla_App.services;
using WaslaApp.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace Wasla_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WaslaClientController : ControllerBase
    {
        private readonly ILogger<WaslaClientController> _logger;
        private readonly IWaslaService _waslaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WaslaClientController(IWaslaService waslaService, IHttpContextAccessor httpContextAccessor, ILogger<WaslaClientController> logger) {
            _waslaService = waslaService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        #region "registration questions"

        //get registration Question List only
        [HttpPost("getQuesList")]
        public async Task<IActionResult> getQuesList(QuesLstReq req)
        {

            return Ok(await _waslaService.getRegistrationQuestionList(req.lang));
        }
        //get registration Questions List with user's answers
        [HttpPost("getQuesWithAnswers")]
        public  IActionResult getQuesWithAnswers(QuesLstReq req)
        {
            string clientId = string.Empty;

            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");

            }
            return Ok(_waslaService.getQuesWithAnswers(clientId, req.lang));
        }

        //save user's answers for registerations questions
        [HttpPost("saveRegistrationSteps")]
        public  IActionResult saveRegistrationSteps(List<RegistrationAnswer> lst)
        {
            string? clientId = string.Empty;
            string? FullName = string.Empty;
            string? email = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
                FullName = _httpContextAccessor.HttpContext.User.FindFirstValue("FullName");
                email = _httpContextAccessor.HttpContext.User.FindFirstValue("Email");
            }
            

            return  Ok(_waslaService.saveRegistrationSteps(lst, clientId , FullName, email));
        }

        [HttpPost("saveClientCopoun")]
        public IActionResult saveClientCopoun()
        {
            string? clientId = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
              
            }
            string copounAuto = HelperCls.getCopounText();
            ClientCopoun copoun = new ClientCopoun { client_id = clientId, copoun = copounAuto, id = 0, start_date = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), end_date = DateOnly.Parse("2025-06-06") };

            return Ok(_waslaService.saveClientCopoun(copoun));
        }

        #endregion


        #region "Profile"
        [HttpPost("SaveClientServices")]
        public IActionResult saveClientServices(List<ClientServiceCast> lst)
        {
            string? clientId = string.Empty;

            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");

            }

            return Ok( _waslaService.saveClientServices(lst,clientId));
        }

        [HttpPost("GetProduct_Tree")]
        public async Task<IActionResult> GetProduct_Tree()
        {
            string? clientId = string.Empty;

            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");

            }

            return Ok(await _waslaService.GetProduct_Tree(clientId));
        }
        [HttpPost("GetPaymentMethods")]
        public async Task<IActionResult> GetPaymentMethods()
        {

            return Ok(await _waslaService.GetPaymentMethods());
        }

        [HttpPost("GetClientBrands")]
        public async Task<IActionResult> GetClientBrands()
        {

            string? clientId = string.Empty;
          
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
             
            }

            return Ok(await _waslaService.GetClientBrands(clientId));
        }

        [HttpPost("GetClientProfiles")]
        public async Task<IActionResult> GetClientProfiles()
        {
            string? clientId = string.Empty;

            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");

            }
            return Ok(await _waslaService.GetClientProfiles(clientId));
        }


        [HttpPost("saveMainProfile")]
        public IActionResult saveMainProfile(ClientProfileCast profile)
        {
            string? clientId = string.Empty;
            string? FullName = string.Empty;
            string? email = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
                FullName = _httpContextAccessor.HttpContext.User.FindFirstValue("FullName");
                email = _httpContextAccessor.HttpContext.User.FindFirstValue("Email");
            }

            profile.client_id = clientId;
            profile.client_name = FullName;
            profile.client_email = email;
            return Ok(_waslaService.saveMainProfile(profile));
        }

        [HttpPost("saveClientBrand")]
        public IActionResult saveClientBrand(ClientBrand brand)
        {
            string clientId = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
            }

            brand.client_Id = clientId;
            return Ok(_waslaService.saveClientBrand(brand));
        }

        [HttpPost("saveProfileImage")]
        public IActionResult saveProfileImage(ImgCls cls)
        {
            string? clientId = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
            }
            var path = Path.Combine("images" + "//", cls.img.FileName);
            //var path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images" + "//", cls.img.FileName);
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    cls.img.CopyTo(stream);
                    stream.Close();
                }
                _logger.LogInformation("image saved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError( "image save exception: " + ex.Message);
            }
           
            ClientImage image = new ClientImage
            {
                client_id = clientId,
                img_name = cls.img.FileName,
                img_path = path,
                type = 1  //mean save profile image
            };
            
            return Ok(_waslaService.saveProfileImage(image));
        }

        [HttpPost("GetProfileImage")]
        public async Task<IActionResult> GetProfileImage()
        {
            string? clientId = string.Empty;

            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");

            }
            return Ok(await _waslaService.GetProfileImage(clientId));
        }
        #endregion




    }
}
