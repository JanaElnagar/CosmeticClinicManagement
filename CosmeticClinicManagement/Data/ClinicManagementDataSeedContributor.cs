using CosmeticClinicManagement.Domain.InventoryManagement;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace CosmeticClinicManagement.Data
{
    public class ClinicManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        
        private readonly IRepository <Store, Guid> _storeRepository;
        private readonly IRepository<RawMaterial, Guid> _rawMaterialRepository;

        public async Task SeedAsync(DataSeedContext context)
        {
            // Implement your data seeding logic here.
            // For example, you can create default treatment plans, stores, or patients.

            if (await _storeRepository.GetCountAsync() == 0)
            {
                var defaultStore = new Store(
                    Guid.NewGuid(),
                    "Main Clinic Store");

                var rawMaterial1 = new RawMaterial(
                    Guid.NewGuid(),
                    "Hyaluronic Acid",
                    "Used for skin hydration treatments",
                    100,
                    49.99m,
                    DateTime.Now.AddYears(1),
                    defaultStore.Id);

                var rawMaterial2 = new RawMaterial(
                    Guid.NewGuid(),
                    "Vitamin C Serum",
                    "Used for skin brightening treatments",
                    150,
                    39.99m,
                    DateTime.Now.AddYears(1),
                    defaultStore.Id);

                var rawMaterial3 = new RawMaterial(
                    Guid.NewGuid(),
                    "Retinol Cream",
                    "Used for anti-aging treatments",
                    200,
                    59.99m,
                    DateTime.Now.AddYears(1),
                    defaultStore.Id);

                await _storeRepository.InsertAsync(defaultStore);
                await _rawMaterialRepository.InsertManyAsync(new[] { rawMaterial1, rawMaterial2, rawMaterial3 });
            }
        }
    }
}
