using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class CreateUsedMaterialDto
    {
        [Required]
        public Guid RawMaterialId { get; set; }
        [Required]
        public decimal Quantity { get; set; }
    }
}
