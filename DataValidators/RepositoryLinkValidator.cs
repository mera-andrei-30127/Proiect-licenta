using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using LibGit2Sharp;

namespace WebApplicationForDidacticPurpose.BL.DataValidators
{
    public class RepositoryLinkValidator : IDataValidator
    {
        public bool ValidateInputData(string data)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(data, UriKind.Absolute, out uriResult)
                            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
