using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamMongoDB.Models;
using ExamMongoDB.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExamMongoDB.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using AspNetCore.Identity.Mongo.Model;
using ExamMongoDB.Identity;

using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using AspNetCore;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace ExamMongoDB.Controllers
{
    public class ExamController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProgrammeRepository<Programme> _programmeRepository;
        private readonly IExamRepository _examRepository;
        private readonly UserManager<Student> _userManager;
        private readonly RoleManager<MongoRole> _roleManager;
        readonly IMongoCollection<Student> _userUserCollection;

        public ExamController(ILogger<HomeController> logger,
            IProgrammeRepository<Programme> programmeRepository,
            IExamRepository examRepository,
             UserManager<Student> userManager,
            RoleManager<MongoRole> roleManager,
            IMongoCollection<Student> userCollection)
        {
            _logger = logger;
            _programmeRepository = programmeRepository;
            _examRepository = examRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _userUserCollection = userCollection;
        }

        [TempData]
        public string StatusMessage1 { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        //public class ErrorException : Exception
        //{
        //    public ErrorException(string message , object e)
        //       : base(message)
        //    {
        //    }
        //}



        // GET: Exam
        [Authorize(Roles = "Sensor")]
        public ActionResult Index2()
        { 
          
            //Console.ReadLine();
            var student = new Student();
           
            var listExam = new ExamViewModel();
            listExam.Exams = _examRepository.GetAllExams();
           

            return View(listExam);
        }

        // GET: Exam by programme id trying // this should be deleted later
        [Authorize(Roles = "Student")]
        public ActionResult Index1()
        {
            var student = new Student();
            var programmeCode = student.Programmes.ProgrammeCode;
            programmeCode = "ITIS"; // this should be get the programmeCode form the index page
            var listExam = new ExamViewModel();
         
            listExam.Exams = _examRepository.GetExamsByProgrammeCode(programmeCode);

          
            return View(listExam);
        }

        // GET: Exam by programme id trying
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return RedirectToAction(nameof(Index));
         //   if (user == null) return NotFound("You are not logged in!");       
            var programmeCode = user.Programmes.ProgrammeCode;
            var model = new ExamViewModel();
            model.Exams = _examRepository.GetExamsByProgrammeCode(programmeCode);
            model.StatusMessage = StatusMessage;

            return View(model);
        }



        // GET: Exam/Details/5
        [Authorize(Roles = "Student")]
        public IActionResult Detail(string id)
        {
            var exam = _examRepository.GetExamById(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        //copied from user/edit
        //Get
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Enroll(string userName, string idExam)
        {
            var exam = _examRepository.GetExamById(idExam);
            var u = await _userManager.FindByNameAsync(userName);

            if (u == null) return NotFound("You are not logged in!");

            var i = (u.ExamEnrollment.ToArray().Length) ; 
            var model = new EnrollForExamViewModel
            {
                IdExam = u.ExamEnrollment[i].SubjectCode,               
                RoomId = u.ExamEnrollment[i].RoomId,
                Mark = u.ExamEnrollment[i].Mark,               
                UserName = u.UserName,
                ProgrammeCode = u.Programmes.ProgrammeCode,
                StatusMessage = StatusMessage,
                StatusMessage1 = StatusMessage1
            };
            return View(model);
        }

        //Post
        //* copied from user/edit
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enroll(EnrollForExamViewModel model)
        {
            var u = await _userManager.FindByIdAsync(model.UserName);
            var flag_already_reg = 0;

            if (u == null) return NotFound("You are not logged in!");
            if (u.Programmes.ProgrammeCode != model.ProgrammeCode) return NotFound(StatusMessage = "The program line is not applicable to the user");
           // if (u.Programmes.ProgrammeCode != model.ProgrammeCode) return Redirect($"/Exam/");
            var date = DateTime.Now;
            var i = (u.ExamEnrollment.ToArray().Length)-1; // it saved over the last element in the array
            if (i > -1)
            {
                for (int wow=0; wow<i+1;wow++ ) {
                    if (u.ExamEnrollment.ElementAtOrDefault(wow).SubjectCode == model.IdExam)
                    {
                        //return Window.Confirm(); install package
                        //MessageBox.Show
                        StatusMessage = " You are already previously enrolled to the selected exam.";
                        flag_already_reg = 1;
                    }
                        
                   
                }

            }
            if(flag_already_reg == 0)
            {

                //if (model.ExamDate < DateTime.Now) return NotFound(StatusMessage = "The exam date is " + model.ExamDate + " But the date today is " + date + " so  The Registration time is passed, you can not enroll now!");

                var temp = new StudentExamEnrollmentArrayViewModel();

                temp.SubjectCode = model.IdExam;
                temp.RoomId = model.RoomId;
                temp.Mark = model.Mark;

                u.ExamEnrollment.Add(temp);
                //u.ExamEnrollment.ElementAtOrDefault(i).SubjectCode = model.IdExam;
                //u.ExamEnrollment.ElementAtOrDefault(i).RoomId = model.RoomId;
                //u.ExamEnrollment.ElementAtOrDefault(i).Mark = model.Mark;

                //u.ExamEnrollment[i].SubjectCode = model.IdExam;           
                //u.ExamEnrollment[i].RoomId = model.RoomId;
                //u.ExamEnrollment[i].Mark = model.Mark;

                //set a new repository
                //after making the method, 
                await _userManager.UpdateAsync(u); // this will update the Array replacing the last element in the array
                                                   //    await _userUserCollection.InsertOneAsync(u);  
                                                   // _examRepository.Enroll.InsertMany(u); 
                                                   //put code here
                                                   //potential solution: await _EntityDomainManager<TData>.InsertAsync(u);
                                                   //public override System.Threading.Tasks.Task<TData> InsertAsync (TData data);

                StatusMessage = "You are now enrolled to the exam.";
                flag_already_reg = 0;
            }
            return Redirect("/Exam");
        }




        //public async Task<ActionResult> AddToEnroll(string idExam, string userName)
        //{
        //    var u = await _userManager.FindByNameAsync(userName);
        //    var exam = _examRepository.GetExamById(idExam);
           


        //    if (!await _roleManager.RoleExistsAsync(roleName))
        //        await _roleManager.CreateAsync(new MongoRole(roleName));

        //    if (u == null) return NotFound();

        //    await _userManager.AddToRoleAsync(u, roleName);
        //    await _userManager.AddClaimAsync(u, new Claim(ClaimTypes.Role, roleName));

        //    return Redirect($"/user/edit/{userName}");
        //}





    }
}