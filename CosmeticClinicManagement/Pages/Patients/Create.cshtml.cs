using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.Patients
{
    public class CreateModel(IPatientAppService patientAppService) : PageModel
    {
        private readonly IPatientAppService _patientAppService = patientAppService;

        [BindProperty]
        public CreateUpdatePatientDto Patient { get; set; } = new CreateUpdatePatientDto();
        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid patient data.");
            }
            await _patientAppService.CreateAsync(Patient);
        }

    }
}
