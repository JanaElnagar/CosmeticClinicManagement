using CosmeticClinicManagement.Domain.ClinicManagement;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Dtos;

namespace CosmeticClinicManagement.Services.Dtos
{
    public class TreatmentPlanDto
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientFullName { get; set; }
        public int TotalSessions { get; set; }
        public int PatientAge { get; set; }
        public TreatmentPlanStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
