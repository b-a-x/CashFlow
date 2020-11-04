using System.Security.Claims;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Server.Controllers
{
    [Route("api/expense")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        [Route("getallexpenseforuser")]
        public async Task<IActionResult> GetAllExpenseForUser()
        {
            //TODO: Вынести получение id user
            return Ok(await _expenseService.GetAllExpenseForUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpPut]
        [Route("updateexpense")]
        public async Task<IActionResult> UpdateExpense([FromBody] ExpenseDto expense)
        {
            return Ok(await _expenseService.UpdateExpenseAsync(expense));
        }

        [HttpPost]
        [Route("createexpenseforuser")]
        public async Task<IActionResult> CreateExpenseForUser([FromBody] ExpenseDto expense)
        {
            return Ok(await _expenseService.CreateExpenseForUserAsync(expense, User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpDelete]
        [Route("removeexpense")]
        public async Task RemoveExpense([FromQuery] string id)
        {
            await _expenseService.RemoveExpenseAsync(id);
        }
    }
}
