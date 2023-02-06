using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Interfaces
{
    public interface IAttendeeHomeworksService
    {
        public bool AddAttendeeHomework(AttendeeHomeworkEntity attendeeHomeworkEntity);

        public List<AttendeeHomeworkEntity> ReturnEntitiesById(int attendeeId, int homeworkId);

        public bool RemoveAllEntities();
        public bool CheckIfAttendeeIdHasSimilarity(int attendeeId);

        public double ReturnMaximumPrecentOfSimilarity(int attendeeId);
    }
}
