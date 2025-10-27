using CosmeticClinicManagement.Domain.ClinicManagement;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class TreatmentPlanDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public List<Session> Sessions { get; set; }
        public TreatmentPlanStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
