using Volo.Abp.Domain.Entities;

namespace CosmeticClinicManagement.Entities
{
    public class Store : BasicAggregateRoot<Guid>
    {
        public Guid ClinicId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<RawMaterial> RawMaterials { get; set; } 

    }

}
