using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL.Models;
using WebApplicationForDidacticPurpose.MODELS.ViewModels;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.Homework;
using WebApplicationForDidacticPurpose.Utility;

namespace WebApplicationForDidacticPurpose.Controllers
{
    public class HomeworkController : Controller
    {
        private readonly IHomeworkService _homeworkService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notifyService;
        private readonly UserManager<User> _userManager;

        public HomeworkController(IHomeworkService homeworkService, IMapper mapper,
            INotyfService notyfService, UserManager<User> userManager)
        {
            _homeworkService = homeworkService;
            _mapper = mapper;
            _notifyService = notyfService;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            int pageSize = 5;
            ViewData["CurrentFilter"] = searchString;
            var homeworks = _homeworkService.GetAllHomeworks();

            if (!string.IsNullOrEmpty(searchString))
            {
                homeworks = homeworks.Where(t => t.Name.Contains(searchString));
            }
            ///homeworks = homeworks.OrderBy(t => t.Name);
            /// var mapper = _mapper.Map<HomeworkViewModel>(homeworks);

            return View(await PaginatedList<HomeworkEntity>.CreateAsync(homeworks, pageNumber ?? 1, pageSize));
        }


        [Authorize(Roles = "Trainer")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Trainer")]
        [HttpPost]
        public async Task<IActionResult> Create(HomeworkViewModel homeworkView)
        {
            var homework = _mapper.Map<HomeworkEntity>(homeworkView);
            homework.Index = _homeworkService.AssignHomeworkIndex();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            homework.Trainer = user;
            if (_homeworkService.GetHomeworkByName(homework.Name) != null)
            {
                _notifyService.Warning("Homework name already exists");
                return View();
            }

            var result = _homeworkService.AddHomework(homework);

            if (result)
            {
                ModelState.Clear();

                _notifyService.Success("Homework created!");
                _homeworkService.RegroupIndexForAllHomeworks();
                return RedirectToAction("Index");
            }
            return View("~/Views/Shared/AnyError.cshtml",
                    new ErrorMessageViewModel("The homework could not be created, please try again.", ErrorType.RequestProcessingError));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(int homeworkId)
        {
            var homework = _homeworkService.GetHomeworkById(homeworkId);

            /// var user = await _userManager.GetUserAsync(HttpContext.User);
            if (homework != null)
            {
                ///_notifyService.Success(homework.Trainer.Email);
                return View(homework);
            }

            return View("~/Views/Shared/AnyError.cshtml",
                    new ErrorMessageViewModel("We cannot find the homework you are looking for.", ErrorType.NotFoundError));
        }
        [HttpGet]
        [Authorize(Roles = "Trainer")]
        public IActionResult DeleteGet(int homeworkId)
        {
            var homework = _homeworkService.GetHomeworkById(homeworkId);

            if (homework == null)
            {
                return View("~/Views/Shared/AnyError.cshtml",
                   new ErrorMessageViewModel("We cannot find the homework you are looking for.", ErrorType.NotFoundError));
            }

            return View("Delete", homework);
        }
        [Authorize(Roles = "Trainer")]
        [HttpPost]
        public IActionResult Delete(int homeworkId)
        {
            var homework = _homeworkService.GetHomeworkById(homeworkId);

            if (homework == null)
            {
                return View("~/Views/Shared/AnyError.cshtml",
                   new ErrorMessageViewModel("We cannot find the homework you are looking for.", ErrorType.NotFoundError));
            }

            var result = _homeworkService.DeleteHomeworkById(homeworkId);

            if (result)
            {
                _homeworkService.RegroupIndexForAllHomeworks();
                _notifyService.Success("Homework deleted");
                return RedirectToAction("Index");
            }

            return View("~/Views/Shared/AnyError.cshtml",
                    new ErrorMessageViewModel("The homework could not be deleted, please try again.", ErrorType.RequestProcessingError));
        }

        [HttpGet]
        [Authorize(Roles = "Trainer")]
        public IActionResult Edit(int homeworkId)
        {
            var homework = _homeworkService.GetHomeworkById(homeworkId);

            if (homework == null)
            {
                return View("~/Views/Shared/AnyError.cshtml",
                    new ErrorMessageViewModel("We cannot find the homework you are looking for.", ErrorType.NotFoundError));
            }

            return View(homework);
        }

        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public IActionResult Edit(HomeworkViewModel homework)
        {
            var homeworkMapper = _mapper.Map<HomeworkEntity>(homework);
            homeworkMapper.Name = _homeworkService.GetHomeworkById(homework.HomeworkId).Name;
            var result = _homeworkService.UpdateHomework(homeworkMapper);

            if (result)
            {
                _notifyService.Success("Homework was edited!");
                return RedirectToAction("Index");
            }
            else
            {
                return View("~/Views/Shared/AnyError.cshtml",
                    new ErrorMessageViewModel("The homework could not be updated, please try again.", ErrorType.RequestProcessingError));
            }
        }
    }
}
