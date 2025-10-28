using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Users;

namespace CosmeticClinicManagement.Pages;

[Authorize]
public class IndexModel : AbpPageModel
{
    public string PartialViewName { get; set; }
    public string UserName { get; set; }
    public void OnGet()
    {
        PartialViewName = "_DoctorDashboardPartial";
        UserName = $"{CurrentUser.Name} {CurrentUser.SurName}";
    }
}