using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Implementation;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Timing;
using System.Linq;

using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using NUglify.Helpers;
namespace CosmeticClinicManagement.Pages.TreatmentPlan
{
    public class CreateTreatmentPlanModalModel : AbpPageModel
    {

        [BindProperty]
        public CreateEditTreatmentPlanViewModel TreatmentPlan { get;  set; }
        [BindProperty]
        public SelectListItem[] Doctors { get; set; }
        [BindProperty]
        public SelectListItem[] Patients { get; set; }

        [BindProperty]
       public List<UpdateSessionDto> Session { get; set; }

        // public SelectListItem[] Categories { get; set; }
        private readonly ITreatmentPlanAppService
       _treatmentPlanAppService;
        //private readonly IPatientAppService _patientAppService;

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
            var doctorLookup = await _treatmentPlanAppService.GetDoctorsAsync();
          Doctors = doctorLookup.Items.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToArray();
            var patientLookup = await _treatmentPlanAppService.GetPatientsAsync();
           Patients=patientLookup.Items.Select(x => new SelectListItem(x.FullName, x.Id.ToString())).ToArray();
        
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

