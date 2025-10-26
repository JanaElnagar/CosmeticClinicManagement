using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Interfaces;
using CosmeticClinicManagement.Domain.InventoryManagement;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<RawMaterialDto>> GetRawMaterialsByStoreIdAsync(Guid storeId)
        {
            var q = await _rawRepository.GetQueryableAsync();
            var raws = await q.Where(r => r.StoreId == storeId).ToListAsync();

            return raws.Select(r => new RawMaterialDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Quantity = r.Quantity,
                Price = r.Price,
                ExpiryDate = r.ExpiryDate,
                StoreId = r.StoreId
            }).ToList();
        }
    }
}
