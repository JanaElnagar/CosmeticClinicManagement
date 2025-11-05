using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface ISessionAppService : IApplicationService
    {
        Task<PagedResultDto<SessionDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task CreateUsedMaterialAsync(Guid SessionId, CreateUpdateUsedMaterialDto input);

        Task<List<SessionDto>> GetListByPlanAsync(Guid PlanId);
        Task CreateAsync(CreateSessionDto input);
        Task<SessionDto> GetAsync(Guid id);
        Task UpdateAsync(Guid id, UpdateSessionDto input);
        Task DeleteAsync(Guid id);

   //     Task<PagedResultDto<UsedMaterialDto>> GetUsedMaterialAsync(Guid SessionId, PagedAndSortedResultRequestDto input);

    }
}
