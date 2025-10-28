using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.InventoryManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Services.Dtos.Dashboard;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace CosmeticClinicManagement.Services.Dashboard
{
    public class AdminDashboardAppService : DashboardAppService
    {
        private readonly IRepository<Patient, Guid> _patients;
        private readonly IRepository<Session, Guid> _sessions;
        private readonly IRepository<RawMaterial, Guid> _materials;
        private readonly IRepository<Store, Guid> _stores;

        public AdminDashboardAppService(
            IRepository<Patient, Guid> patients,
            IRepository<Session, Guid> sessions,
            IRepository<RawMaterial, Guid> materials,
            IRepository<Store, Guid> stores,
            ICurrentUser currentUser
        ) : base(currentUser)
        {
            _patients = patients;
            _sessions = sessions;
            _materials = materials;
            _stores = stores;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            return new DashboardStatsDto
            {
                TotalPatients = await _patients.GetCountAsync(),
                TotalSessions = await _sessions.GetCountAsync(),
                LowStockMaterials = await _materials.CountAsync(x => x.Quantity < 5),
                TotalStaff = 10 // optional: query Identity users instead
            };
        }

        public async Task<DashboardChartDto> GetSessionsTrendAsync()
        {
            var sessions = await _sessions.GetListAsync();
            var grouped = sessions
                .GroupBy(s => s.CreationTime.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToList();

            return new DashboardChartDto
            {
                Labels = grouped.Select(x => x.Date.ToString("MMM dd")).ToList(),
                Values = grouped.Select(x => x.Count).ToList()
            };
        }
    }
}
