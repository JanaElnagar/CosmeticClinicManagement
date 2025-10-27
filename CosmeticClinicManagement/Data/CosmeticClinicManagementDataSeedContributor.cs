using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmeticClinicManagement.Constants;
using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace CosmeticClinicManagement.Data
{
   // [DependsOn(typeof(DefaultRolesDataSeeder))]
    public class CosmeticClinicManagementDataSeedContributor :
 IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<TreatmentPlan, Guid> _treatmentRepository;
       // private readonly IdentityUserManager _userManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Patient, Guid> _patientRepository;
        private readonly IClock _clock;

        public CosmeticClinicManagementDataSeedContributor(IRepository<TreatmentPlan, Guid> treatmentRepository
            , IdentityUserManager userManager, IGuidGenerator guidGenerator, IRepository<Patient, Guid> patientRepository, IClock clock)
        {
            _treatmentRepository = treatmentRepository;
        //    _userManager = userManager;
            _guidGenerator = guidGenerator;
            _patientRepository = patientRepository;
            _clock = clock;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            //if (await _patientRepository.GetCountAsync() > 0)
            //{
            //    return; // Already seeded
            //}

            var patient1 = new Patient(
                    Guid.NewGuid(),
                    "John",
                    "Doe",
                    new DateTime(1990, 5, 10),
                    "john.doe@example.com",
                    "+201001234567"
                );
            var patient2 = new Patient(
                    Guid.NewGuid(),
                    "Jane",
                    "Smith",
                    new DateTime(1985, 12, 20),
                    "jane.smith@example.com",
                    "+201002345678"
                );
            var patient3 = new Patient(
                    Guid.NewGuid(),
                    "Ali",
                    "Ahmed",
                    new DateTime(1995, 7, 15),
                    "ali.ahmed@example.com",
                    "+201003456789"
                );
            //await _patientRepository
            // .InsertManyAsync(new[] { patient1, patient2, patient3 });





            if (await _treatmentRepository.CountAsync() > 0) { 
                return;
        }

            var tp1Id = Guid.NewGuid();
            var treatmentPlan1 = new TreatmentPlan
            (
                tp1Id,
                Guid.NewGuid(),
                patient1.Id);
            var session1 = new Session(
             Guid.NewGuid(),
             tp1Id,
             _clock.Now.AddDays(5),
              new List<string> { "Apply sunscreen after session", "Extra hydration requested" },

              SessionStatus.InProgress);
            var used = new UsedMaterial(_guidGenerator.Create(), 10m);
            session1.AddUsedMaterial(used);
            session1.AddUsedMaterial(new UsedMaterial(Guid.NewGuid(), 5.5m));

            var session2 = new Session(
    Guid.NewGuid(),
    tp1Id,
    _clock.Now.AddDays(7),
    new List<string> { "Check skin reaction", "Remind patient to hydrate" },
    SessionStatus.InProgress
);
            session2.AddUsedMaterial(new UsedMaterial(Guid.NewGuid(), 8m));
            session2.AddUsedMaterial(new UsedMaterial(Guid.NewGuid(), 12m));

            treatmentPlan1.AddSession(session1);
            treatmentPlan1.AddSession(session2);

            // Second TreatmentPlan
            var tp2Id = Guid.NewGuid();
            var treatmentPlan2 = new TreatmentPlan(tp2Id, Guid.NewGuid(), patient2.Id);

            var s2_1 = new Session(
                Guid.NewGuid(),
                tp2Id,
                _clock.Now.AddDays(7),
                new List<string> { "Use gentle cleanser", "Patient has sensitive skin" },
                SessionStatus.InProgress
            );
            s2_1.AddUsedMaterial(new UsedMaterial(Guid.NewGuid(), 15m));
            s2_1.AddUsedMaterial(new UsedMaterial(Guid.NewGuid(), 7m));

            var s2_2 = new Session(
                Guid.NewGuid(),
                           tp2Id,
                _clock.Now.AddDays(10),
                new List<string> { "Check progress", "Apply serum" },
                SessionStatus.InProgress
            );
            s2_2.AddUsedMaterial(new UsedMaterial(Guid.NewGuid(), 9m));
            s2_2.AddUsedMaterial(new UsedMaterial(Guid.NewGuid(), 6.5m));

            treatmentPlan2.AddSession(s2_1);
            treatmentPlan2.AddSession(s2_2);

            // Insert both TreatmentPlans
            await _treatmentRepository.InsertAsync(treatmentPlan1, autoSave: true);
            await _treatmentRepository.InsertAsync(treatmentPlan2, autoSave: true);
            if (await _patientRepository.GetCountAsync() > 0)
            {
                return; // Already seeded
            }
            await _patientRepository
             .InsertManyAsync(new[] { patient1, patient2, patient3 });
        }

    }
            
        
        }

 