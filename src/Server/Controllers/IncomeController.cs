using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClient.Model.Entities;
using WebClient.Services;

namespace CashFlowManagement.Server.Controllers
{
    [Route("api/income")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        
        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        [Route("getallincomeforuser")]
        public async Task<IActionResult> GetAllForUser()
        {
            //TODO: Вынести получение id user
            return Ok(await _incomeService.GetAllIncomeForUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpPost]
        [Route("updateincome")]
        public async Task<IActionResult> UpdateIncome([FromBody]Income income)
        {
            return Ok(await _incomeService.UpdateIncomeAsync(income));
        }
        
        [HttpPut]
        [Route("createincome")]
        public async Task<IActionResult> CreateIncome([FromBody]Income income)
        {
            return Ok(await _incomeService.CreateIncomeAsync(income));
        }

        [HttpDelete]
        [Route("removeincome")]
        public async Task RemoveIncome([FromQuery] string id)
        {
            await _incomeService.RemoveIncomeAsync(id);
        }
    }
}
