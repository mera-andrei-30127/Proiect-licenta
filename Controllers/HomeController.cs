using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using WebApplicationForDidacticPurpose.DAL.Models;
using WebApplicationForDidacticPurpose.MODELS.RegisterAndLoginModels;
using WebApplicationForDidacticPurpose.MODELS.ViewModels;

namespace WebApplicationForDidacticPurpose.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly INotyfService _notifyService;
        private readonly IAttendeeService _attendeeService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager,
            SignInManager<User> signInManager, INotyfService notyfService, IAttendeeService attendeeService,
            IUserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _notifyService = notyfService;
            _attendeeService = attendeeService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
             _signInManager.SignOutAsync().Wait();

            if (User.Identity.IsAuthenticated)
            {
                /*
                var user = await _userManager.GetUserAsync(HttpContext.User);
                
                var action = _userService.ReturnActionForUserRole(user).Item1;
                var controller = _userService.ReturnActionForUserRole(user).Item2;

                _notifyService.Information("You are logged in");
                return RedirectToAction(action, controller);*/
                _notifyService.Information("You are logged in");
                return RedirectToAction("Index", "Homework");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserEntity modelView)
        {
            _signInManager.SignOutAsync().Wait();
            modelView.Error = "";

            var email = modelView.Email;
            if (email == null || modelView.Password == null)
            {
                ModelState.AddModelError(modelView.Error, "Please complete all the empty fileds");
                return View(modelView);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, modelView.Password, false, false);
                    if (result.Succeeded)
                    {
                        _notifyService.Information("You are logged in.");
                        /*
                        var action = _userService.ReturnActionForUserRole(user).Item1;
                        var controller = _userService.ReturnActionForUserRole(user).Item2;
                        return RedirectToAction(action, controller);*/
                        return RedirectToAction("Index", "Homework");
                    }
                    else
                    {
                        ModelState.AddModelError(modelView.Error, "Incorrect email or password");
                    }
                }
                else
                {
                    ModelState.AddModelError(modelView.Error, "Email does not exist");
                }

            }

            return View(modelView);

        }

        [HttpGet]

        public IActionResult Register()
        {
             _signInManager.SignOutAsync().Wait();

            if (User.Identity.IsAuthenticated)
            {
                /*
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var action = _userService.ReturnActionForUserRole(user).Item1;
                var controller = _userService.ReturnActionForUserRole(user).Item2;

                _notifyService.Information("You are logged in");
                return RedirectToAction(action, controller);*/
                _notifyService.Information("You are logged in");
                return RedirectToAction("Index", "Homework");
            }
            return View();


        }


        [HttpPost]
        public IActionResult Register(RegisterUserEntity registerUser)
        {
            _signInManager.SignOutAsync().Wait();
            registerUser.Error = "";
            var attendee = new AttendeeEntity
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                Group = registerUser.Group,
                RepozitoryLink = registerUser.RepositoryLink
            };

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(registerUser.Error, "All fileds are required");
                return View(registerUser);
            }
            else
            if (_userService.AddUserToDb(attendee, registerUser.Password).Result)
            {
                _signInManager.SignOutAsync().Wait();
                _notifyService.Success("Registration successful!");
                return RedirectToAction("Index", "Homework");
            }
            else
            {
                ModelState.AddModelError("Email", "user already exists");
            }

            return View(registerUser);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");

            _notifyService.Information("You were logged out.");

            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }

}
