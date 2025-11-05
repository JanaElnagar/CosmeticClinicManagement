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


namespace CosmeticClinicManagement.Services.Implementation
{
    [Authorize("TreatmentPlanManagement")]
    public class TreatmentPlanAppService : ApplicationService, ITreatmentPlanAppService
    {
        private readonly IRepository<TreatmentPlan, Guid> _treatmentPlanRepository;
        private readonly IRepository<Session, Guid> _sessionRepository;
        private readonly IRepository<Patient, Guid> _patientRepository;
        private readonly IRepository<RawMaterial, Guid> _rawMaterialRepository;
        private readonly IRepository<Volo.Abp.Identity.IdentityUser, Guid> _doctorRepository;
        private readonly UserManager<Volo.Abp.Identity.IdentityUser> _userManager;

        public TreatmentPlanAppService(IRepository<TreatmentPlan, Guid> treatmentPlanRepository, IRepository<Session, Guid> sessionRepository
            , IRepository<Patient, Guid> patientRepository,IRepository<RawMaterial,Guid> rawMaterialRepository,
            IRepository<Volo.Abp.Identity.IdentityUser,Guid> doctorRepository, UserManager<Volo.Abp.Identity.IdentityUser> userManager)
        {
            _treatmentPlanRepository = treatmentPlanRepository;
            _sessionRepository = sessionRepository;
            _patientRepository = patientRepository;
            _rawMaterialRepository=rawMaterialRepository;
            _doctorRepository = doctorRepository;
            _userManager = userManager;
        }


        //public async Task<TreatmentPlanDto> CreateAsync(string text)
        //{
        //    var todoItem = await _treatmentPlanRepository.InsertAsync(
        //        new TodoItem { Text = text }
        //    );

        //    return new TodoItemDto
        //    {
        //        Id = todoItem.Id,
        //        Text = todoItem.Text
        //    };
        //}
        public async Task<PagedResultDto<TreatmentPlanDto>> GetListAsync(
                                PagedAndSortedResultRequestDto input)
        {
            var queryable = await _treatmentPlanRepository.WithDetailsAsync(x => x.Sessions, x =>x.Doctor);
            var paged = queryable.OrderBy(tp => tp.Id)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount).Select(tp => new TreatmentPlanDto
            {
                Id = tp.Id,
                DoctorId = tp.DoctorId,
                DoctorName = tp.Doctor.Name,
                PatientId = tp.PatientId,
                Status = tp.Status,
                CreatedDate = tp.CreationTime,
                Sessions = tp.Sessions.ToList()
            });
            var planDtos
                = await
           AsyncExecuter.ToListAsync(paged);
            var count = await _treatmentPlanRepository.GetCountAsync();
           
            // Efficient lookup for patient names
            var patientIds = planDtos.Select(x => x.PatientId).Distinct().ToList();
            var patients = await _patientRepository.GetListAsync(p => patientIds.Contains(p.Id));
            var patientDict = patients.ToDictionary(p => p.Id, p => p.FullName);

            foreach (var plan in planDtos)
            {
                plan.PatientFullName = patientDict.GetValueOrDefault(plan.PatientId, "Unknown");
            }

            return new PagedResultDto<TreatmentPlanDto>(count, planDtos);
        }

        public async Task CreateAsync(CreateUpdateTreatmentPlanDto input)
        {
            var patientExists = await _patientRepository.AnyAsync(p => p.Id == input.PatientId);
            if (!patientExists)
            {
                throw new UserFriendlyException($"Patient with ID {input.PatientId} does not exist.");
            }

            await _treatmentPlanRepository.InsertAsync(
            ObjectMapper.Map<CreateUpdateTreatmentPlanDto, TreatmentPlan>
           (input)
            );
        }

        public async Task<PagedResultDto<SessionDto>>
 GetSessionsAsync(Guid planId,PagedAndSortedResultRequestDto
input)
        {
            var queryable = await _sessionRepository
     .WithDetailsAsync(x => x.UsedMaterials);
            queryable = queryable.Where(s=>s.PlanId==planId)
                .OrderByDescending(tp => tp.CreationTime)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
            var sessions = await
           AsyncExecuter.ToListAsync(queryable);
            var count = await _sessionRepository.CountAsync(s => s.PlanId==planId);
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
        public async Task CreateSessionAsync(Guid PlanId , [FromBody]CreateSessionDto input)
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
            var doctorUsers = await _userManager.GetUsersInRoleAsync("Doctor");

            // Get all users with Doctor role from IdentityServer


            return new ListResultDto<DoctorDto>(doctorUsers.Select(d => new DoctorDto
            {
                Id = d.Id,
                Name = d.Name
            }).ToList());
        }

    }
}
