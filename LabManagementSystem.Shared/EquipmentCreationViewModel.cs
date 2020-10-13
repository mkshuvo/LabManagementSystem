using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabManagementSystem.Shared
{
    class EquipmentCreationViewModel
    { 
        [Required]
        [Display(Name = "Equipment Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; }
        [Required]
        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
