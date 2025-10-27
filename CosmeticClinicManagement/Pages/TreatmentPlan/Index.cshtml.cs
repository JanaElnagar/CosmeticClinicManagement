using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.TreatmentPlan
{
    public class IndexModel : PageModel
    {
        public List<TreatmentPlanDto> TreatmentPlan { get; set; } = new();

        private readonly TreatmentPlanAppService _treatmentPlanAppService;

        public IndexModel(TreatmentPlanAppService todoAppService)
        {
            _treatmentPlanAppService = todoAppService;
        }

        public async Task OnGetAsync()
        {
          //  TreatmentPlan = await _treatmentPlanAppService.GetListAsync();
        }
    }
}
