using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlow.Client.Web.Services;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace CashFlow.Client.Web.Pages
{
    public partial class AssetPassive
    {
        [Inject]
        private IFormatProvider _provider { get; set; }

        [Inject]
        public HttpInterceptorService _interceptor { get; set; }

        [Inject]
        public IAssetService _assetService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _interceptor.RegisterEvent();
            _assets = (List<AssetDto>)await _assetService.GetAllAssetForUserAsync(string.Empty);
            //_expenses = (List<ExpenseDto>)await _expenseService.GetAllExpenseForUserAsync(string.Empty);
            await base.OnInitializedAsync();
        }

        public void Dispose() => _interceptor.DisposeEvent();

        private RadzenGrid<AssetDto> _assetGrid;
        private List<AssetDto> _assets = new List<AssetDto>();

        private void InsertRowAsset()
        {
            _assetGrid.InsertRow(new AssetDto { OrderNumber = (_assets.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1 });
        }

        private async Task OnUpdateRowAsset(AssetDto asset)
        {
            AssetDto edit = _assets.FirstOrDefault(x => x.Id == asset.Id);
            if (edit != null)
            {
                edit.Name = asset.Name;
                edit.Price = asset.Price;
                edit.Quantity = asset.Quantity;
                await _assetService.UpdateAssetAsync(edit);
            }
        }

        private async Task OnCreateRowAsset(AssetDto asset)
        {
            asset = await _assetService.CreateAssetForUserAsync(asset, string.Empty);
            _assets.Add(asset);
            await _assetGrid.Reload();
        }

        private void EditRowAsset(AssetDto asset)
        {
            _assetGrid.EditRow(asset);
        }

        private void SaveRowAsset(AssetDto asset)
        {
            _assetGrid.UpdateRow(asset);
        }

        private void CancelEditAsset(AssetDto asset)
        {
            _assetGrid.CancelEditRow(asset);
        }

        private async Task DeleteRowAsset(AssetDto asset)
        {
            if (_assets.Contains(asset))
            {
                _assets.Remove(asset);
                await _assetGrid.Reload();
                await _assetService.RemoveAssetAsync(asset.Id);
            }
            else
            {
                _assetGrid.CancelEditRow(asset);
            }
        }


        public class Passive
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public float Price { get; set; }
            public int OrderNumber { get; set; }
        }

        private RadzenGrid<Passive> passiveGrid;
        private List<Passive> passives = new List<Passive>
        {
            new Passive{Id = Guid.NewGuid().ToString(), Name = "Ипотека", Price = 5000000, OrderNumber = 1}
        };

        private void InsertRowPassive()
        {
            var passive = new Passive();
            var id = (passives.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1;
            passive.OrderNumber = id;
            passiveGrid.InsertRow(passive);
        }

        private void OnUpdateRowPassive(Passive passive)
        {
            //TODO Научиться правильно пределять объект
            foreach (Passive item in passives)
            {
                if (item.Id == passive.Id)
                {
                    item.Name = passive.Name;
                    item.Price = passive.Price;
                }
            }
        }

        private void OnCreateRowPassive(Passive passive)
        {
            passives.Add(passive);
        }

        private void EditRowPassive(Passive passive)
        {
            passiveGrid.EditRow(passive);
        }

        private void SaveRowPassive(Passive passive)
        {
            passiveGrid.UpdateRow(passive);
        }

        private void CancelEditPassive(Passive passive)
        {
            passiveGrid.CancelEditRow(passive);
        }

        private void DeleteRowPassive(Passive passive)
        {
            //TODO Научиться правильно пределять объект
            if (passives.Contains(passive))
            {
                passives.Remove(passive);
                passiveGrid.Reload();
            }
            else
            {
                passiveGrid.CancelEditRow(passive);
            }
        }
    }
}
