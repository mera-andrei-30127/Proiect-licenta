using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly WebApplicationDbContext _dbContext;
        private readonly ICompare2StringsService _compare2StringsService;
        private readonly IAttendeeService _attendeeService;
        private readonly IAttendeeHomeworksService _attendeeHomeworksService;

        public GitHubService(WebApplicationDbContext dbContext, ICompare2StringsService compare2StringsService,
            IAttendeeService attendeeService, IAttendeeHomeworksService attendeeHomeworksService)
        {
            _dbContext = dbContext;
            _compare2StringsService = compare2StringsService;
            _attendeeService = attendeeService;
            _attendeeHomeworksService = attendeeHomeworksService;
        }
        private AttendeeHomeworkEntity CreateEntity(int attendeeId, int homeworkId,
            int attendeeNameThatCopyWithId, int similarityProcent)
        {
            var attendeeHomework = new AttendeeHomeworkEntity
            {
                AttendeeId = attendeeId,
                HomeworkId = homeworkId,
                AttendeeIdThatCopyWith = attendeeNameThatCopyWithId,
                SimilarityProcent = similarityProcent
            };
            return attendeeHomework;
        }
        public bool CalculateSimilarityProcent()
        {
            if (!_attendeeHomeworksService.RemoveAllEntities())
            {
                return false;
            }
            var homeworkList = _dbContext.Homeworks.ToList();
            var attendeeIdList = _attendeeService.GetAllAttendeesId().ToList();
            var verifyOperation = false;

            for (int index1 = 0; index1 < attendeeIdList.Count() - 1; index1++)
            {
                foreach (var homework in homeworkList)
                {
                    for (int index2 = index1 + 1; index2 < attendeeIdList.Count(); index2++)
                    {
                        var attendee1 = _attendeeService.GetAttendeeById(attendeeIdList[index1]);
                        var attendee2 = _attendeeService.GetAttendeeById(attendeeIdList[index2]);
                        var compareString_1 = ReturnFilesToBeCompared(homework.Name, attendee1.Email).Item1;
                        var compareString_2 = ReturnFilesToBeCompared(homework.Name, attendee2.Email).Item1;

                        if (compareString_1 != null && compareString_2 != null)
                        {
                            var result = _compare2StringsService.Calculate(compareString_1.ToString(), compareString_2.ToString());
                            if (result > 100)
                            {
                                result = 100;
                            }
                            var attendeeHomework_1 = CreateEntity(attendee1.AttendeeId, homework.HomeworkId, attendee2.AttendeeId, result);
                            var attendeeHomework_2 = CreateEntity(attendee2.AttendeeId, homework.HomeworkId, attendee1.AttendeeId, result);
                            if (_attendeeHomeworksService.AddAttendeeHomework(attendeeHomework_1) && _attendeeHomeworksService.AddAttendeeHomework(attendeeHomework_2))
                            {
                                verifyOperation = true;
                            }

                        }

                    }
                }
            }
            return verifyOperation;

        }

        public bool CloneRepozitoryToLocalFolder()
        {
            
            var attendeeList = _dbContext.Attendees.ToList();
            var verifyCreateRepository = false;

            foreach (var attendee in attendeeList)
            {
                Uri uriResult;
                bool result = Uri.TryCreate(attendee.RepozitoryLink, UriKind.Absolute, out uriResult)
                                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                try
                {
                    Directory.Delete(Directory.GetCurrentDirectory() + @"\Repositories\");
                }
                catch (Exception)
                {

                }
                if (result)
                {
                    string repoDirectory = Directory.GetCurrentDirectory() + @"\Repositories\" + attendee.Email;
                    if (!Directory.Exists(repoDirectory))
                    {
                        Directory.CreateDirectory(repoDirectory);
                        try
                        {
                            Repository.Clone(attendee.RepozitoryLink, repoDirectory);
                            verifyCreateRepository = true;
                        }
                        catch (Exception)
                        {

                        }

                    }
                }

            }
            return verifyCreateRepository;
        }

        public (string, List<string>) ReturnFilesToBeCompared(string homeworkName, string attendeeEmail)
        {
            string repoDirectory = Directory.GetCurrentDirectory() + @"\Repositories\" + attendeeEmail + @"\" + homeworkName + @"\src";
                      
            if (Directory.Exists(repoDirectory))
            {

                var fileEntries = Directory.EnumerateFiles(repoDirectory, "*.java");
                StringBuilder stringBuilder = new StringBuilder();
                List<string> codeList = new List<string>();
                foreach (string file in fileEntries)
                {

                    using (var streamReader = new StreamReader(file))
                    {
                        string row;
                        while ((row = streamReader.ReadLine()) != null)
                        {
                            codeList.Add(row);
                            codeList.Add("\r\n");
                            stringBuilder.AppendFormat("<>{0}<>", row.ToString());
                        }

                    }
                }
                return (stringBuilder.ToString(), codeList);
            }
            return (null, null);
        }
    }
}
