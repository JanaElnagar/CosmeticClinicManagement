using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Dtos.Sessions;
using CosmeticClinicManagement.Services.Implementation;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Timing;
using System.Linq;

using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
namespace CosmeticClinicManagement.Pages.TreatmentPlan
{
    public class CreateTreatmentPlanModalModel : AbpPageModel
    {

        [BindProperty]
        public CreateEditTreatmentPlanViewModel TreatmentPlan { get;  set; }

        [BindProperty]
        public List<CreateUpdateSessionDto> Session { get; set; }
        // public SelectListItem[] Categories { get; set; }
        private readonly ITreatmentPlanAppService
       _treatmentPlanAppService;
        public CreateTreatmentPlanModalModel(
        ITreatmentPlanAppService treatmentPlanAppService)
        {
            _treatmentPlanAppService = treatmentPlanAppService;
        }
        public async Task OnGetAsync() {
            TreatmentPlan = new CreateEditTreatmentPlanViewModel
            {
                CreatedDate = Clock.Now,
                Status = TreatmentPlanStatus.Ongoing,

                
            };
         //   TreatmentPlan.Sessions = Session;
            // TODO
        }
 public async Task<IActionResult> OnPostAsync()
        {
            // TODO
            await _treatmentPlanAppService.CreateAsync(
 ObjectMapper

.Map<CreateEditTreatmentPlanViewModel, CreateUpdateTreatmentPlanDto>
(TreatmentPlan)
 );
            return NoContent();
        }
    }
}

