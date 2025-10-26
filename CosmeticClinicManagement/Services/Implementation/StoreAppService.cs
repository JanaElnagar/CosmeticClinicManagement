using CosmeticClinicManagement.Services.Interfaces;
using CosmeticClinicManagement.Domain.InventoryManagement;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using CosmeticClinicManagement.Services.Dtos.Store;
using Volo.Abp;

namespace CosmeticClinicManagement.Services
{
    public class StoreAppService : ApplicationService, IStoreAppService
    {
        private readonly IRepository<Store, Guid> _storeRepository;
        private readonly IRepository<RawMaterial, Guid> _rawRepository;

        public StoreAppService(IRepository<Store, Guid> storeRepository, IRepository<RawMaterial, Guid> rawRepository)
        {
            _storeRepository = storeRepository;
            _rawRepository = rawRepository;
        }

        public async Task<List<StoreDto>> GetAllStoresAsync()
        {
            var q = await _storeRepository.GetQueryableAsync();
            var stores = await q.Include(s => s.RawMaterials).ToListAsync();

            return stores.Select(s => new StoreDto
            {
                Id = s.Id,
                Name = s.Name,
                RawMaterials = s.RawMaterials.Select(r => new RawMaterialDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Quantity = r.Quantity,
                    Price = r.Price,
                    ExpiryDate = r.ExpiryDate,
                    StoreId = r.StoreId
                }).ToList()
            }).ToList();
        }

        public async Task<StoreDto> GetAsync(Guid id)
        {
            var q = await _storeRepository.GetQueryableAsync();
            var store = await q.Include(s => s.RawMaterials).FirstOrDefaultAsync(s => s.Id == id) ?? await _storeRepository.GetAsync(id);

            return MapToStoreDto(store);
        }

        public async Task<StoreDto> CreateAsync(CreateStoreDto input)
        {
            var store = new Store(Guid.NewGuid(), input.Name);
            await _storeRepository.InsertAsync(store, autoSave: true);

            return MapToStoreDto(store);
        }

        public async Task<StoreDto> UpdateAsync(Guid id, UpdateStoreDto input)
        {
            var store = await _storeRepository.GetAsync(id);
            store.ChangeName(input.Name);
            await _storeRepository.UpdateAsync(store, autoSave: true);
            var q = await _storeRepository.GetQueryableAsync();
            store = await q.Include(s => s.RawMaterials).FirstOrDefaultAsync(s => s.Id == id) ?? store;
            return MapToStoreDto(store);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _storeRepository.DeleteAsync(id, autoSave: true);
        }

        public async Task<List<RawMaterialDto>> GetRawMaterialsByStoreIdAsync(Guid storeId)
        {
            var q = await _rawRepository.GetQueryableAsync();
            var raws = await q.Where(r => r.StoreId == storeId).ToListAsync();

            return raws.Select(MapToRawDto).ToList();
        }

        public async Task<RawMaterialDto> GetRawMaterialAsync(Guid id)
        {
            var raw = await _rawRepository.GetAsync(id);
            return MapToRawDto(raw);
        }

        public async Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialDto input)
        {
            var raw = new RawMaterial(Guid.NewGuid(), input.Name, input.Description, input.Quantity, input.Price, input.ExpiryDate, input.StoreId);
            await _rawRepository.InsertAsync(raw, autoSave: true);
            return MapToRawDto(raw);
        }

        public async Task<RawMaterialDto> UpdateRawMaterialAsync(Guid id, UpdateRawMaterialDto input)
        {
            var q = await _rawRepository.GetQueryableAsync();
            var raw = await q.FirstOrDefaultAsync(r => r.Id == id);
            if (raw == null)
            {
                throw new UserFriendlyException("Raw material not found.");
            }

            raw.UpdateDetails(input.Name, input.Description, input.Quantity, input.Price, input.ExpiryDate);
            await _rawRepository.UpdateAsync(raw, autoSave: true);
            return MapToRawDto(raw);
        }

        public async Task DeleteRawMaterialAsync(Guid id)
        {
            var q = await _rawRepository.GetQueryableAsync();
            var raw = await q.FirstOrDefaultAsync(r => r.Id == id);
            if (raw == null)
            {
                throw new UserFriendlyException("Raw material not found.");
            }

            await _rawRepository.DeleteAsync(id, autoSave: true);
        }

        private static StoreDto MapToStoreDto(Store s) =>
            new StoreDto
            {
                Id = s.Id,
                Name = s.Name,
                RawMaterials = s.RawMaterials?.Select(MapToRawDto).ToList() ?? new List<RawMaterialDto>()
            };

        private static RawMaterialDto MapToRawDto(RawMaterial r) =>
            new RawMaterialDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Quantity = r.Quantity,
                Price = r.Price,
                ExpiryDate = r.ExpiryDate,
                StoreId = r.StoreId
            };
    }
}
