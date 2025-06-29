using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla_App.services.Admin;
using Wasla_App.services.Client;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.PackagesAndServices;
using WaslaApp.Data.Models.profile;

namespace Wasla_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WaslaAdminController : Controller
    {
        private readonly IWaslaService _waslaService;
        private readonly IAdminWaslaService _adminWaslaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WaslaAdminController(IWaslaService waslaService, IHttpContextAccessor httpContextAccessor, IAdminWaslaService adminWaslaService)
        {
            _waslaService = waslaService;
            _adminWaslaService = adminWaslaService;
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

        //[HttpPost("SavePricingPackageCurrency")]
        //public IActionResult SavePricingPackageCurrency(PricingPkgCurrencyCast req)
        //{
        //    return Ok( _waslaService.SavePricingPackageCurrency(req));
        //}

        //[HttpPost("GetPricingPkgCurrency")]
        //public async Task<IActionResult> GetPricingPkgCurrency(PricingPkgCurrencyReq req)
        //{
        //    return Ok(await _waslaService.GetPricingPkgCurrency(req));
        //}
        [HttpPost("GetPricingPackages")]
        public async Task<IActionResult> GetPricingPackages(PricingPackageReq req)
        {
            return Ok(await _waslaService.GetPricingPackages(req));
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
        [HttpPost("SavePricingPKgFeatureLst")]
        public IActionResult SavePricingPKgFeatureLst(List<PricingPkgFeature> lst)
        {
            return Ok(_waslaService.SavePricingPKgFeatureLst(lst));
        }
        [HttpPost("GetPricingPkgFeatures")]
        public  IActionResult GetPricingPkgFeatures(PricingPkgFeatureReq req)
        {
            return Ok( _waslaService.GetPricingPkgFeatures(req));
        }
        #endregion

        #region new Services &packages
        [HttpPost("SaveMainPackage")]
        public IActionResult SaveMainPackage(PackageSaveReq row)
        {
            return Ok(_adminWaslaService.SaveMainPackage(row));
        }

        [HttpPost("AssignPackagesToService")]
        public IActionResult AssignPackagesToService(ServicePackageReq row)
        {
            return Ok(_adminWaslaService.AssignPackagesToService(row));
        }

        [HttpPost("AssignPriceToPackage")]
        public IActionResult AssignPriceToPackage(PackagePriceSaveReq row)
        {
            return Ok(_adminWaslaService.AssignPriceToPackage(row));
        }

        [HttpPost("getMainPackages")]
        public async Task<IActionResult> getMainPackages()
        {
            return Ok(await _adminWaslaService.getMainPackages());
        }

        [HttpPost("getMainServices")]
        public async Task<IActionResult> getMainServices()
        {
            return Ok(await _adminWaslaService.getMainServices());
        }

        [HttpPost("getServiceGrpWithPkgs")]
        public IActionResult getServiceGrpWithPkgs()
        {
            return Ok(_adminWaslaService.getServiceGrpWithPkgs());
        }
        [HttpPost("getServicePackagePrice")]
        public async Task<IActionResult> getServicePackagePrice(PackagesPriceReq req)
        {
            return Ok(await _adminWaslaService.getServicePackagePrice(req.service_package_id));
        }

        [HttpPost("getMainFeatures")]
        public async Task<IActionResult> getMainFeatures()
        {
            return Ok(await _adminWaslaService.getMainFeatures());
        }
        [HttpPost("getPackageFeatures")]
        public async Task<IActionResult> getPackageFeatures(PackageFeatureReq req)
        {
            return Ok(await _adminWaslaService.getPackageFeatures(req));
        }

        [HttpPost("AssignFeaturesToPackage")]
        public IActionResult AssignFeaturesToPackage(packages_feature row)
        {
            return Ok(_adminWaslaService.AssignFeaturesToPackage(row));
        }
        #endregion
    }
}
