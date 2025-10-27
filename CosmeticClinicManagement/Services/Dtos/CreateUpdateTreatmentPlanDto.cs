using CosmeticClinicManagement.Domain.ClinicManagement;
using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class CreateUpdateTreatmentPlanDto
    {
        [Required]
        public Guid DoctorId { get; set; }
        [Required]
        public Guid PatientId { get;  set; }
        public List<Session> Sessions { get;  set; }
        public TreatmentPlanStatus Status { get; set; }

    }
}
