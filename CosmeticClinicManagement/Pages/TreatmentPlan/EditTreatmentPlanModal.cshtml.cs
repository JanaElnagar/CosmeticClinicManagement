using AutoMapper.Internal.Mappers;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ObjectMapping;

namespace CosmeticClinicManagement.Pages.TreatmentPlan
{
    public class EditTreatmentPlanModalModel : AbpPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public CreateEditTreatmentPlanViewModel TreatmentPlan { get; set; }
        // public SelectListItem[] Categories { get; set; }
        private readonly ITreatmentPlanAppService _treatmentPlanAppService;
        public EditTreatmentPlanModalModel(ITreatmentPlanAppService treatmentPlanAppService)
        {
           _treatmentPlanAppService = treatmentPlanAppService;
        }
        public async Task OnGetAsync()
        {
            var treatmentPlanDto = await _treatmentPlanAppService.GetAsync(Id);
            TreatmentPlan = ObjectMapper.Map<TreatmentPlanDto,
           CreateEditTreatmentPlanViewModel>(treatmentPlanDto);

            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // TODO
            await _treatmentPlanAppService.UpdateAsync(Id,
                ObjectMapper.Map<CreateEditTreatmentPlanViewModel,CreateUpdateTreatmentPlanDto>(TreatmentPlan)
 );
            return NoContent();
        }
    }
}
