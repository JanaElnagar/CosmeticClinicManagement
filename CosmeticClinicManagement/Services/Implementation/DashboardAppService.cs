using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace CosmeticClinicManagement.Services.Dashboard
{
    public abstract class DashboardAppService : ApplicationService
    {
        protected readonly ICurrentUser CurrentUser;

        protected DashboardAppService(ICurrentUser currentUser)
        {
            CurrentUser = currentUser;
        }
    }
}
