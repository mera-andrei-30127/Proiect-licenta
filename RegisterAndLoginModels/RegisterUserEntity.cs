using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.MODELS.RegisterAndLoginModels
{
    public class RegisterUserEntity
    {
        public int? AttendeeId { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "First name length cannot exceed {1} characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "Last name length cannot exceed {1} characters")]
        public string LastName { get; set; }

        [Required]
        public string Group { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$", ErrorMessage = "Wrong email")]
        public string Email { get; set; }

        [Required]
        [Url]
        public string RepositoryLink { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Password must have at least {1} caracters")]
        public string Password { get; set; }

        public string Error { get; set; }
    }
}
