using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace CosmeticClinicManagement.Entities
{
    public class RawMaterial : AuditedEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; } = null!;

    }
}
