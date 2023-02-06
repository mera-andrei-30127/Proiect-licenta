using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL.Models;
using WebApplicationForDidacticPurpose.MODELS.ViewModels;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.Attendee;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.AttendeeHomework;

namespace WebApplicationForDidacticPurpose.Controllers
{
    [Authorize]
    public class AttendeeController : Controller
    {
        private readonly IAttendeeService _attendeeService;
        private readonly IUserService _userService;
        private readonly IUpdateFileService _updateFileService;
        private readonly IGitHubService _gitHubService;
        private readonly IMapper _mapper;
        public readonly INotyfService _notifyService;
        private readonly IHomeworkService _homeworkService;

        public AttendeeController(IHomeworkService homeworkService, INotyfService notifyService,
            IAttendeeService attendeeService, IUserService userService, IUpdateFileService updateFileService,
            IGitHubService gitHubService, IMapper mapper)
        {
            _attendeeService = attendeeService;
            _userService = userService;
            _updateFileService = updateFileService;
            _gitHubService = gitHubService;
            _mapper = mapper;
            _homeworkService = homeworkService;
            _notifyService = notifyService;
        }

        [Authorize(Roles = "Trainer")]
        [HttpGet]
        public IActionResult Index()
        {
            List<AttendeeEntity> attendees = _attendeeService.GetAllAttendees().ToList();
            _gitHubService.CloneRepozitoryToLocalFolder();
            _gitHubService.CalculateSimilarityProcent();
            var viewModel = new AttendeeHomeworksViewModel()
            {
                Attendees = attendees,
            };
            ViewBag.Message = TempData["Message"];
            return View(viewModel);
        }
        public IActionResult Similarities(int attendeeId)
        {
            var attendeeView = new AttendeeSimilaritiesViewModel
            {
                AttendeeId_1 = attendeeId
            };
            return View(attendeeView);
        }
        public IActionResult CodePreview(int attendeeId_1, int attendeeId_2, int homeworkId)
        {
            var attendeeView = new AttendeeSimilaritiesViewModel
            {
                AttendeeId_1 = attendeeId_1,
                AttendeeId_2 = attendeeId_2,
                HomeworkId = homeworkId
            };
            return View(attendeeView);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult MyAccount()
        {
            var attendee = _attendeeService.GetAttendeeByName(User.Identity.Name);
            if (attendee != null)
            {
                return View(attendee);
            }
            else
            {
                return View("~/Views/Shared/AnyError.cshtml",
                   new ErrorMessageViewModel("We cannot find the page you are looking for.", ErrorType.NotFoundError));
            }

        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult MyAccount(AttendeeEntity attendeeEntity)
        {
            var attendee = _attendeeService.GetAttendeeById(attendeeEntity.AttendeeId);
            attendee.Group = attendeeEntity.Group;
            attendee.RepozitoryLink = attendeeEntity.RepozitoryLink;

            var result = _attendeeService.UpdateAttendee(attendee);
            if (result)
            {
                _notifyService.Success("Account was edited!");
                return View(attendee);
            }
            else
            {
                return View("~/Views/Shared/AnyError.cshtml",
                   new ErrorMessageViewModel("Account could not be updated, please try again", ErrorType.RequestProcessingError));
            }

        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult PasswordChange(int attendeeId)
        {
            var attendee = _attendeeService.GetAttendeeById(attendeeId);
            var passwordModel = new ChangePasswordViewModel
            {
                Email = attendee.Email
            };
            if (passwordModel != null)
            {
                return View(passwordModel);
            }
            else
            {
                return View("~/Views/Shared/AnyError.cshtml",
                   new ErrorMessageViewModel("We cannot find the page you are looking for.", ErrorType.NotFoundError));
            }

        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult PasswordChange(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.UpdateUserPassword(viewModel);
                if (result.Result)
                {
                    _notifyService.Success("Password was changed!");
                    return RedirectToAction("MyAccount");
                }
                else
                {
                    return View("~/Views/Shared/AnyError.cshtml",
                       new ErrorMessageViewModel("Password could not be updated, please try again", ErrorType.RequestProcessingError));
                }
            }
            return View("~/Views/Shared/AnyError.cshtml",
                       new ErrorMessageViewModel("Password could not be updated, please try again", ErrorType.RequestProcessingError));

        }

        [Authorize(Roles = "Trainer")]
        [HttpPost]
        public IActionResult UploadFileToCreateNewUsers(List<IFormFile> files)
        {

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension != ".csv")
                {
                    TempData["Message"] = "Please upload only csv files";
                    return RedirectToAction("Index");
                }
                /*
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string filePath = Path.Combine(basePath, fileName);*/

                if (_updateFileService.ValidateInputLines(file))
                {
                    if (_updateFileService.ReturnDataFromUploadedFile(file))
                    {
                        TempData["Message"] = "Opperation completed succesfully";
                    }
                    else
                    {
                        TempData["Message"] = "All attendees already exists";
                        return RedirectToAction("Index", "Attendee");
                    }
                }
                else
                {
                    TempData["Message"] = "Bad input data";
                    return RedirectToAction("Index", "Attendee");
                }


            }
            return RedirectToAction("Index", "Attendee");
        }


    }
}