using CosmeticClinicManagement.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface ITreatmentPlanAppService : IApplicationService
    {
        Task<PagedResultDto<TreatmentPlanDto>>
 GetListAsync(PagedAndSortedResultRequestDto
input);

        Task CreateAsync(CreateUpdateTreatmentPlanDto input);
    }
}
