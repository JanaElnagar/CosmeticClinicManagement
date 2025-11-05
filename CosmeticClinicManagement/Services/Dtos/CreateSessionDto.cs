using CosmeticClinicManagement.Domain.ClinicManagement;
using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class CreateSessionDto
    {
        [Required]
        public DateTime SessionDate { get; set; }

        public List<string> Notes { get; set; }
        public List<CreateUsedMaterialDto> UsedMaterials { get; set; }
        public SessionStatus Status { get; set; }
        [Required]
        public Guid PlanId { get; set; }
    }
}
