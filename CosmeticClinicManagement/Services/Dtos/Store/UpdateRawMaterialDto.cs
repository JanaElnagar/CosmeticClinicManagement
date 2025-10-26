using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Services.Dtos.Store
{
    public class UpdateRawMaterialDto
    {
        public string Name { get; set; } = null!;
         public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
