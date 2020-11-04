using System.Security.Claims;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
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
        [Route("getallassetforuser")]
        public async Task<IActionResult> GetAllAssetForUser()
        {
            return Ok(await _assetService.GetAllAssetForUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpPut]
        [Route("updateasset")]
        public async Task<IActionResult> UpdateAsset([FromBody] AssetDto asset)
        {
            return Ok(await _assetService.UpdateAssetAsync(asset));
        }

        [HttpPost]
        [Route("createassetforuser")]
        public async Task<IActionResult> CreateAssetForUser([FromBody] AssetDto asset)
        {
            return Ok(await _assetService.CreateAssetForUserAsync(asset, User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpDelete]
        [Route("removeasset")]
        public async Task RemoveAsset([FromQuery] string id)
        {
            await _assetService.RemoveAssetAsync(id);
        }
    }
}
