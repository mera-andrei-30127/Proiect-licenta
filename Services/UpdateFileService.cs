using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebApplicationForDidacticPurpose.BL.DataValidators;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.BL.Services
{
    public class UpdateFileService : IUpdateFileService
    {
        private readonly IUserService _userService;

        public UpdateFileService(IUserService userService)
        {
            _userService = userService;
        }
        public bool ReturnDataFromUploadedFile(IFormFile file)
        {
            bool result = false;
            using (var fileStream = file.OpenReadStream())
            using (var reader = new StreamReader(fileStream))
            {
                string row;
                string firstRow = reader.ReadLine();
                while ((row = reader.ReadLine()) != null)
                {
                    var attendee = new AttendeeEntity
                    {
                        FirstName = row.Split(",")[0].ToString(),
                        LastName = row.Split(",")[1].ToString(),
                        Email = row.Split(",")[2].ToString(),
                        Group = row.Split(",")[3].ToString(),
                        RepozitoryLink = row.Split(",")[4].ToString()
                    };
                    var password = row.Split(",")[0].ToString() + row.Split(",")[1].ToString();
                    if (_userService.AddUserToDb(attendee, password).Result)
                    {
                        result = true;
                    }
                }

            }
            return result;
        }

        public bool ValidateInputLines(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            using (var reader = new StreamReader(fileStream))
            {
                string row;
                string firstRow = reader.ReadLine();
                while ((row = reader.ReadLine()) != null)
                {
                    char[] separators = new char[] { ',' };
                    string[] rowData = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    if (rowData.Count() != 5)
                    {
                        return false;
                    }

                    SelectValidator selectValidator = new SelectValidator();
                    string[] inputType = { "email", "group", "password", "repositoryLink" };
                    var data = "";
                    foreach (string input in inputType)
                    {
                        if (input.Equals("email"))
                        {
                            selectValidator.SetValidator(new EmailValidator());
                            data = row.Split(",")[2].ToString();
                        }
                        if (input.Equals("group"))
                        {
                            selectValidator.SetValidator(new GroupValidator());
                            data = row.Split(",")[3].ToString();

                        }
                        if (input.Equals("repositoryLink"))
                        {
                            selectValidator.SetValidator(new RepositoryLinkValidator());    
                            data = row.Split(",")[4].ToString();
                        }
                        if (input.Equals("password"))
                        {
                            selectValidator.SetValidator(new PasswordValidator());
                            data = row.Split(",")[0].ToString() + row.Split(",")[1].ToString(); ;

                        }
                        var validatorResult = selectValidator.ExecuteValidator(data);

                        if (validatorResult == false)
                        {
                            return false;
                        }
                    }
                }

            }
            return true;
        }

    }
}
