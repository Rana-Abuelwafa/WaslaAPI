﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla_App.services;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace Wasla_App.Controllers
{
    [Authorize(Roles = "Admin")]
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

        #region "questions"
        [HttpPost("saveQuestions")]
        public IActionResult saveQuestions(RegistrationQuestion ques)
        {

            return Ok(_waslaService.saveQuestions(ques));
        }
        [HttpPost("deleteQuestions")]
        public IActionResult deleteQuestions(RegistrationQuestion ques)
        {

            return Ok(_waslaService.deleteQuestions(ques));
        }
        [HttpPost("getAdminQuesList")]
        public async Task<IActionResult> getQuesList(QuesLstReq req)
        {
            

            return Ok(await _waslaService.getRegistrationQuestionList(req.lang));
        }

        #endregion "questions"

        #region "product"

        [HttpPost("GetProduct")]
        public async Task<IActionResult> GetProduct(ServiceReq req)
        {
            return Ok(await _waslaService.GetProduct(req));
        }
        [HttpPost("GetProduct_Tree")]
        public async Task<IActionResult> GetProduct_Tree(LangReq req)
        {
            return Ok(await _waslaService.GetProduct_Tree("admin",req.lang));
        }

        [HttpPost("SaveProduct")]
        public IActionResult SaveProduct(Service service)
        {
            return Ok(_waslaService.SaveProduct(service));
        }
        #endregion "product"


        #region "packages & services"

        [HttpPost("SavePricingPackageCurrency")]
        public IActionResult SavePricingPackageCurrency(PricingPkgCurrencyCast req)
        {
            return Ok( _waslaService.SavePricingPackageCurrency(req));
        }

        [HttpPost("GetPricingPkgCurrency")]
        public async Task<IActionResult> GetPricingPkgCurrency(PricingPkgCurrencyReq req)
        {
            return Ok(await _waslaService.GetPricingPkgCurrency(req));
        }
        [HttpPost("GetPricingPackageWithService")]
        public async Task<IActionResult> GetPricingPackageWithService(LangReq req)
        {
            return Ok(await _waslaService.GetPricingPackageWithService(req));
        }

        [HttpPost("SavePricingPackage")]
        public IActionResult SavePricingPackage(PricingPackage req)
        {
            return Ok(_waslaService.SavePricingPackage(req));
        }
        [HttpPost("SavePricingPKgServices")]
        public IActionResult SavePricingPKgServices(List<PricingPkgService> service)
        {
            return Ok(_waslaService.SavePricingPKgServices(service));
        }
        #endregion
    }
}
