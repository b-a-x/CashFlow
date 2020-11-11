using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Server.Extensions;
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
        public async Task<IActionResult> GetAllPassiveForUser() => 
            Ok(await _passiveService.GetAllPassiveForUserAsync(User.GetIdentifier()));

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdatePassive([FromBody] PassiveDto passive) => 
            Ok(await _passiveService.UpdatePassiveAsync(passive));

        [HttpPost]
        [Route("createforuser")]
        public async Task<IActionResult> CreatePassiveForUser([FromBody] PassiveDto passive) => 
            Ok(await _passiveService.CreatePassiveForUserAsync(passive, User.GetIdentifier()));

        [HttpDelete]
        [Route("remove")]
        public Task RemovePassive([FromQuery] string id) => 
            _passiveService.RemovePassiveAsync(id);
    }
}