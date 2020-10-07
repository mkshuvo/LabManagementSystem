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
        public string Description { get; set; }
        public DateTime SubmissionDate { get; set; }
        public bool Status { get; set; }
    }
}
