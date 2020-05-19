using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo.Model;
using ExamMongoDB.Identity;
using ExamMongoDB.Models;
using ExamMongoDB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExamMongoDB.Controllers
{
    [Authorize(Roles = "Sensor")]
    [Authorize]
   // [Route("[controller]/[action]")]
    public class EnrollmentSensorController : Controller
    {
        private readonly UserManager<Student> _userManager;
        private readonly RoleManager<MongoRole> _roleManager;
        readonly IMongoCollection<Student> _userUserCollection;

        public EnrollmentSensorController(
            UserManager<Student> userManager,
            RoleManager<MongoRole> roleManager,
            IMongoCollection<Student> userCollection)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userUserCollection = userCollection;
        }

        [Authorize(Roles = "Sensor")]
        public ActionResult Index(string id) => View(_userManager.Users);


        [HttpGet]
        [Authorize(Roles = "Sensor")]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByNameAsync(id);

            if (user == null) return NotFound();
            ////////////
            var model = new StudentExamEnrollmentArrayViewModel
            {
                //Id = user.Id.ToString(),
                //AccessFailedCount = user.AccessFailedCount,
                //AuthenticatorKey = user.AuthenticatorKey,
                //ConcurrencyStamp = user.ConcurrencyStamp,
                //Email = user.Email,
                //EmailConfirmed = user.EmailConfirmed,
                //LockoutEnabled = user.LockoutEnabled,
                //LockoutEnd = user.LockoutEnd,
                //NormalizedEmail = user.NormalizedEmail,
                //NormalizedUserName = user.NormalizedUserName,
                //PasswordHash = user.PasswordHash,
                //PhoneNumber = user.PhoneNumber,
                //PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                //SecurityStamp = user.SecurityStamp,
                //TwoFactorEnabled = user.TwoFactorEnabled,
                //UserName = user.UserName,
                //Fname = user.Fname,
                //Lname = user.Lname,
                //ProgrammeCode = user.Programmes.ProgrammeCode,
                //ProgrammeName = user.Programmes.ProgrammeName,
                //Programmes = user.Programmes,
                SubjectCode = user.ExamEnrollment.ToArray().FirstOrDefault().SubjectCode, // these codes are not ok
                RoomId =  user.ExamEnrollment.ToArray().FirstOrDefault().RoomId,
                Mark = user.ExamEnrollment.ToArray().FirstOrDefault().Mark
            };
            return View(model);
            ////////////          
        }

        [Authorize(Roles = "Sensor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StudentExamEnrollmentArrayViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //..FindByIdAsync(model.Id);

            if (user == null) return NotFound();
         //   var examEnrollmentViewModel = new ExamEnrollmentViewModel();

           
            //user.AccessFailedCount = model.AccessFailedCount;
            //user.ConcurrencyStamp = model.ConcurrencyStamp;
            //user.Email = model.Email;
            //user.EmailConfirmed = model.EmailConfirmed;
            //user.LockoutEnabled = model.LockoutEnabled;
            //user.LockoutEnd = model.LockoutEnd;
            //user.PhoneNumber = model.PhoneNumber;
            //user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            //user.SecurityStamp = model.SecurityStamp;
            //user.TwoFactorEnabled = model.TwoFactorEnabled;
            //user.UserName = model.UserName;
            //user.Fname = model.Fname;
            //user.Lname = model.Lname;
            //user.Programmes.ProgrammeCode = model.ProgrammeCode;
            //user.Programmes.ProgrammeName = model.ProgrammeName;
            //examEnrollmentViewModel.ProgrammeId = user.Programmes.Id;
            //examEnrollmentViewModel.ProgrammeName = user.Programmes.ProgrammeName;
            //user.ExamEnrollment = model.ExamEnrollment;

            user.ExamEnrollment.ToArray().LastOrDefault().SubjectCode = model.SubjectCode;
            user.ExamEnrollment.ToArray().LastOrDefault().RoomId = model.RoomId;
            user.ExamEnrollment.ToArray().LastOrDefault().Mark = model.Mark;
            //user.Logins.LastOrDefault().UserId = _userManager.FindByIdAsync(model.Id).ToString(); // need correcting
          
            await _userManager.UpdateAsync(user);

            // await _userUserCollection.ReplaceOneAsync(x=>x.Id == user.Id, user);
            return Redirect("/EnrollmentSensor");
        }
        ////////////////////////////

        [Authorize(Roles = "Sensor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
           // var user = await _userUserCollection.DeleteOneAsync(x=>x.Id == id);
            var user = await _userUserCollection.DeleteOneAsync(x => x.Id ==ObjectId.Parse (id));
            return Redirect("/EnrollmentSensor");
        }


       
    }
}