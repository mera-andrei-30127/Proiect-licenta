using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationForDidacticPurpose.MODELS.ViewModels
{
    public class ErrorMessageViewModel
    {
        public string Message { get; set; }

        public ErrorType Type { get; set; }

        public ErrorMessageViewModel(string message, ErrorType type)
        {
            Message = message;
            Type = type;
        }
    }
}
