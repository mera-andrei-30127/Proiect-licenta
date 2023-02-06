using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Services
{
    public class AttendeeHomeworksService : IAttendeeHomeworksService
    {
        private readonly WebApplicationDbContext _dbContext;

        public AttendeeHomeworksService(WebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool AddAttendeeHomework(AttendeeHomeworkEntity attendeeHomeworkEntity)
        {
            if (attendeeHomeworkEntity != null)
            {
                _dbContext.AttendeeHomework.Add(attendeeHomeworkEntity);
                try
                {
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
            return false;
        }

        public bool RemoveAllEntities()
        {
            _dbContext.AttendeeHomework.RemoveRange(_dbContext.AttendeeHomework.ToList());
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public bool CheckIfAttendeeIdHasSimilarity(int attendeeId)
        {
            return _dbContext.AttendeeHomework.AsNoTracking().Any(a => a.AttendeeId == attendeeId);
        }
        public List<AttendeeHomeworkEntity> ReturnEntitiesById(int attendeeId, int homeworkId)
        {
            var entities = _dbContext.AttendeeHomework.AsNoTracking().Where(a => a.AttendeeId == attendeeId && a.HomeworkId == homeworkId);
            return entities.ToList();
        }

        public double ReturnMaximumPrecentOfSimilarity(int attendeeId)
        {
            var value = 0.0;
            if (CheckIfAttendeeIdHasSimilarity(attendeeId))
            {
                value = _dbContext.AttendeeHomework.Where(a => a.AttendeeId == attendeeId).Max(a => a.SimilarityProcent);
            }
           
            return value;
        }
    }
}
