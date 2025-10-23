using CosmeticClinicManagement.Domain.Interfaces;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace CosmeticClinicManagement.Services.Implementation
{
    public class PatientAppService : ApplicationService, IPatientAppService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IGuidGenerator _guidGenerator;

        public PatientAppService(IPatientRepository patientRepository, IGuidGenerator guidGenerator)
        {
            _patientRepository = patientRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<PatientDto> CreateAsync(PatientDto input)
        {
            input.Id = _guidGenerator.Create();
            var patient = ObjectMapper.Map<PatientDto, Patient>(input);
            patient = await _patientRepository.InsertAsync(patient, autoSave: true);
            return ObjectMapper.Map<Patient, PatientDto>(patient);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _patientRepository.DeleteAsync(id);
        }

        public async Task<PatientDto> GetAsync(Guid id)
        {
            PatientDto patientDto = ObjectMapper.Map<Patient, PatientDto>(await _patientRepository.GetAsync(id));
            return patientDto;
        }

        public async Task<PagedResultDto<PatientDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var patients = await _patientRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            var totalCount = await _patientRepository.GetCountAsync();
            return new PagedResultDto<PatientDto>(
                totalCount,
                ObjectMapper.Map<List<Patient>, List<PatientDto>>(patients)
            );
        }

        public async Task UpdateAsync(Guid id, PatientDto input)
        {
            if (id != input.Id)
            {
                throw new ArgumentException("The ID in the URL does not match the ID in the body.");
            }

            var patient = ObjectMapper.Map<PatientDto, Patient>(input);
            await _patientRepository.UpdateAsync(patient, autoSave: true);
        }
    }
}
