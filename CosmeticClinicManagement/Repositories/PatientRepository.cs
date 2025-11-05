using CosmeticClinicManagement.Data;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CosmeticClinicManagement.Repositories
{
    public class PatientRepository(IDbContextProvider<CosmeticClinicManagementDbContext> dbContextProvider/*,
        IServiceScopeFactory serviceScopeFactory*/) : EfCoreRepository<CosmeticClinicManagementDbContext, Patient, Guid>(dbContextProvider), IPatientRepository
    {
        //private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        public async Task<List<Patient>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting)
        {
            var query = await GetQueryableAsync();
            return await query
                .OrderBy(sorting ?? nameof(Patient.FirstName))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<Dictionary<Guid, (string FullName, DateTime DateOfBirth)>> GetPatientNamesAndDateOfBirthAsync(List<Guid> ids)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(p => ids.Contains(p.Id))
                .ToDictionaryAsync(
                    p => p.Id,
                    p => ($"{p.FirstName} {p.LastName}", p.DateOfBirth)
                );
        }

        //public async Task<Dictionary<Guid, (string, DateTime)>> GetPatientNamesAndDateOfBirthAsync(List<Guid> ids)
        //{
        //    using var scope = _serviceScopeFactory.CreateScope();
        //    var context = scope.ServiceProvider.GetRequiredService<CosmeticClinicManagementDbContext>();
        //    return await context.Patients
        //    .Where(p => ids.Contains(p.Id)).ToDictionaryAsync(p => p.Id, p => ($"{p.FirstName} {p.LastName}", p.DateOfBirth));

        //}
    }
}
