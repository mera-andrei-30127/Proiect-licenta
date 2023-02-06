using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationForDidacticPurpose.MODELS.RegisterAndLoginModels
{
    public class LoginUserEntity
    {

        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$", ErrorMessage = "Wrong email")]
        public string Email { get; set; }

        [MinLength(5, ErrorMessage = "Password must have at least {1} caracters")]
        public string Password { get; set; }

        public string Error { get; set; }
    }
}
