using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Server.Extensions;
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
        [Route("getallforuser")]
        public async Task<IActionResult> GetAllExpenseForUser() => 
            Ok(await _expenseService.GetAllExpenseForUserAsync(User.GetIdentifier()));

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateExpense([FromBody] ExpenseDto expense) => 
            Ok(await _expenseService.UpdateExpenseAsync(expense));

        [HttpPost]
        [Route("createforuser")]
        public async Task<IActionResult> CreateExpenseForUser([FromBody] ExpenseDto expense) => 
            Ok(await _expenseService.CreateExpenseForUserAsync(expense, User.GetIdentifier()));

        [HttpDelete]
        [Route("remove")]
        public Task RemoveExpense([FromQuery] string id) =>
            _expenseService.RemoveExpenseAsync(id);
    }
}