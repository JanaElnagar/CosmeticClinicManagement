using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.Sessions
{
    public class ViewSessionsModalModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid PlanId { get; set; }
        public void OnGet()
        {
        }
    }
}
