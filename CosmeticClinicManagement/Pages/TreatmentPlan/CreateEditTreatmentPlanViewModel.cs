using CosmeticClinicManagement.Domain.ClinicManagement;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace CosmeticClinicManagement.Pages.TreatmentPlan
{
    public class CreateEditTreatmentPlanViewModel
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public List<Session> Sessions { get; set; }
        //[SelectItems("status")]
        public TreatmentPlanStatus Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

    }
}
