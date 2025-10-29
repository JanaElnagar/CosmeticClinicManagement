namespace CosmeticClinicManagement.Services.Dtos
{
    public class CreateUpdateUsedMaterialDto
    {
        public Guid RawMaterialId { get; set; }
        public decimal Quantity { get; set; }
    }
}
