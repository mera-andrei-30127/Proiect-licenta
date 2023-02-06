using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationForDidacticPurpose.MODELS.ViewModels.Attendee
{
    public class AttendeeViewModel
    {
        public int AttendeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Group { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
