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
        
        [Inject]
        public IPassiveService _passiveService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _interceptor.RegisterEvent();
            try
            {
                //TODO: Обрабатывать Exception авторизации 
                _assets = (List<AssetDto>)await _assetService.GetAllAssetForUserAsync(string.Empty);
                _passives = (List<PassiveDto>)await _passiveService.GetAllPassiveForUserAsync(string.Empty);
            }
            catch (Exception _)
            {
                NavigationManager.NavigateTo("/login");
            }
            
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

                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _assetService.UpdateAssetAsync(edit);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
        }

        private async Task OnCreateRowAsset(AssetDto asset)
        {
            try
            {
                //TODO: Обрабатывать Exception авторизации 
                asset = await _assetService.CreateAssetForUserAsync(asset, string.Empty);
            }
            catch (Exception _)
            {
                NavigationManager.NavigateTo("/login");
            }
            
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

                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _assetService.RemoveAssetAsync(asset.Id);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
            else
            {
                _assetGrid.CancelEditRow(asset);
            }
        }

        private RadzenGrid<PassiveDto> _passiveGrid;
        private List<PassiveDto> _passives = new List<PassiveDto>();

        private void InsertRowPassive()
        {
            _passiveGrid.InsertRow(new PassiveDto { OrderNumber = (_passives.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1 });
        }

        private async Task OnUpdateRowPassive(PassiveDto passive)
        {
            PassiveDto edit = _passives.FirstOrDefault(x => x.Id == passive.Id);
            if (edit != null)
            {
                edit.Name = passive.Name;
                edit.Price = passive.Price;
                edit.OrderNumber = passive.OrderNumber;

                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _passiveService.UpdatePassiveAsync(edit);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
        }

        private async Task OnCreateRowPassive(PassiveDto passive)
        {
            try
            {
                //TODO: Обрабатывать Exception авторизации 
                passive = await _passiveService.CreatePassiveForUserAsync(passive, string.Empty);
            }
            catch (Exception _)
            {
                NavigationManager.NavigateTo("/login");
            }
            
            _passives.Add(passive);
            await _passiveGrid.Reload();
        }

        private void EditRowPassive(PassiveDto passive)
        {
            _passiveGrid.EditRow(passive);
        }

        private void SaveRowPassive(PassiveDto passive)
        {
            _passiveGrid.UpdateRow(passive);
        }

        private void CancelEditPassive(PassiveDto passive)
        {
            _passiveGrid.CancelEditRow(passive);
        }

        private async Task DeleteRowPassive(PassiveDto passive)
        {
            if (_passives.Contains(passive))
            {
                _passives.Remove(passive);
                await _passiveGrid.Reload();
                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _passiveService.RemovePassiveAsync(passive.Id);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
                
            }
            else
            {
                _passiveGrid.CancelEditRow(passive);
            }
        }
    }
}