using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace CosmeticClinicManagement.Data
{
    public class DoctorDashboardDataSeeder(IdentityUserManager userManager,
        IPatientRepository patientRepository,
        ITreatmentPlanRepository treatmentPlanRepository,
        IGuidGenerator guidGenerator) : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityUserManager _userManager = userManager;
        private readonly IPatientRepository _patientRepository = patientRepository;
        private readonly ITreatmentPlanRepository _treatmentPlanRepository = treatmentPlanRepository;
        private readonly IGuidGenerator _guidGenerator = guidGenerator;
        public async Task SeedAsync(DataSeedContext context)
        {
            var doctorUser = await SeedDoctorIfNotExist();
            // Seeding patients
            if (await _patientRepository.GetCountAsync() > 0)
            {
                return; // Already seeded
            }
            var patient1 = await _patientRepository.InsertAsync(new Patient(
                _guidGenerator.Create(),
                "John",
                "Doe",
                new DateTime(1990, 1, 1),
                "j@d.com",
                "1234567890"
            ));

            var patient2 = await _patientRepository.InsertAsync(new Patient(
                _guidGenerator.Create(),
                "Hossam",
                "Hassan",
                new DateTime(1990, 1, 1),
                "h@h.com",
                "1234568890"
            ));

            await _patientRepository.InsertManyAsync([patient1, patient2]);

            // Seeding treatment plans
            var treatmentPlan1 = new TreatmentPlan(
                _guidGenerator.Create(),
                doctorUser.Id,
                patient1.Id
            );

            var treatmentPlan2 = new TreatmentPlan(
                _guidGenerator.Create(),
                doctorUser.Id,
                patient2.Id
            );

            // Adding sessions to treatment plans
            treatmentPlan1.AddSession(new Session(
                _guidGenerator.Create(),
                treatmentPlan1.Id,
                DateTime.Now.AddHours(2),
                ["Initial consultation"],
                SessionStatus.Planned
            ));

            treatmentPlan1.AddSession(new Session(
                _guidGenerator.Create(),
                treatmentPlan1.Id,
                DateTime.Now.AddDays(3),
                ["consultation"],
                SessionStatus.Planned
            ));

            treatmentPlan1.AddSession(new Session(
                _guidGenerator.Create(),
                treatmentPlan1.Id,
                DateTime.Now.AddDays(5),
                ["consultation"],
                SessionStatus.Planned
            ));

            treatmentPlan2.AddSession(new Session(
                _guidGenerator.Create(),
                treatmentPlan1.Id,
                DateTime.Now.AddHours(5),
                ["Initial consultation"],
                SessionStatus.Planned
            ));

            treatmentPlan2.AddSession(new Session(
                _guidGenerator.Create(),
                treatmentPlan1.Id,
                DateTime.Now.AddDays(3),
                ["consultation"],
                SessionStatus.Planned
            ));

            treatmentPlan2.AddSession(new Session(
                _guidGenerator.Create(),
                treatmentPlan1.Id,
                DateTime.Now.AddDays(5),
                ["consultation"],
                SessionStatus.Planned
            ));

            await _treatmentPlanRepository.InsertManyAsync([treatmentPlan1, treatmentPlan2]);
        }

        private async Task<IdentityUser> SeedDoctorIfNotExist()
        {
            var doctorUser = await _userManager.FindByNameAsync("doctor");
            if (doctorUser == null)
            {
                doctorUser = new IdentityUser(
                    _guidGenerator.Create(),
                    "doctor",
                    "d@s.com"
                );

                doctorUser.SetProperty("Name", "Osama");
                doctorUser.SetProperty("SurName", "Ibrahim");

                await _userManager.CreateAsync(doctorUser, "Doctor@123");
                await _userManager.AddToRoleAsync(doctorUser, "Doctor");
            }

            
            return doctorUser;
        }
    }
}
