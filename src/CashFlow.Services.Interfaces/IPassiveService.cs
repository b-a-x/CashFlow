using System.Collections.Generic;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;

namespace CashFlow.Services.Interfaces
{
    public interface IPassiveService
    {
        Task<IReadOnlyCollection<PassiveDto>> GetAllPassiveForUserAsync(string userId);
        Task<PassiveDto> UpdatePassiveAsync(PassiveDto passive);
        Task<PassiveDto> CreatePassiveForUserAsync(PassiveDto passive, string userId);
        Task RemovePassiveAsync(string id);
    }
}
