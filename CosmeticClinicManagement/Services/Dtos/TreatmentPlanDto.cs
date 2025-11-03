using CosmeticClinicManagement.Domain.ClinicManagement;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Dtos;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class TreatmentPlanDto: EntityDto<Guid>
    {
        public Guid DoctorId { get; set; }

        public string DoctorName { get; set; }
        public Guid PatientId { get; set; }
        public string PatientFullName { get; set; }
        public List<Session> Sessions { get; set; }
        public TreatmentPlanStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
