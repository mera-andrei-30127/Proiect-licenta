using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApplicationForDidacticPurpose.DAL.Models
{
    public class HomeworkEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HomeworkId { get; set; }
        [Required]
        public int Index { get; set; }
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

        public virtual ICollection<AttendeeHomeworkEntity> AttendeeHomeworks { get; set; }

        public User Trainer { get; set; }
    }
}
