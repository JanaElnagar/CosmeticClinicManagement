using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Implementation;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.TreatmentPlan.Sessions
{
    public class IndexModel : PageModel
    {
        
        public async Task OnGetAsync()
        {
            //  TreatmentPlan = await _treatmentPlanAppService.GetListAsync();
        }
    }
}
