﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wasla_App.services;
using WaslaApp.Data;
using WaslaApp.Data.Entities;

namespace Wasla_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WaslaClientController : ControllerBase
    {
        private readonly IWaslaService _waslaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WaslaClientController(IWaslaService waslaService, IHttpContextAccessor httpContextAccessor) {
            _waslaService = waslaService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("getQuesList")]
        public async Task<IActionResult> getQuesList(string lang)
        {

            return Ok(await _waslaService.getRegistrationQuestionList(lang));
        }

        [HttpPost("saveQuesList")]
        public  IActionResult saveQuesList(List<RegistrationAnswer> lst)
        {
            string clientId = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
            }
            

            return  Ok(_waslaService.saveRegistrationSteps(lst, clientId));
        }
    }
}
