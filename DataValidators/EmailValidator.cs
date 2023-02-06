using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WebApplicationForDidacticPurpose.BL.Interfaces;

namespace WebApplicationForDidacticPurpose.BL.DataValidators
{
    public class EmailValidator : IDataValidator
    {
        public bool ValidateInputData(string email)
        {
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$");
        }
    }
}
