using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;


namespace CosmeticClinicManagement.Services.Implementation
{
    public class TreatmentPlanAppService : ApplicationService, ITreatmentPlanAppService
    {
        private readonly IRepository<TreatmentPlan, Guid> _treatmentPlanRepository;
        private readonly IRepository<Session, Guid> _sessionRepository;
        private readonly IRepository<Patient, Guid> _patientRepository;

        public TreatmentPlanAppService(IRepository<TreatmentPlan, Guid> treatmentPlanRepository, IRepository<Session, Guid> sessionRepository
            , IRepository<Patient, Guid> patientRepository)
        {
            _treatmentPlanRepository = treatmentPlanRepository;
            _sessionRepository = sessionRepository;
            _patientRepository = patientRepository;
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
            var queryable = await _treatmentPlanRepository.WithDetailsAsync(x => x.Sessions);
            var paged = queryable.OrderBy(tp => tp.Id)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
            var treatmentPlans
                = await
           AsyncExecuter.ToListAsync(paged);
            var count = await _treatmentPlanRepository.GetCountAsync();
            var planDtos = ObjectMapper.Map<List<TreatmentPlan>, List<TreatmentPlanDto>>(treatmentPlans);

            // Efficient lookup for patient names
            var patientIds = planDtos.Select(x => x.PatientId).Distinct().ToList();
            var patients = await _patientRepository.GetListAsync(p => patientIds.Contains(p.Id));
            var patientDict = patients.ToDictionary(p => p.Id, p => p.FullName);

            foreach (var plan in planDtos)
                plan.PatientFullName = patientDict.GetValueOrDefault(plan.PatientId, "Unknown");

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
 GetSessionsAsync(Guid PlanId,PagedAndSortedResultRequestDto
input)
        {
            var queryable = await _sessionRepository
     .WithDetailsAsync(x => x.UsedMaterials);
            queryable = queryable.Where(s=>s.PlanId==PlanId)
                .OrderBy(tp => tp.Id)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
            var sessions = await
           AsyncExecuter.ToListAsync(queryable);
            var count = await _sessionRepository.CountAsync(s => s.PlanId==PlanId);
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
        public async Task CreateSessionAsync(Guid PlanId , [FromBody]CreateUpdateSessionDto input)
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

    }
}
