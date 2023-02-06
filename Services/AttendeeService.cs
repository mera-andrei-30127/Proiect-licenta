using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Services
{
    public class AttendeeService : IAttendeeService
    {
        private readonly WebApplicationDbContext _dbContext;

        public AttendeeService(WebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private bool IsEmailUsed(string email)
        {
            return _dbContext.Attendees.Any(a => a.Email == email);
        }
        public bool CreateAttendee(AttendeeEntity attendee)
        {
            if (attendee == null || IsEmailUsed(attendee.Email))
            {
                return false;
            }
            try
            {
                _dbContext.Attendees.Add(attendee);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAttendee(int id)
        {
            var attendee = GetAttendeeById(id);

            if (attendee == null)
                return false;
            else
                try
                {
                    _dbContext.Attendees.Remove(attendee);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
        }
        public IQueryable<int> GetAllAttendeesId()
        {
            return _dbContext.Attendees.AsNoTracking().Select(x => x.AttendeeId);
        }
        public IQueryable<AttendeeEntity> GetAllAttendees()
        {
            return _dbContext.Attendees.AsNoTracking().Select(x => x);
        }

        public AttendeeEntity GetAttendeeByEmail(string email)
        {
            return _dbContext.Attendees.AsNoTracking().Include(a => a.User).FirstOrDefault(a => string.Equals(a.Email, email));
        }

        public AttendeeEntity GetAttendeeById(int id)
        {
            return _dbContext.Attendees.FirstOrDefault(a => a.AttendeeId == id);
        }

        public bool UpdateAttendee(AttendeeEntity attendee)
        {
            if (attendee is null)
            {
                return false;
            }
            if (IsEmailUsed(attendee.Email) && attendee.AttendeeId != GetAttendeeByEmail(attendee.Email).AttendeeId)
            {
                return false;
            }
            _dbContext.Update(attendee);
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AttendeeEntity GetAttendeeByName(string attendeeName)
        {
            return _dbContext.Attendees.AsNoTracking().FirstOrDefault(a => (a.FirstName + a.LastName).Equals(attendeeName));
        }
    }
}
