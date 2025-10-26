namespace CosmeticClinicManagement.Services.Dtos.Store
{
    public class CreateRawMaterialDto
    {
        public Guid StoreId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
