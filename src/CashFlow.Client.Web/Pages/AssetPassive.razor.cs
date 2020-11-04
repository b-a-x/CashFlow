using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlow.Client.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace CashFlow.Client.Web.Pages
{
    public partial class AssetPassive
    {
        [Inject]
        private IFormatProvider provider { get; set; }

        [Inject]
        public HttpInterceptorService interceptor { get; set; }

        protected override Task OnInitializedAsync()
        {
            interceptor.RegisterEvent();
            return base.OnInitializedAsync();
        }

        public void Dispose() => interceptor.DisposeEvent();

        public class Asset
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public float Price { get; set; }
            public int OrderNumber { get; set; }
        }

        private RadzenGrid<Asset> assetGrid;
        private List<Asset> assets = new List<Asset>
        {
            new Asset{Id = Guid.NewGuid().ToString(), Name = "Акции компании Apple", Quantity = 10, Price = 100000, OrderNumber = 1}
        };

        private void InsertRowAsset()
        {
            var asset = new Asset();
            var id = (assets.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1;
            asset.OrderNumber = id;
            assetGrid.InsertRow(asset);
        }

        private void OnUpdateRowAsset(Asset asset)
        {
            //TODO Научиться правильно пределять объект
            foreach (Asset item in assets)
            {
                if (item.Id == asset.Id)
                {
                    item.Name = asset.Name;
                    item.Quantity = asset.Quantity;
                    item.Price = asset.Price;
                }
            }
        }

        private void OnCreateRowAsset(Asset asset)
        {
            assets.Add(asset);
        }

        private void EditRowAsset(Asset asset)
        {
            assetGrid.EditRow(asset);
        }

        private void SaveRowAsset(Asset asset)
        {
            assetGrid.UpdateRow(asset);
        }

        private void CancelEditAsset(Asset asset)
        {
            assetGrid.CancelEditRow(asset);
        }

        private void DeleteRowAsset(Asset asset)
        {
            //TODO Научиться правильно пределять объект
            if (assets.Contains(asset))
            {
                assets.Remove(asset);
                assetGrid.Reload();
            }
            else
            {
                assetGrid.CancelEditRow(asset);
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
