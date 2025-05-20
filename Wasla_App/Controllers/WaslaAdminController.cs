using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetProduct(ProductReq req)
        {
            return Ok(await _waslaService.GetProduct(req));
        }
        [HttpPost("GetProduct_Tree")]
        public async Task<IActionResult> GetProduct_Tree(LangReq req)
        {
            return Ok(await _waslaService.GetProduct_Tree("admin",req.lang));
        }

        [HttpPost("SaveProduct")]
        public IActionResult SaveProduct(Product product)
        {
            return Ok(_waslaService.SaveProduct(product));
        }
        #endregion "product"


        #region "packages & services"
        [HttpPost("GetPricingPackage")]
        public async Task<IActionResult> GetPricingPackage(LangReq req)
        {
            return Ok(await _waslaService.GetPricingPackage(req));
        }

        [HttpPost("SavePricingPackage")]
        public IActionResult SavePricingPackage(PricingPackageCast req)
        {
            return Ok(_waslaService.SavePricingPackage(req));
        }
        [HttpPost("AssignPackageToServices")]
        public IActionResult AssignPackageToServices(PackageServiceAssignReq req)
        {
            return Ok(_waslaService.AssignPackageToServices(req));
        }
        #endregion
    }
}
