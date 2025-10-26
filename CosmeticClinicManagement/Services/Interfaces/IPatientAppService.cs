using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface IPatientAppService : IApplicationService
    {
        Task<PagedResultDto<PatientDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<PatientDto> GetAsync(Guid id);
        Task<PatientDto> CreateAsync(PatientDto input);
        Task UpdateAsync(Guid id, PatientDto input);
        Task DeleteAsync(Guid id);

        Task<List<PatientDto>> GetAllPatientsAsync();
    }
}
