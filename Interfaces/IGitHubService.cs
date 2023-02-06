using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationForDidacticPurpose.BL.Interfaces
{
    public interface IGitHubService
    {
        public bool CloneRepozitoryToLocalFolder();
        public bool CalculateSimilarityProcent();
        public (string, List<string>) ReturnFilesToBeCompared(string homeworkName, string attendeeEmail);

        
    }
}
