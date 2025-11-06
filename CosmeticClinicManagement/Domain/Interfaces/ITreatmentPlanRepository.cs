using CosmeticClinicManagement.Domain.ClinicManagement;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Domain.Interfaces
{
    public interface ITreatmentPlanRepository : IRepository<TreatmentPlan, Guid>
    {
        Task<List<TreatmentPlan>> GetTreatmentPlansByDoctorIdAsync(Guid doctorId);
        Task<List<TreatmentPlan>> GetListWithDetailsAsync();
        Task<List<TreatmentPlan>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting);
    }
}
