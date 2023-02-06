using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationForDidacticPurpose.BL.Interfaces;

namespace WebApplicationForDidacticPurpose.BL.DataValidators
{
    public class PasswordValidator : IDataValidator
    {
        public bool ValidateInputData(string password)
        {
            if(password.Length < 5)
            {
                return false;
            }
            return true;
        }
    }
}
