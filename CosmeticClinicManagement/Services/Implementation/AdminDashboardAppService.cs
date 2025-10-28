using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Services.Dtos.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class AdminDashboardAppService : ApplicationService
    {
        private readonly IRepository<Patient, Guid> _patientRepository;
        private readonly IRepository<TreatmentPlan, Guid> _treatmentPlanRepository;
        private readonly IdentityUserManager _userManager;

        public AdminDashboardAppService(
            IRepository<Patient, Guid> patientRepository,
            IRepository<TreatmentPlan, Guid> treatmentPlanRepository,
            IdentityUserManager userManager)
        {
            _patientRepository = patientRepository;
            _treatmentPlanRepository = treatmentPlanRepository;
            _userManager = userManager;
        }

        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            try
            {
                var totalPatients = await _patientRepository.GetCountAsync();

                // Count users in the "Doctor" role
                var allUsers = await _userManager.Users.ToListAsync();
                var totalDoctors = 0;
                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Doctor"))
                        totalDoctors++;
                }

                var activePlans = await _treatmentPlanRepository.CountAsync(tp => tp.Status == TreatmentPlanStatus.Ongoing);
                var closedPlans = await _treatmentPlanRepository.CountAsync(tp => tp.Status == TreatmentPlanStatus.Closed);

                // Example: revenue estimation (adjust as needed)
                decimal totalRevenue = (activePlans * 400) + (closedPlans * 750);

                return new AdminDashboardDto
                {
                    TotalPatients = totalPatients,
                    TotalDoctors = totalDoctors,
                    ActiveTreatmentPlans = activePlans,
                    ClosedTreatmentPlans = closedPlans,
                    TotalRevenue = totalRevenue
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while fetching admin dashboard data.");
                return new AdminDashboardDto
                {
                    TotalPatients = 0,
                    TotalDoctors = 0,
                    ActiveTreatmentPlans = 0,
                    ClosedTreatmentPlans = 0,
                    TotalRevenue = 0
                };

       }    }
    }
}
