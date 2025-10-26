using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.Patients
{
    public class CreateModel(IPatientAppService patientAppService) : PageModel
    {
        private readonly IPatientAppService _patientAppService = patientAppService;
        [BindProperty]
        public PatientDto Patient { get; set; } = new PatientDto();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _patientAppService.CreateAsync(Patient);
            return RedirectToPage("Index");
        }

    }
}
