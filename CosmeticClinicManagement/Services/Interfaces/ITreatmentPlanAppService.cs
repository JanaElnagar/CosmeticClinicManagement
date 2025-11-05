using CosmeticClinicManagement.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface ITreatmentPlanAppService : IApplicationService
    {
        Task<PagedResultDto<TreatmentPlanDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<PagedResultDto<SessionDto>> GetSessionsAsync(Guid PlanId, PagedAndSortedResultRequestDto input);
        Task CreateAsync(CreateUpdateTreatmentPlanDto input);
        Task<TreatmentPlanDto> GetAsync(Guid id);
        Task UpdateAsync(Guid id, CreateUpdateTreatmentPlanDto input);
        Task DeleteAsync(Guid id);
        Task CreateSessionAsync (Guid PlanId,CreateSessionDto input);
        Task<ListResultDto<PatientDto>> GetPatientsAsync();
        Task<ListResultDto<DoctorDto>> GetDoctorsAsync();

    }
}
