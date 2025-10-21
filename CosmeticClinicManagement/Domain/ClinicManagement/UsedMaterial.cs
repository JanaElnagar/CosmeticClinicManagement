using Volo.Abp.Domain.Values;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class UsedMaterial : ValueObject
    {
        public Guid RawMaterialId { get; private set; }
        public decimal Quantity { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return [ RawMaterialId, Quantity ];
        }
    }
}
