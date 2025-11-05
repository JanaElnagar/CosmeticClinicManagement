using CosmeticClinicManagement.Domain.ClinicManagement;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace CosmeticClinicManagement.Pages.TreatmentPlan
{
    public class CreateEditTreatmentPlanViewModel
    {
        [Required]
        [SelectItems("Doctors")]
        [DisplayName("Doctor")]
        public Guid DoctorId { get; set; }
        [Required]
        [DisplayName("Patient")]
        [SelectItems("Patients")]
        public Guid PatientId { get; set; }
        //[SelectItems("status")]
        [HiddenInput]
        public TreatmentPlanStatus Status { get; set; }

        //public List<Session> Sessions { get; set; }
       
        [DataType(DataType.Date)]
        [HiddenInput(DisplayValue = false)]
        public DateTime CreatedDate { get; set; }

    }
}
