using CosmeticClinicManagement.Domain.ClinicManagement;
using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class CreateUpdateSessionDto
    {
        //public Guid PlanId { get; set; }
        [Required]
        public DateTime SessionDate { get; set; }

        public List<string> Notes { get;  set; }
        public SessionStatus Status { get; set; }
        public Guid PlanId { get; set; }
    }
}
