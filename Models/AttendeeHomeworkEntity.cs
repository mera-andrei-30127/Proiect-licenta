using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApplicationForDidacticPurpose.DAL.Models
{
    public class AttendeeHomeworkEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendeeHomeworkId { get; set; }
        [Required]
        public int AttendeeId { get; set; }
        public AttendeeEntity Attendee { get; set; }
        [Required]
        public int HomeworkId { get; set; }
        public HomeworkEntity Homework { get; set; }
        public int AttendeeIdThatCopyWith { get; set; }
        public int SimilarityProcent { get; set; }
    }
}
