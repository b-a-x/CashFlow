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
            //TODO: бизенс логику убрать в menager, сделать в одной транзакции
            //TODO: Тест выборка
            var orderNumber = _context.Incomes
                .Where(x => x.UserId == userId || x.UserId == null || x.UserId == string.Empty)
                .OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber + 1 ?? 1;
            var newIncome = new Income { Name = asset.Name, OrderNumber = orderNumber, Price = 0 };
            await _context.Incomes.AddAsync(newIncome);

            var newAsset = new Asset
            {
                Name = asset.Name,
                OrderNumber = asset.OrderNumber,
                Price = asset.Price,
                Quantity = asset.Quantity,
                UserId = userId,
                IncomeId = newIncome.Id
            };
            await _context.Assets.AddAsync(newAsset);
            await _context.SaveChangesAsync();

            asset.Id = newAsset.Id;
            asset.Name = newAsset.Name;
            asset.OrderNumber = newAsset.OrderNumber;
            asset.Price = newAsset.Price;
            asset.Quantity = newAsset.Quantity;

            return asset;
        }

        public async Task RemoveAssetAsync(string id)
        {
            //TODO: бизенс логику убрать в menager, сделать в одной транзакции
            Asset original = await _context.Assets.FindAsync(id);
            Income income = await _context.Incomes.FindAsync(original.IncomeId);
            _context.Incomes.Remove(income);
            _context.Assets.Remove(original);
            await _context.SaveChangesAsync();
        }
    }
}
