using CosmeticClinicManagement.Domain.ClinicManagement;
using Volo.Abp.Application.Dtos;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class SessionDto: EntityDto<Guid>
    {
        public DateTime SessionDate { get; set; }
        public List<UsedMaterial> UsedMaterials { get; set; }
        public List<string> Notes { get; set; }
        public SessionStatus Status { get; set; }
        public Guid PlanId { get; set; }

    }
}
