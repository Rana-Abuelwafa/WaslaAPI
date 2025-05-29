using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla_App.services;
using WaslaApp.Data;
using WaslaApp.Data.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Wasla_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaslaBrowseController : ControllerBase
    {
        private readonly CustomViewRendererService _viewService;
        private readonly ILogger<WaslaClientController> _logger;
        private readonly IWaslaService _waslaService;

        public WaslaBrowseController(CustomViewRendererService viewService,IWaslaService waslaService, ILogger<WaslaClientController> logger)
        {
            _viewService = viewService;
            _waslaService = waslaService;
            _logger = logger;
        }
        [HttpPost("GetPricingPackageWithService")]
        public async Task<IActionResult> GetPricingPackageWithService(LangReq req)
        {
            return Ok(await _waslaService.GetPricingPackageWithService(req));
        }
        [HttpPost("SendTestEmail2Async")]
        public async Task<IActionResult> SendTestEmail2Async()
        {
            var templatePath = Path.Combine("/Views/Email" + "/", "Test.cshtml");
            //var templatePath = "~/Views/Email/Test.cshtml";
            var msg = await _viewService.RenderViewToStringAsync(templatePath, ("Foo", "Bar"), ControllerContext);

            return Ok(msg);
        }
    }
}
