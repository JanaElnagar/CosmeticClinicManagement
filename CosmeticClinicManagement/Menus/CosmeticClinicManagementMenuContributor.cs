using CosmeticClinicManagement.Localization;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace CosmeticClinicManagement.Menus;

public class CosmeticClinicManagementMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<CosmeticClinicManagementResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                CosmeticClinicManagementMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );

        if (CosmeticClinicManagementModule.IsMultiTenant)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }
        context.Menu.AddItem(
         new ApplicationMenuItem(
         "TreatmentPlan",
         l["Menu:TreatmentPlan"],
         icon: "fas fa-shopping-cart"
         ).AddItem(
         new ApplicationMenuItem(
         "CosmeticClinicManagement.TreatmentPlans",
         l["Menu:TreatmentPlans"],
         url: "/TreatmentPlan"
         )
         )
        );

        context.Menu.AddItem(
         new ApplicationMenuItem(
         "SessionManagement",
         l["Menu:SessionManagement"],
         icon: "fas fa-shopping-cart"
         ).AddItem(
         new ApplicationMenuItem(
         "CosmeticClinicManagement.Sessions",
         l["Menu:Sessions"],
         url: "/Sessions"
         )
         )
        );

        return Task.CompletedTask;
    }
}
