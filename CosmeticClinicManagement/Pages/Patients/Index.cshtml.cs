using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace CosmeticClinicManagement.Pages.Patients
{
    public class IndexModel(IPatientAppService appService) : AbpPageModel
    {
        private readonly IPatientAppService _patientAppService = appService;
        public List<PatientDto> Patients { get; set; } = [];

        public async Task OnGetAsync()
        {
            Patients = await _patientAppService.GetAllPatientsAsync();
        }
    }
}
