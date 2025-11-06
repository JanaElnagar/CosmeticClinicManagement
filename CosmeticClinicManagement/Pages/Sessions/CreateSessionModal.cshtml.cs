//using CosmeticClinicManagement.Domain.ClinicManagement;
//using CosmeticClinicManagement.Services.Dtos;
//using CosmeticClinicManagement.Services.Implementation;
//using CosmeticClinicManagement.Services.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Volo.Abp.Timing;
//using System.Linq;

//using System.Threading.Tasks;
//using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
//namespace CosmeticClinicManagement.Pages.Sessions

//{
//    public class CreateSessionModalModel : AbpPageModel
//    {

//        [BindProperty]
//        public CreateEditSessionViewModel Session { get; set; }
//        [BindProperty(SupportsGet = true)]
//        public Guid PlanId { get; set; }

//        // public SelectListItem[] Categories { get; set; }
//        private readonly ITreatmentPlanAppService
//       _treatmentPlanAppService;
//        public CreateSessionModalModel(
//        ITreatmentPlanAppService treatmentPlanAppService)
//        {
//            _treatmentPlanAppService = treatmentPlanAppService;
//        }
//        public async Task OnGetAsync()
//        {
//            Session = new CreateEditSessionViewModel
//            {
//                PlanId=PlanId,
//                Status= SessionStatus.InProgress,


//            };
//            //   TreatmentPlan.Sessions = Session;
//            // TODO
//        }
//        public async Task<IActionResult> OnPostAsync()
//        {
//            // TODO
//            await _treatmentPlanAppService.CreateSessionAsync(
//                PlanId,ObjectMapper.Map<CreateEditSessionViewModel, CreateSessionDto>(Session)
// );
//            return NoContent();
//        }
//    }
//}

