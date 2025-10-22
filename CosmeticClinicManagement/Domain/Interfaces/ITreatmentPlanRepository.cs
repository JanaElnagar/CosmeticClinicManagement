using CosmeticClinicManagement.Domain.ClinicManagement;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Domain.Interfaces
{
    public interface ITreatmentPlanRepository : IRepository<TreatmentPlan, Guid>
    {
    }
}
