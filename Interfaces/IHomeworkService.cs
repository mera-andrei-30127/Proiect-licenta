using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Interfaces
{
    public interface IHomeworkService
    {
        IQueryable<HomeworkEntity> GetAllHomeworks();
        public bool AddHomework(HomeworkEntity homeworkEntity);

        public int AssignHomeworkIndex();

        public void RegroupIndexForAllHomeworks();

        public HomeworkEntity GetHomeworkById(int id);

        public HomeworkEntity GetHomeworkByName(string homeworkName);
        public bool DeleteHomeworkById(int id);
        public bool UpdateHomework(HomeworkEntity homeworkEntity);
    }
}
