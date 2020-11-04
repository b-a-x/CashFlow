using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;

namespace CashFlow.Services.Interfaces
{
    public interface IAssetService
    {
        Task<IReadOnlyCollection<AssetDto>> GetAllAssetForUserAsync(string userId);
        Task<AssetDto> UpdateAssetAsync(AssetDto asset);
        Task<AssetDto> CreateAssetForUserAsync(AssetDto asset, string userId);
        Task RemoveAssetAsync(string id);
    }
}
