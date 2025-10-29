using System;
using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using CosmeticClinicManagement.Services.Dtos;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CosmeticClinicManagement.Services.Implementation
{
    public class TreatmentPlanAppService : ApplicationService, ITreatmentPlanAppService
    {
        private readonly IRepository<TreatmentPlan, Guid> _treatmentPlanRepository;
        private readonly IRepository<Session, Guid> _sessionRepository;

        public TreatmentPlanAppService(IRepository<TreatmentPlan, Guid> treatmentPlanRepository, IRepository<Session, Guid> sessionRepository)
        {
            _treatmentPlanRepository = treatmentPlanRepository;
            _sessionRepository = sessionRepository;
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
            return new PagedResultDto<TreatmentPlanDto>(
            count,
            ObjectMapper.Map<List<TreatmentPlan>, List<TreatmentPlanDto>>
           (treatmentPlans)
            ); }

        public async Task CreateAsync(CreateUpdateTreatmentPlanDto input)
        {

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
