using Volo.Abp.Domain.Values;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class UsedMaterial : ValueObject
    {
        public Guid RawMaterialId { get; private set; }
        public decimal Quantity { get; private set; }

        public UsedMaterial() { }

        public UsedMaterial(Guid rawMaterialId, decimal quantity)
        {
            RawMaterialId = rawMaterialId;
            Quantity = quantity;
        }

        public UsedMaterial AddQuantity(decimal additionalQuantity)
        {
            if (additionalQuantity <= 0)
            {
                throw new ArgumentException("Additional quantity must be greater than zero.");
            }
            return new UsedMaterial(RawMaterialId, Quantity + additionalQuantity);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return RawMaterialId;
            yield return Quantity;
        }
    }
}
