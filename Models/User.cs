using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationForDidacticPurpose.DAL.Models
{
    public class User : IdentityUser
    {
        public AttendeeEntity? Attendee { get; set; }
        public int? AttendeeId { get; set; }

        [EmailAddress]
        public override string Email { get; set; }
    }
}
