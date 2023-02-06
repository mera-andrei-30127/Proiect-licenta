using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.MODELS.ViewModels.Homework
{
    public class HomeworkViewModel
    {
        public int HomeworkId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string RepozitoryLink { get; set; }

        public User Trainer { get; set; }
    }
}
