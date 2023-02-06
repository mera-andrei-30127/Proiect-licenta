using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly WebApplicationDbContext _dbContext;

        public HomeworkService(WebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddHomework(HomeworkEntity homeworkEntity)
        {
            if (homeworkEntity == null)
            {
                return false;
            }
            try
            {
                _dbContext.Homeworks.Add(homeworkEntity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteHomeworkById(int id)
        {
            var homework = GetHomeworkById(id);

            if (homework == null)
            {
                return false;
            }
            else
            {
                _dbContext.Homeworks.Remove(homework);
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public int AssignHomeworkIndex()
        {
            return _dbContext.Homeworks.Count() + 1;
        }

        public IQueryable<HomeworkEntity> GetAllHomeworks()
        {
            return _dbContext.Homeworks.AsNoTracking().Select(t => t);
        }

        public HomeworkEntity GetHomeworkById(int id)
        {
            return _dbContext.Homeworks.AsNoTracking().FirstOrDefault(t => t.HomeworkId == id);
        }

        public bool UpdateHomework(HomeworkEntity homeworkEntity)
        {
            var homeworkToBeUpdated = _dbContext.Homeworks.AsNoTracking()
                .FirstOrDefault(t => t.HomeworkId == homeworkEntity.HomeworkId);

            homeworkToBeUpdated.Name = homeworkEntity.Name;
            homeworkToBeUpdated.Description = homeworkEntity.Description;
            homeworkToBeUpdated.Deadline = homeworkEntity.Deadline;
            homeworkToBeUpdated.RepozitoryLink = homeworkEntity.RepozitoryLink;
            if (homeworkToBeUpdated != null)
            {
                _dbContext.Update(homeworkToBeUpdated);
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
            return false;
        }

        public void RegroupIndexForAllHomeworks()
        {
            var index = 0;
            var homeworkList = _dbContext.Homeworks.ToList();
            foreach (var homework in homeworkList)
            {
                homework.Index = ++index;
                _dbContext.Homeworks.Update(homework);
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {

            }
        }

        public HomeworkEntity GetHomeworkByName(string homeworkName)
        {
            return _dbContext.Homeworks.FirstOrDefault(a => a.Name.Equals(homeworkName));
        }
    }
}
