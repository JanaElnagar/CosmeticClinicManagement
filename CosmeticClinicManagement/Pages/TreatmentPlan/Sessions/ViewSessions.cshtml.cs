using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.TreatmentPlan.Sessions
{
    public class ViewSessionsModel : PageModel
    {
        public Guid PlanId { get; set; }

        public void OnGet(Guid planId)
        {
            PlanId = planId;
        }
    }
    
}
