using CosmeticClinicManagement.Data;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CosmeticClinicManagement.Repositories
{
    public class PatientRepository : EfCoreRepository<CosmeticClinicManagementDbContext, Patient, Guid>, IPatientRepository
    {
        public PatientRepository(IDbContextProvider<CosmeticClinicManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Patient>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting)
        {
            var query = await GetQueryableAsync();
            return await query
                .OrderBy(sorting ?? nameof(Patient.FirstName))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
