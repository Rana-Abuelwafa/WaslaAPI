using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("GetProduct_Tree")]
        public async Task<IActionResult> GetProduct_Tree()
        {
            return Ok(await _waslaService.GetProduct_Tree("admin"));
        }

        [HttpPost("SaveProduct")]
        public IActionResult SaveProduct(Product product)
        {
            return Ok(_waslaService.SaveProduct(product));
        }
        #endregion "product"

    }
}
