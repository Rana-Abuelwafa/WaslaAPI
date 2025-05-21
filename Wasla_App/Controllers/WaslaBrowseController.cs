using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla_App.services;
using WaslaApp.Data.Models;

namespace Wasla_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaslaBrowseController : ControllerBase
    {
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
    }
}
