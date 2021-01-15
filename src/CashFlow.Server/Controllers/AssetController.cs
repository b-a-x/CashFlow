using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Server.Extensions;
using CashFlow.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Server.Controllers
{
    [Route("api/asset")]
    [ApiController]
    [Authorize]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService expenseService)
        {
            _assetService = expenseService;
        }

        [HttpGet]
        [Route("getallforuser")]
        public async Task<IActionResult> GetAllAssetForUser() => 
            Ok(await _assetService.GetAllAssetForUserAsync(User.GetIdentifier()));

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAsset([FromBody] AssetDto asset) => 
            Ok(await _assetService.UpdateAssetAsync(asset));

        [HttpPost]
        [Route("createforuser")]
        public async Task<IActionResult> CreateAssetForUser([FromBody] AssetDto asset) => 
            Ok(await _assetService.CreateAssetForUserAsync(asset, User.GetIdentifier()));

        [HttpDelete]
        [Route("remove")]
        public Task RemoveAsset([FromQuery] string id) => 
            _assetService.RemoveAssetAsync(id);
    }
}