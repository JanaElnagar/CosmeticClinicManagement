using CosmeticClinicManagement.Services.Dtos;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface ISessionAppService : IApplicationService
    {
        Task<List<SessionDto>> GetAsync(Guid planId);
    }
}
