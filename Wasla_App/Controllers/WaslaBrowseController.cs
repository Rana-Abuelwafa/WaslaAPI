using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla_App.services.Client;
using WaslaApp.Data;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.PackagesAndServices;
using static System.Net.Mime.MediaTypeNames;

namespace Wasla_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaslaBrowseController : ControllerBase
    {
        //private readonly CustomViewRendererService _viewService;
        private readonly ILogger<WaslaClientController> _logger;
        private readonly IWaslaService _waslaService;

        public WaslaBrowseController(IWaslaService waslaService, ILogger<WaslaClientController> logger)
        {
           
            _waslaService = waslaService;
            _logger = logger;
        }
        [HttpPost("GetPricingPackageWithService")]
        public async Task<IActionResult> GetPricingPackageWithService(LangReq req)
        {
            return Ok(await _waslaService.GetPricingPackageWithService(req));
        }
        [HttpPost("GetSearchResult")]
        public async Task<IActionResult> GetSearchResult(SearchCls req)
        {
            return Ok(await _waslaService.GetSearchResult(req));
        }

    }
}
