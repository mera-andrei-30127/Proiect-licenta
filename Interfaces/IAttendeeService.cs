using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Interfaces
{
    public interface IAttendeeService
    {
        public bool CreateAttendee(AttendeeEntity attendee);
        public AttendeeEntity GetAttendeeById(int id);

        public AttendeeEntity GetAttendeeByName(string attendeeName);
        public bool UpdateAttendee(AttendeeEntity attendee);
        public bool DeleteAttendee(int id);
        public IQueryable<AttendeeEntity> GetAllAttendees();
        public AttendeeEntity GetAttendeeByEmail(String email);
        public IQueryable<int> GetAllAttendeesId();

    }
}
