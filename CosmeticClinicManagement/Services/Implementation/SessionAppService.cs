using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.InventoryManagement;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class SessionAppService: ApplicationService, ISessionAppService
    {
        private readonly IRepository<Session, Guid> _sessionRepository;
        private readonly IRepository<RawMaterial,Guid> _rawMaterialRepository;

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
        public async Task CreateAsync(CreateSessionDto input)
        {
            try
            {
                var session = new Session(
                  Guid.NewGuid(),
                  input.PlanId,
                  input.SessionDate,
                  input.Notes ?? new List<string>(),
                  SessionStatus.InProgress
                  );
                if (input.Notes != null && input.Notes.Any())
                {
                    foreach (var noteText in input.Notes)
                    {
                        session.AddNote(noteText); // make sure your domain entity has this method
                    }
                }
                if (input.UsedMaterials != null && input.UsedMaterials.Any())
                {
                    foreach (var materialDto in input.UsedMaterials)
                    {
                        session.AddUsedMaterial(new UsedMaterial(materialDto.RawMaterialId, materialDto.Quantity));
                    }
                    await _sessionRepository.InsertAsync(session);
                    await CurrentUnitOfWork.SaveChangesAsync();

                 

                }
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
        public async Task UpdateAsync(Guid id, UpdateSessionDto
        input)
        {
            var session = await _sessionRepository.GetAsync(id,includeDetails:true);
            session.UpdateDate(input.SessionDate);
            session.UpdateStatus(input.Status);

            session.ClearNotes();
            if (input.Notes != null && input.Notes.Any())
            {
                foreach (var noteText in input.Notes)
                {
                    session.AddNote(noteText);
                }
            }

            session.ClearUsedMaterials();
            if (input.UsedMaterials != null && input.UsedMaterials.Any())
            {
                foreach (var materialDto in input.UsedMaterials)
                {
                    session.AddUsedMaterial(
                        new UsedMaterial(materialDto.RawMaterialId, materialDto.Quantity)
                    );
                }
            }

            await _sessionRepository.UpdateAsync(session,autoSave: true);
            await CurrentUnitOfWork.SaveChangesAsync();

           // return ObjectMapper.Map<Session, SessionDto>(session);

        }
        public async Task DeleteAsync(Guid id)
        {
            await _sessionRepository.DeleteAsync(id);
        }
        //[Authorize]
        
        public async Task CreateUsedMaterialAsync(Guid SessionId, [FromBody] CreateUpdateUsedMaterialDto input)
        {
                var sessionExists = await _sessionRepository.AnyAsync(s => s.Id == SessionId);
            if (!sessionExists)
            {
                throw new EntityNotFoundException(typeof(Session), SessionId);
            }
            else
            {
                //var session = await _sessionRepository.GetAsync(SessionId);
                //session.AddUsedMaterial(new UsedMaterial(input.RawMaterialId, input.Quantity));
                //await _sessionRepository.UpdateAsync(session);
                var sessionNew = new Session(
    Guid.NewGuid(),
    Guid.Parse("93a0ae98-5e5b-fad2-3444-3a1d569637f2"),
    Clock.Now.AddDays(5),
    new List<string> { "Apply sunscreen after session", "Extra hydration requested" },
    SessionStatus.InProgress
    );
                sessionNew.AddUsedMaterial(new UsedMaterial(Guid.Parse("a4edd6eb-24a0-4d2c-95f9-275d1d90eb6e"), 2m));
                await _sessionRepository.InsertAsync(sessionNew);
            }

            
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
