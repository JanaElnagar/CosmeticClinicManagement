using CosmeticClinicManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CosmeticClinicManagement.Authorization
{
    public class TreatmentPlanPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("TreatmentPlanManagement",
                LocalizableString.Create<CosmeticClinicManagementResource>("TreatmentPlanManagement"));
            var permission = myGroup.AddPermission("TreatmentPlanManagement",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:TreatmentPlanManagement"));

            permission.AddChild("TreatmentPlanManagement.Create",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:CreateTreatmentPlan"));

            permission.AddChild("TreatmentPlanManagement.Edit",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:EditTreatmentPlan"));

            permission.AddChild("TreatmentPlanManagement.Delete",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:DeleteTreatmentPlan"));

            permission.AddChild("TreatmentPlanManagement.View",
                LocalizableString.Create<CosmeticClinicManagementResource>("Permission:ViewTreatmentPlan"));

        }
        }
}
