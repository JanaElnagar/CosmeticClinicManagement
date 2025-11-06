using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.InventoryManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Domain.Services;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;


namespace CosmeticClinicManagement.Services.Implementation
{
    [Authorize("TreatmentPlanManagement")]
    public class TreatmentPlanAppService : ApplicationService, ITreatmentPlanAppService
    {
        private readonly ITreatmentPlanRepository _treatmentPlanRepository;
        private readonly IRepository<Session, Guid> _sessionRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IRepository<RawMaterial, Guid> _rawMaterialRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger<TreatmentPlanAppService> _logger;

        public TreatmentPlanAppService(ITreatmentPlanRepository treatmentPlanRepository, IRepository<Session, Guid> sessionRepository
            , IPatientRepository patientRepository, IRepository<RawMaterial, Guid> rawMaterialRepository, IIdentityUserRepository userRepository, IUnitOfWorkManager unitOfWorkManager, ILogger<TreatmentPlanAppService> logger)
        {
            _treatmentPlanRepository = treatmentPlanRepository;
            _sessionRepository = sessionRepository;
            _patientRepository = patientRepository;
            _rawMaterialRepository = rawMaterialRepository;
            _logger = logger;
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /*
            public Guid Id { get; set; }
            public string DoctorName { get; set; }
            public string PatientFullName { get; set; }
            public int TotalSessions { get; set; }
            public TreatmentPlanStatus Status { get; set; }
            public DateTime CreatedDate { get; set; }
            public int PatientAge { get; set; }
         */
        public async Task<PagedResultDto<TreatmentPlanDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            _logger.LogInformation("Fetching treatment plans with SkipCount: {SkipCount}, MaxResultCount: {MaxResultCount}, Sorting: {Sorting}",
                input.SkipCount, input.MaxResultCount, input.Sorting);

            var treatmentPlans = await _treatmentPlanRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting ?? "CreationTime DESC"
            );
            int totalCount = await _treatmentPlanRepository.CountAsync();
            _logger.LogInformation("Fetched {Count} treatment plans from repository.", treatmentPlans.Count);

            var doctorsIds = treatmentPlans.Select(tp => tp.DoctorId).Distinct().ToList();
            var patientsIds = treatmentPlans.Select(tp => tp.PatientId).Distinct().ToList();

            _logger.LogInformation("Fetching details for {DoctorCount} doctors and {PatientCount} patients.",
                doctorsIds.Count, patientsIds.Count);
            var doctors = await _userRepository.GetListByIdsAsync(doctorsIds);
            _logger.LogInformation("Fetched {DoctorCount} doctors.", doctors.Count);

            _logger.LogInformation("Fetching patient names and dates of birth.");
            var patients = await _patientRepository.GetPatientNamesAndDateOfBirthAsync(patientsIds);
            _logger.LogInformation("Fetched details for {PatientCount} patients.", patients.Count);


            var treatmentPlanDtos = treatmentPlans.Select(tp =>
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == tp.DoctorId);
                var patient = patients[tp.PatientId];
                return new TreatmentPlanDto
                {
                    Id = tp.Id,
                    DoctorName = doctor != null ? $"{doctor.Name} {doctor.Surname}" : "Unknown Doctor",
                    PatientFullName = patient.FullName,
                    TotalSessions = tp.Sessions.Count,
                    Status = tp.Status,
                    CreatedDate = tp.CreationTime,
                    PatientAge = DateTime.Now.Year - patient.DateOfBirth.Year
                };
            }).ToList();


            return new PagedResultDto<TreatmentPlanDto>(
                totalCount,
                treatmentPlanDtos
            );
        }

        public async Task CreateAsync(CreateUpdateTreatmentPlanDto input)
        {
            var patientExists = await _patientRepository.AnyAsync(p => p.Id == input.PatientId);
            if (!patientExists)
            {
                throw new UserFriendlyException($"Patient with ID {input.PatientId} does not exist.");
            }

            await _treatmentPlanRepository.InsertAsync(
            ObjectMapper.Map<CreateUpdateTreatmentPlanDto, TreatmentPlan>(input));
        }

        public async Task<PagedResultDto<SessionDto>> GetSessionsAsync(Guid planId, PagedAndSortedResultRequestDto input)
        {
            var queryable = await _sessionRepository
     .WithDetailsAsync(x => x.UsedMaterials);
            queryable = queryable.Where(s => s.PlanId == planId)
                .OrderByDescending(tp => tp.CreationTime)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
            var sessions = await
           AsyncExecuter.ToListAsync(queryable);
            var count = await _sessionRepository.CountAsync(s => s.PlanId == planId);
            return new PagedResultDto<SessionDto>(
            count,
            ObjectMapper.Map<List<Session>, List<SessionDto>>
           (sessions)
            );
        }
        //public async Task<ListResultDto<SessionDto>>
        // GetSessionsAsync()
        //{
        //    var sessions = await _sessionRepository.GetListAsync();
        //    return new ListResultDto<Dtos.SessionDto>(
        //    ObjectMapper
        //    .Map<List<Session>, List<Dtos.SessionDto>>
        //   (sessions)
        //    );
        //}

        public async Task<TreatmentPlanDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<TreatmentPlan, TreatmentPlanDto>
           (await _treatmentPlanRepository
            .GetAsync(id));
        }

        public async Task UpdateAsync(Guid id, CreateUpdateTreatmentPlanDto input)
        {
            var treatmentPlan = await _treatmentPlanRepository.GetAsync(id);
            ObjectMapper.Map<CreateUpdateTreatmentPlanDto, TreatmentPlan>
           (input, treatmentPlan);
        }
        //[Authorize]
        public async Task CreateSessionAsync(Guid PlanId, [FromBody] CreateSessionDto input)
        {
            var treatmentPlan = await _treatmentPlanRepository.GetAsync(input.PlanId);
            treatmentPlan.AddSession(new Session(
                GuidGenerator.Create(),
                input.PlanId,
                input.SessionDate,
                new List<string> { "Use gentle cleanser", "Patient has sensitive skin" },
                input.Status));
            await _treatmentPlanRepository.UpdateAsync(treatmentPlan);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _treatmentPlanRepository.DeleteAsync(id);
        }
        public async Task AddUsedMaterialToSessionAsync(Guid planId, Guid sessionId, Guid rawMaterialId, decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            var rawMaterial = await _rawMaterialRepository.FindAsync(rawMaterialId)
                ?? throw new InvalidOperationException("Raw material not found.");

            if (quantity > rawMaterial.Quantity)
            {
                throw new InvalidOperationException("Insufficient raw material quantity available.");
            }

            var plan = await _treatmentPlanRepository.FindAsync(planId, includeDetails: true)
                ?? throw new InvalidOperationException("Treatment plan not found.");

            plan.AddUsedMaterialToSession(sessionId, new UsedMaterial(rawMaterialId, quantity));
        }

        public async Task<ListResultDto<PatientDto>> GetPatientsAsync()
        {
            var patients = await _patientRepository.GetListAsync();
            var patientsDtos = ObjectMapper.Map<List<Patient>, List<PatientDto>>(patients);
            return new ListResultDto<PatientDto>(patientsDtos);
        }
        public async Task<ListResultDto<DoctorDto>> GetDoctorsAsync()
        {
            throw new NotImplementedException();
        }

    }
}
