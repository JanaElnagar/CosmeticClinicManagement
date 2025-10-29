using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using CosmeticClinicManagement.Services.Dtos.Dashboard;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class ReceptionistDashboardAppService : ApplicationService
    {
        private readonly IRepository<TreatmentPlan, Guid> _treatmentPlanRepository;
        private readonly IRepository<Patient, Guid> _patientRepository;

        public ReceptionistDashboardAppService(
            IRepository<TreatmentPlan, Guid> treatmentPlanRepository,
            IRepository<Patient, Guid> patientRepository)
        {
            _treatmentPlanRepository = treatmentPlanRepository;
            _patientRepository = patientRepository;
        }

        public async Task<ReceptionistDashboardDto> GetDashboardDataAsync()
        {
            var treatmentPlans = await _treatmentPlanRepository.GetListAsync(includeDetails:true);
            var patients = await _patientRepository.GetListAsync();


            //var allSessions = treatmentPlans
            //    .SelectMany(tp => tp.Sessions)
            //    .ToList();

            var allSessions = treatmentPlans
                        .Where(tp => tp.Sessions != null)
                        .SelectMany(tp => tp.Sessions)
                        .ToList();

            var upcomingSessions = allSessions
                .Where(s => s.SessionDate > DateTime.Now && s.Status == SessionStatus.Planned)
                .OrderBy(s => s.SessionDate)
                .Take(5)
                .ToList();

            var completedCount = allSessions.Count(s => s.Status == SessionStatus.Completed);
            var cancelledCount = allSessions.Count(s => s.Status == SessionStatus.Cancelled);

            return new ReceptionistDashboardDto
            {
                TotalPatients = patients.Count,
                TotalTreatmentPlans = treatmentPlans.Count,
                UpcomingSessions = upcomingSessions.Count,
                CompletedSessions = completedCount,
                CancelledSessions = cancelledCount
            };
        }
    }

}
