using Volo.Abp.Domain.Entities;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class Session : Entity<Guid>
    {
        public DateTime SessionDate { get; private set; }
        public IEnumerable<UsedMaterial> UsedMaterials { get; private set; }
        public IEnumerable<string> Notes { get; private set; }
    }
}
