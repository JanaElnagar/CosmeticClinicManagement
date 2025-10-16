using CosmeticClinicManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Data
{
    public class ClinicDataSeedContibuter : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Store,Guid> _storeRepository;
        private readonly IRepository<RawMaterial, Guid> _rawMaterialRepository;

        public ClinicDataSeedContibuter(IRepository<Store, Guid> storeRepository, IRepository<RawMaterial, Guid> rawMaterialRepository)
        {
            _storeRepository = storeRepository;
            _rawMaterialRepository = rawMaterialRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _storeRepository.GetCountAsync() > 0)
            {
                return;
            }
            var store1 = await _storeRepository.InsertAsync(new Store
            {
                ClinicId = Guid.NewGuid(),
                Name = "Main Store",
                RawMaterials = new List<RawMaterial>()
            }, autoSave: true);
            var store2 = await _storeRepository.InsertAsync(new Store
            {
                ClinicId = Guid.NewGuid(),
                Name = "Secondary Store",
                RawMaterials = new List<RawMaterial>()
            }, autoSave: true);
            var rawMaterials = new List<RawMaterial>
            {
                new RawMaterial
                {
                    Id = Guid.NewGuid(),
                    Name = "Aloe Vera Gel",
                    Description = "Natural soothing gel for skin care.",
                    Quantity = 100,
                    Price = 15.50m,
                    ExpiryDate = DateTime.Now.AddMonths(12),
                    StoreId = store1.Id
                },
                new RawMaterial
                {
                    Id = Guid.NewGuid(),
                    Name = "Hyaluronic Acid Serum",
                    Description = "Hydrating serum for anti-aging.",
                    Quantity = 50,
                    Price = 45.00m,
                    ExpiryDate = DateTime.Now.AddMonths(18),
                    StoreId = store1.Id
                },
                new RawMaterial
                {
                    Id = Guid.NewGuid(),
                    Name = "Vitamin C Cream",
                    Description = "Brightening cream with Vitamin C.",
                    Quantity = 75,
                    Price = 30.00m,
                    ExpiryDate = DateTime.Now.AddMonths(10),
                    StoreId = store2.Id
                },
                new RawMaterial
                {
                    Id = Guid.NewGuid(),
                    Name = "Retinol Night Cream",
                    Description = "Anti-aging night cream with Retinol.",
                    Quantity = 60,
                    Price = 55.00m,
                    ExpiryDate = DateTime.Now.AddMonths(14),
                    StoreId = store2.Id
                }
            };
            foreach (var rawMaterial in rawMaterials)
            {
                await _rawMaterialRepository.InsertAsync(rawMaterial, autoSave: true);
            }
        }
    }
}
