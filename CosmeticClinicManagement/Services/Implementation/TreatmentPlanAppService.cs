using System;
using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using CosmeticClinicManagement.Services.Dtos.Sessions;
using Volo.Abp.ObjectMapping;


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
           // var treatmentPlan = ObjectMapper.Map<CreateUpdateTreatmentPlanDto, TreatmentPlan>
           //(input);
           // if (input.Sessions != null)
           // {
           //     treatmentPlan.Sessions = input.Sessions.Select(
           //         s=> ObjectMapper.Map<SessionDto, Session>(s)).ToList();
           // }
            await _treatmentPlanRepository.InsertAsync(
            ObjectMapper.Map<CreateUpdateTreatmentPlanDto, TreatmentPlan>
           (input)
            );
        }
        public async Task<ListResultDto<SessionsLookupDto>>
         GetSessionsAsync()
        {
            var sessions = await _sessionRepository.GetListAsync();
            return new ListResultDto<SessionsLookupDto>(
            ObjectMapper
            .Map<List<Session>, List<SessionsLookupDto>>
           (sessions)
            );
        }

        //public async Task<TreatmentPlanDto> GetAsync(Guid id)
        //{
        //    return ObjectMapper.Map<Product, ProductDto>
        //   (await _productRepository
        //    .GetAsync(id));
        //}

        //public async Task UpdateAsync(Guid id, CreateUpdateProductDto input)
        //{
        //    var product = await _productRepository.GetAsync(id);
        //    ObjectMapper.Map<CreateUpdateProductDto, Product>
        //   (input, product);
        //}


        public async Task DeleteAsync(Guid id)
        {
            await _treatmentPlanRepository.DeleteAsync(id);
        }

    }
}
