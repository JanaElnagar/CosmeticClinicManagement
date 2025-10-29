using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class SessionAppService: ApplicationService, ISessionAppService
    {
        private readonly IRepository<Session, Guid> _sessionRepository;
        public SessionAppService(IRepository<Session, Guid> sessionRepository)
        {
             _sessionRepository = sessionRepository;
        }
     
        public async Task<PagedResultDto<SessionDto>>
 GetListAsync(PagedAndSortedResultRequestDto
input)
        {
            var queryable = await _sessionRepository
     .WithDetailsAsync(x => x.UsedMaterials);
            queryable = queryable.OrderBy(tp => tp.Id)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
            var sessions = await
           AsyncExecuter.ToListAsync(queryable);
            var count = await _sessionRepository.GetCountAsync();
            return new PagedResultDto<SessionDto>(
            count,
            ObjectMapper.Map<List<Session>, List<SessionDto>>
           (sessions)
            );
        }
        public async Task CreateAsync(CreateUpdateSessionDto input)
        {
            try
            {
                await _sessionRepository.InsertAsync(
            ObjectMapper.Map<CreateUpdateSessionDto, Session>
           (input)
            );
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating session. Input: {@input}", input);
                throw; // let ABP produce the 500 with details in dev
            }
        }
        public async Task<List<SessionDto>> GetListByPlanAsync(Guid PlanId)
        {
            var sessions=await _sessionRepository.GetListAsync(
                s => s.PlanId == PlanId
            );
            return ObjectMapper.Map<List<Session>, List<SessionDto>>(sessions);
        }

        public async Task<SessionDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Session, SessionDto>(
            await _sessionRepository.GetAsync(id)
            );
        }
        public async Task UpdateAsync(Guid id, CreateUpdateSessionDto
        input)
        {
            var session = await _sessionRepository.GetAsync(id);
            ObjectMapper.Map(input, session);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _sessionRepository.DeleteAsync(id);
        }
        //[Authorize]
        public async Task CreateUsedMaterialAsync(Guid SessionId, [FromBody] CreateUpdateUsedMaterialDto input)
        {
            var session = await _sessionRepository.GetAsync(SessionId);
            session.AddUsedMaterial(new UsedMaterial(input.RawMaterialId,input.Quantity));
            await _sessionRepository.UpdateAsync(session);
        }
 //       public async Task<PagedResultDto<UsedMaterialDto>>GetUsedMaterialsAsync(Guid SessionId,
 //PagedAndSortedResultRequestDto input)
 //       {
 //           /* TODO: Implementation */
 //           var queryable = await _sessionRepository
 //   .WithDetailsAsync(x => x.UsedMaterials);
 //           queryable = queryable.Where(s => s.Id == SessionId)
 //               .OrderBy(tp => tp.Id)
 //           .Skip(input.SkipCount)
 //           .Take(input.MaxResultCount);
 //           var usedMaterials = await
 //          AsyncExecuter.ToListAsync(queryable);
 //           var count = await _sessionRepository.CountAsync();
 //           return new PagedResultDto<SessionDto>(
 //           count,
 //           ObjectMapper.Map<List<UsedMaterial>, List<UsedMaterialDto>>
 //          (usedMaterials)
 //           );
 //       }

    }
}
