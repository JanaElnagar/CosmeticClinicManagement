using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace CosmeticClinicManagement.Pages.TreatmentPlan
{
    public class IndexModel(ITreatmentPlanAppService treatmentPlanAppService) : AbpPageModel
    {
        private readonly ITreatmentPlanAppService _treatmentPlanAppService = treatmentPlanAppService;
        public List<TreatmentPlanDto> TreatmentPlans { get; set; }

        public async Task OnGetAsync()
        {
            var temp = await _treatmentPlanAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = 100,
                SkipCount = 0,
                Sorting = "CreationTime DESC"
            });

            TreatmentPlans = [.. temp.Items];
        }
    }
}
