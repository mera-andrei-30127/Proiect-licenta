using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationForDidacticPurpose.BL.Interfaces;

namespace WebApplicationForDidacticPurpose.BL.DataValidators
{
    public class GroupValidator : IDataValidator
    {
        public bool ValidateInputData(string inputGroup)
        {
            var Groups = Enum.GetNames(typeof(DAL.Models.GroupType));
            foreach(string group in Groups)
            {
                if (group.Equals(inputGroup))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
