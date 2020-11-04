using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;

namespace CashFlow.Server.Lib.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetProvider _provider;
        public AssetService(IAssetProvider provider)
        {
            _provider = provider;
        }

        public Task<IReadOnlyCollection<AssetDto>> GetAllAssetForUserAsync(string userId)
        {
            return _provider.GetAllAssetForUserAsync(userId);
        }

        public Task<AssetDto> UpdateAssetAsync(AssetDto asset)
        {
            return _provider.UpdateAssetAsync(asset);
        }

        public Task<AssetDto> CreateAssetForUserAsync(AssetDto asset, string userId)
        {
            return _provider.CreateAssetForUserAsync(asset, userId);
        }

        public Task RemoveAssetAsync(string id)
        {
            return _provider.RemoveAssetAsync(id);
        }
    }
}
