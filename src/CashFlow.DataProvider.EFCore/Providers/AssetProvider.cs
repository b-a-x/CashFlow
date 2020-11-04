using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Model;
using CashFlow.Model.Dto.Request;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.DataProvider.EFCore.Providers
{
    //TODO: AutoMapper
    public class AssetProvider : IAssetProvider
    {
        private readonly DataContext _context;
        public AssetProvider(DataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<AssetDto>> GetAllAssetForUserAsync(string userId)
        {
            //TODO: Тест выборка
            return await _context.Assets.Where(x => x.UserId == userId || x.UserId == null || x.UserId == string.Empty)
                .OrderBy(x => x.OrderNumber).AsNoTracking()
                .Select(x => new AssetDto { Id = x.Id, Name = x.Name, OrderNumber = x.OrderNumber, Price = x.Price, Quantity = x.Quantity })
                .ToArrayAsync();
        }

        public async Task<AssetDto> UpdateAssetAsync(AssetDto asset)
        {
            Asset original = await _context.Assets.FindAsync(asset.Id);
            original.Name = asset.Name;
            original.OrderNumber = asset.OrderNumber;
            original.Price = asset.Price;
            original.Quantity = asset.Quantity;

            await _context.SaveChangesAsync();

            asset.Id = original.Id;
            asset.Name = original.Name;
            asset.OrderNumber = original.OrderNumber;
            asset.Price = original.Price;
            return asset;
        }

        public async Task<AssetDto> CreateAssetForUserAsync(AssetDto asset, string userId)
        {
            var newIncome = new Asset
            {
                Name = asset.Name,
                OrderNumber = asset.OrderNumber,
                Price = asset.Price,
                Quantity = asset.Quantity,
                UserId = userId
            };
            await _context.Assets.AddAsync(newIncome);
            await _context.SaveChangesAsync();

            asset.Id = newIncome.Id;
            asset.Name = newIncome.Name;
            asset.OrderNumber = newIncome.OrderNumber;
            asset.Price = newIncome.Price;
            asset.Quantity = newIncome.Quantity;

            return asset;
        }

        public async Task RemoveAssetAsync(string id)
        {
            Asset original = await _context.Assets.FindAsync(id);
            _context.Assets.Remove(original);
            await _context.SaveChangesAsync();
        }
    }
}
