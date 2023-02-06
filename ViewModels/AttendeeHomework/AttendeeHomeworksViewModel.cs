using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.MODELS.ViewModels.AttendeeHomework
{
    public class AttendeeHomeworksViewModel
    {
        public virtual List<AttendeeEntity> Attendees { get; set; }

        public virtual List<HomeworkEntity> Homeworks { get; set; }


    }
}
