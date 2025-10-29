//<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Dashboard/Index");
        }
    }
}
//=======
//﻿using Microsoft.AspNetCore.Authorization;
//using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
//using Volo.Abp.Users;

//namespace CosmeticClinicManagement.Pages;

//[Authorize]
//public class IndexModel : AbpPageModel
//{
//    public string PartialViewName { get; set; }
//    public string UserName { get; set; }
//    public void OnGet()
//    {
//        PartialViewName = "_DoctorDashboardPartial";
//        UserName = $"{CurrentUser.Name} {CurrentUser.SurName}";
//    }
//}
//>>>>>>> 438533152f0871d479d0df826e57662b41b7535a
