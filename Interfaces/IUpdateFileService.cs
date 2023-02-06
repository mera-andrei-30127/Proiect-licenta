using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationForDidacticPurpose.BL.Interfaces
{
    public interface IUpdateFileService
    {
        public bool ValidateInputLines(IFormFile file);
        public bool ReturnDataFromUploadedFile(IFormFile file);

    }
}
