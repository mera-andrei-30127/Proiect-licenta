using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationForDidacticPurpose.BL.Interfaces;

namespace WebApplicationForDidacticPurpose.BL.DataValidators
{
    public class SelectValidator
    {
        private IDataValidator _dataValidator;

        public void SetValidator(IDataValidator dataValidator)
        {
            _dataValidator = dataValidator;
        }

        public bool ExecuteValidator(string data)
        {
            return _dataValidator.ValidateInputData(data);
        }
    }
}
