using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Server.Extensions;
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
        [Route("getallforuser")]
        public async Task<IActionResult> GetAllIncomeForUser() => 
            Ok(await _incomeService.GetAllIncomeForUserAsync(User.GetIdentifier()));

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateIncome([FromBody] IncomeDto income) => 
            Ok(await _incomeService.UpdateIncomeAsync(income));

        [HttpPost]
        [Route("createforuser")]
        public async Task<IActionResult> CreateIncomeForUser([FromBody] IncomeDto income) => 
            Ok(await _incomeService.CreateIncomeForUserAsync(income, User.GetIdentifier()));

        [HttpDelete]
        [Route("remove")]
        public Task RemoveIncome([FromQuery] string id) => 
            _incomeService.RemoveIncomeAsync(id);
    }
}