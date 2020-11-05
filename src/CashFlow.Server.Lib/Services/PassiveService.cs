using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;

namespace CashFlow.Server.Lib.Services
{
    public class PassiveService : IPassiveService
    {
        private readonly IPassiveProvider _provider;
        public PassiveService(IPassiveProvider provider)
        {
            _provider = provider;
        }

        public Task<IReadOnlyCollection<PassiveDto>> GetAllPassiveForUserAsync(string userId)
        {
            return _provider.GetAllPassiveForUserAsync(userId);
        }

        public Task<PassiveDto> UpdatePassiveAsync(PassiveDto passive)
        {
            return _provider.UpdatePassiveAsync(passive);
        }

        public Task<PassiveDto> CreatePassiveForUserAsync(PassiveDto passive, string userId)
        {
            return _provider.CreatePassiveForUserAsync(passive, userId);
        }

        public Task RemovePassiveAsync(string id)
        {
            return _provider.RemovePassiveAsync(id);
        }
    }
}