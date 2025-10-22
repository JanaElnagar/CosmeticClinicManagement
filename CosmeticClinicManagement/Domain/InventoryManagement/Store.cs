using Volo.Abp.Domain.Entities;

namespace CosmeticClinicManagement.Domain.InventoryManagement
{
    public class Store : BasicAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public List<RawMaterial> Materials { get; private set; }

        protected Store() { }

        public Store (Guid Id, string name) : base(Id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Store name cannot be empty.", nameof(name));
            }

            Name = name;
            Materials = [];
        }
    }
}
