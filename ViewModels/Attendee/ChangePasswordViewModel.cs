using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplicationForDidacticPurpose.MODELS.ViewModels.Attendee
{
    public class ChangePasswordViewModel
    {
        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$", ErrorMessage = "Wrong email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must have at least {1} caracters")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must have at least {1} caracters")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must have at least {1} caracters")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
