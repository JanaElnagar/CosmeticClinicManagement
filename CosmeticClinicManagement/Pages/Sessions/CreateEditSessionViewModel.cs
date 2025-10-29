using CosmeticClinicManagement.Domain.ClinicManagement;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CosmeticClinicManagement.Pages.Sessions
{
    public class CreateEditSessionViewModel
    {

        [Required]
        [DataType(DataType.Date)]
        public DateTime SessionDate { get; set; }
        public List<string> Notes { get;  set; }
        public SessionStatus Status { get; set; }
       
        [HiddenInput]
        public Guid PlanId { get; set; }

        

    }
}
