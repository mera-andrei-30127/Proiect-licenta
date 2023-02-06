using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApplicationForDidacticPurpose.DAL.Models
{
    public class AttendeeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendeeId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name length cannot exceed {1} caracters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name length cannot exceed {1} caracters")]
        public string LastName { get; set; }

        [Required]
        public string Group { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string RepozitoryLink { get; set; }

        public virtual ICollection<AttendeeHomeworkEntity> AttendeeHomeworks { get; set; }
        public User User { get; set; }
    }
}
