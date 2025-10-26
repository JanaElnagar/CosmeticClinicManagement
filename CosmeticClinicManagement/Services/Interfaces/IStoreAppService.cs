using CosmeticClinicManagement.Services.Dtos.Store;
using Volo.Abp.Application.Services;

namespace CosmeticClinicManagement.Services.Interfaces
{
    public interface IStoreAppService : IApplicationService
    {
        Task<List<StoreDto>> GetAllStoresAsync();
        // get list of raw materials by store id
        Task<List<RawMaterialDto>> GetRawMaterialsByStoreIdAsync(Guid storeId);

        Task<StoreDto> GetAsync(Guid id);
        Task<StoreDto> CreateAsync(CreateStoreDto input);
        Task<StoreDto> UpdateAsync(Guid id, UpdateStoreDto input);
        Task DeleteAsync(Guid id);

        // RawMaterial CRUD
        Task<RawMaterialDto> GetRawMaterialAsync(Guid id);
        Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialDto input);
        Task<RawMaterialDto> UpdateRawMaterialAsync(Guid id, UpdateRawMaterialDto input);
        Task DeleteRawMaterialAsync(Guid id);
    }
}
