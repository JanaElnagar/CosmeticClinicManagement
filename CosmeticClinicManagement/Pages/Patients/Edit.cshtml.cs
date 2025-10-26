using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.Patients
{
    public class EditModel(IPatientAppService patientAppService) : PageModel
    {
        private readonly IPatientAppService _patientAppService = patientAppService;

        [BindProperty]
        public PatientDto Patient { get; set; }

        public async Task OnGetAsync(Guid Id)
        {
            Patient = await _patientAppService.GetAsync(Id);
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _patientAppService.UpdateAsync(id, Patient);
            return RedirectToPage("Index");
        }
    }
}
