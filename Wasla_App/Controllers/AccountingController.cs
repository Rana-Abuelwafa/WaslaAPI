using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla_App.services.Admin;
using WaslaApp.Data.Models.admin.Accounting;
using WaslaApp.Data.Models.admin.Packages_Services;

namespace Wasla_App.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : Controller
    {
        private readonly IAdminWaslaService _adminWaslaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountingController(IHttpContextAccessor httpContextAccessor, IAdminWaslaService adminWaslaService)
        {
            _adminWaslaService = adminWaslaService;
            _httpContextAccessor = httpContextAccessor;
        }
        #region "Accounting"

        [HttpPost("GetAllInvoices")]
        public async Task<IActionResult> GetAllInvoices(GetInvoicesReq req)
        {
            return Ok(await _adminWaslaService.GetAllInvoices(req));
        }
        [HttpPost("ChangeInvoiceStatus")]
        public IActionResult ChangeInvoiceStatus(ChangeInvoiceStatusReq row)
        {
            return Ok(_adminWaslaService.ChangeInvoiceStatus(row));
        }
        #endregion
    }
}
