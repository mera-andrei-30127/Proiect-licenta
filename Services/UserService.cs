using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL;
using WebApplicationForDidacticPurpose.DAL.Models;
using WebApplicationForDidacticPurpose.MODELS.RegisterAndLoginModels;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.Attendee;

namespace WebApplicationForDidacticPurpose.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAttendeeService _attendeeService;
        private readonly WebApplicationDbContext _dbContext;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,
                             IAttendeeService attendeeService, WebApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _attendeeService = attendeeService;
            _dbContext = dbContext;
        }
        public async Task<bool> AddUserToDb(AttendeeEntity attendee, string password)
        {
            if (_attendeeService.CreateAttendee(attendee))
            {
                string userName = attendee.FirstName + attendee.LastName;
                var user = new User { Attendee = attendee, AttendeeId = attendee.AttendeeId, UserName = userName, Email = attendee.Email };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    attendee.User = user;
                    _attendeeService.UpdateAttendee(attendee);
                    ///var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    var signInResult = await _signInManager.UserManager.CheckPasswordAsync(user, password);
                    if (signInResult)
                    {
                        var retrievedUser = await _userManager.FindByEmailAsync(attendee.Email);
                        _ = await _userManager.AddToRoleAsync(retrievedUser, "User");
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> UpdateUserPassword(ChangePasswordViewModel viewModel)
        {
            
            var retrievedUser = await _userManager.FindByEmailAsync(viewModel.Email);
            
            var result = await _userManager.ChangePasswordAsync(retrievedUser, viewModel.CurrentPassword, viewModel.NewPassword);           
            if (result.Succeeded)
            {
                _ = await _userManager.SetEmailAsync(retrievedUser, viewModel.Email);
                await _signInManager.RefreshSignInAsync(retrievedUser);
                return true;
            }

            return false;
        }
        public (string, string) ReturnActionForUserRole(User user)
        {
            if (user != null && _userManager.IsInRoleAsync(user, "User").Result)
            {
                return ("MyWork", "Attendee");
            }
            else
            {
                return ("Index", "Attendee");
            }
        }
    }
}
