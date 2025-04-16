﻿using Microsoft.AspNetCore.Mvc;
using Wasla_App.services;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace Wasla_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaslaAdminController : Controller
    {
        private readonly IWaslaService _waslaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WaslaAdminController(IWaslaService waslaService, IHttpContextAccessor httpContextAccessor)
        {
            _waslaService = waslaService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("saveQuestions")]
        public IActionResult saveQuestions(RegistrationQuestion ques)
        {

            return Ok(_waslaService.saveQuestions(ques));
        }
        [HttpPost("getAdminQuesList")]
        public async Task<IActionResult> getQuesList(QuesLstReq req)
        {

            return Ok(await _waslaService.getRegistrationQuestionList(req.lang));
        }
    }
}
