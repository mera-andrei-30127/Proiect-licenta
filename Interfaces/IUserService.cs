using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplicationForDidacticPurpose.DAL.Models;
using WebApplicationForDidacticPurpose.MODELS.RegisterAndLoginModels;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.Attendee;

namespace WebApplicationForDidacticPurpose.BL.Interfaces
{
    public interface IUserService
    {
        public Task<bool> AddUserToDb(AttendeeEntity attendee, string password);
        public (string, string) ReturnActionForUserRole(User user);

        public Task<bool> UpdateUserPassword(ChangePasswordViewModel loginUser);
    }
}
