using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class SessionAppService: ApplicationService, ISessionAppService
    {
        private readonly IRepository<Session, Guid> _sessionRepository;
        public SessionAppService(IRepository<Session, Guid> sessionRepository)
        {
             _sessionRepository = sessionRepository;
        }
        public async Task<List<SessionDto>> GetAsync(Guid planId)
        {
            return ObjectMapper.Map<List<Session>, List<SessionDto>>
          (await _sessionRepository.GetListAsync(s=>s.PlanId==planId));
           }
    }
}
