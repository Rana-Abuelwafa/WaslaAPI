using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Wasla_App.Models;
using Wasla_App.services.Admin;
using WaslaApp.Data.Models.admin.Accounting;
using WaslaApp.Data.Models.admin.Packages_Services;
using WaslaApp.Data.Models.admin.reports;

namespace Wasla_App.Controllers
{
    [Authorize(Roles = "Admin,accountant")]
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

        #region "Reports"
        [HttpPost("GetReports")]
        public async Task<IActionResult> GetReports_Mains()
        {
            return Ok(await _adminWaslaService.GetReports_Mains());
        }
        [HttpPost("GetSummaryInvoice")]
        public async Task<IActionResult> GetSummaryInvoice(ReportReq req)
        {
            return Ok(await _adminWaslaService.GetSummaryInvoice(req));
        }
        [HttpPost("GetReportData")]
        public async Task<IActionResult> GetReportData(ReportReq req)
        {
            if (req.report_id == 1)
            {
                return Ok(await _adminWaslaService.GetSummaryInvoice(req));
            }
            else if (req.report_id == 2)
            {
                return Ok(await _adminWaslaService.GetSummaryServiceReport(req));
            }
            return Ok();
        }

        [HttpPost("PrintSummaryInvoice")]
        public IActionResult PrintSummaryInvoice(SummaryInvoiceReq req)
        {
            string format = "dd-MM-yyyy HH:mm:ss";
            string? dateF = DateTime.ParseExact(req.date_from, format, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string? dateT = DateTime.ParseExact(req.date_to, format, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            var pdfBytes = SummaryInvoiceReportPdf.GenerateAsync(dateF, dateT, req.invoices);

            return File(pdfBytes, "application/pdf", $"InvoiceSummary.pdf");
        }

        [HttpPost("PrintSummaryService")]
        public IActionResult PrintSummaryService(SummaryServiceReq req)
        {
            string format = "dd-MM-yyyy HH:mm:ss";
            string? dateF = DateTime.ParseExact(req.date_from, format, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string? dateT = DateTime.ParseExact(req.date_to, format, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            var pdfBytes = SummaryServiceReportPdf.GenerateAsync(dateF, dateT, req.invoices);
            return File(pdfBytes, "application/pdf", $"InvoiceSummary.pdf");
        }
        #endregion
    }
}
