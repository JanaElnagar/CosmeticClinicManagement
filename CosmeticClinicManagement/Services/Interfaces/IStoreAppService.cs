using CosmeticClinicManagement.Services.Dtos;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface IStoreAppService : IApplicationService
    {
        Task<List<StoreDto>> GetAllStoresAsync();
        // get list of raw materials by store id
        Task<List<RawMaterialDto>> GetRawMaterialsByStoreIdAsync(Guid storeId);
    }
}
