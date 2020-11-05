using System.Security.Claims;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Server.Controllers
{
    [Route("api/passive")]
    [ApiController]
    [Authorize]
    public class PassiveController : ControllerBase
    {
        private readonly IPassiveService _passiveService;

        public PassiveController(IPassiveService passiveService)
        {
            _passiveService = passiveService;
        }

        [HttpGet]
        [Route("getallforuser")]
        public async Task<IActionResult> GetAllPassiveForUser()
        {
            //TODO: Вынести получение id user
            return Ok(await _passiveService.GetAllPassiveForUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdatePassive([FromBody] PassiveDto passive)
        {
            return Ok(await _passiveService.UpdatePassiveAsync(passive));
        }

        [HttpPost]
        [Route("createforuser")]
        public async Task<IActionResult> CreatePassiveForUser([FromBody] PassiveDto passive)
        {
            return Ok(await _passiveService.CreatePassiveForUserAsync(passive, User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpDelete]
        [Route("remove")]
        public async Task RemovePassive([FromQuery] string id)
        {
            await _passiveService.RemovePassiveAsync(id);
        }
    }
}
