using System.Security.Claims;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Server.Controllers
{
    [Route("api/income")]
    [ApiController]
    [Authorize]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        [Route("getallincomeforuser")]
        public async Task<IActionResult> GetAllIncomeForUser()
        {
            //TODO: Вынести получение id user
            return Ok(await _incomeService.GetAllIncomeForUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpPut]
        [Route("updateincome")]
        public async Task<IActionResult> UpdateIncome([FromBody] IncomeDto income)
        {
            return Ok(await _incomeService.UpdateIncomeAsync(income));
        }

        [HttpPost]
        [Route("createincomeforuser")]
        public async Task<IActionResult> CreateIncomeForUser([FromBody] IncomeDto income)
        {
            return Ok(await _incomeService.CreateIncomeForUserAsync(income, User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpDelete]
        [Route("removeincome")]
        public async Task RemoveIncome([FromQuery] string id)
        {
            await _incomeService.RemoveIncomeAsync(id);
        }
    }
}
