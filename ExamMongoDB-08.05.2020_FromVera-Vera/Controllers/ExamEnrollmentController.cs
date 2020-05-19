using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
//using ExamMongoDB.Extensions;
using ExamMongoDB.Identity;
using ExamMongoDB.Identity.ManageViewModels;
//using ExamMongoDB.Mailing;
using ExamMongoDB.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExamMongoDB.Models.Repositories;
using ExamMongoDB.Models;
using MongoDB.Bson;

namespace ExamMongoDB.Controllers
{
   
    [Authorize(Roles = "Student")]
    // [Route("[controller]/[action]")]
    public class ExamEnrollmentController : Controller
    {
        private readonly UserManager<Student> _userManager;
        private readonly SignInManager<Student> _signInManager;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IExamRepository _enrollmentRepository;
        private readonly IExamEnrollmentRepository _examEnrollmentRepository;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ExamEnrollmentController(
          UserManager<Student> userManager,
          SignInManager<Student> signInManager,
          //IEmailSender emailSender,
        IExamRepository examRepository,
        IExamEnrollmentRepository examEnrollmentRepository,
          ILogger<ExamEnrollmentController> logger,
          UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _enrollmentRepository = examRepository;
            _examEnrollmentRepository = examEnrollmentRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> Index(ExamEnrollmentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

                model = new ExamEnrollmentViewModel();
                    foreach (var examEnrollment in user.ExamEnrollment)
                    {                  
                    model.SubjectCode = examEnrollment.SubjectCode;
                //model.SubjectName = examEnrollment.SubjectName;
                //model.ExamDate = examEnrollment.ExamDate;
                model.RoomId = examEnrollment.RoomId;
                model.Mark = examEnrollment.Mark;
                //model.Enrolled = examEnrollment.Enrolled;
            }
            return View(model);
        }       
    }
}
