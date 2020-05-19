using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo.Model;
using ExamMongoDB.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using ExamMongoDB.Models;
using ExamMongoDB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ExamMongoDB.Models.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ExamMongoDB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<Student> _userManager;
        private readonly RoleManager<MongoRole> _roleManager;
       private  readonly IMongoCollection<Student> _userUserCollection;
         readonly IProgrammeRepository<Programme> _programmeRepository;
        readonly IProgrammeRepository<MyRole> _myRoleIprogrammeRepository;

        public UserController(
            UserManager<Student> userManager,
            IProgrammeRepository<Programme> programmeRepository,
            IProgrammeRepository<MyRole> myRoleIprogrammeRepository
,
            RoleManager<MongoRole> roleManager,
            IMongoCollection<Student> userCollection)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userUserCollection = userCollection;
            _programmeRepository = programmeRepository;
            _myRoleIprogrammeRepository = myRoleIprogrammeRepository;
        }

        public async Task<ActionResult> Index(string id)
        {
            await _roleManager.CreateAsync(new MongoRole("Admin"));
            var role = await _roleManager.FindByNameAsync("Admin");
            await _roleManager.AddClaimAsync(role, new Claim("Administration", "ManageUsers"));
           // return View(_userManager.Users);
           
            await _roleManager.CreateAsync(new MongoRole("Sensor"));
            var role1 = await _roleManager.FindByNameAsync("Sensor");
            await _roleManager.AddClaimAsync(role1, new Claim("Administration", "ManageStudents"));
           
            return View(_userManager.Users);
        }


        public async Task<ActionResult> AddToRole(string roleName, string userName)
        {
            var u = await _userManager.FindByNameAsync(userName);

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new MongoRole(roleName));

            if (u == null) return NotFound();

            await _userManager.AddToRoleAsync(u, roleName);
            await _userManager.AddClaimAsync(u, new Claim(ClaimTypes.Role, roleName));

            return Redirect($"/user/edit/{userName}");
        }

        public async Task<ActionResult> CheckInRole(string roleName, string userName)
        {
            var u = await _userManager.FindByNameAsync(userName);

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new MongoRole(roleName));

            if (u == null) return NotFound();

            var res = await _userManager.IsInRoleAsync(u, roleName);

            return Content(res.ToString());
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByNameAsync(id);


            if (user == null) return NotFound();

          
            var programmesId = user.Programmes == null ? user.Programmes.ProgrammeCode = "-1" : user.Programmes.ProgrammeCode;

            var viewmodel = new UserMyRoleViewModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Fname = user.Fname,
                Lname = user.Lname,
                ProgrammeCode = programmesId,
                ProgrammeName = user.Programmes.ProgrammeName,

                //Programmes = _programmeRepository.List().ToList(), // makes error

               
                Programmes = ProgrammeSelectList(),
                MyRoles = MyRoleSelectList()

            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserMyRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);


            if (user == null) return NotFound();


            if (model.MyRoleId == "-1")
            {
                ViewBag.Message = "Please select an author from the list!";

                return View(GetAllProgrammes());
            }



            user.Email = model.Email;        
            user.PhoneNumber = model.PhoneNumber;        
            user.UserName = model.UserName;
            user.Fname = model.Fname;
            user.Lname = model.Lname;
            user.Programmes.ProgrammeCode = model.ProgrammeCode;
            user.Programmes.ProgrammeName = model.ProgrammeName;
            //user.Programmes.ProgrammeCode = model.Programmes.ElementAtOrDefault(0).ProgrammeCode;
            //user.Programmes.ProgrammeName = model.Programmes.ElementAtOrDefault(0).ProgrammeName;
            await _userManager.UpdateAsync(user);
            return Redirect("/user");
           // return View(GetAllProgrammes());
        }
        ///////////////////////

        List<Programme> ProgrammeSelectList()
        {
            var programmes = _programmeRepository.List().ToList();
            programmes.Insert(0, new Programme { ProgrammeCode = "", ProgrammeName = "--Select from menu--" });
            programmes.Insert(1, new Programme { ProgrammeCode = "ITIS", ProgrammeName = "IT og Info" });
            programmes.Insert(2, new Programme { ProgrammeCode = "OKLED", ProgrammeName = "Okonomi" });
            programmes.Insert(3, new Programme { ProgrammeCode = "Annet", ProgrammeName = "Annet" });

            return programmes;
        }

        UserMyRoleViewModel GetAllProgrammes()
        {
            var vmodel = new UserMyRoleViewModel
            {
                Programmes = ProgrammeSelectList()
            };

            return vmodel;
        }


        ///////////////////////
        List<MyRole> MyRoleSelectList()
        {
            var myRoles = _myRoleIprogrammeRepository.List().ToList();
            myRoles.Insert(0, new MyRole { MyRoleId="", MyRoleName = "-Select Role-" });
            myRoles.Insert(1, new MyRole { MyRoleId = "Admin", MyRoleName = "Admin" });
            myRoles.Insert(2, new MyRole { MyRoleId = "Sensor", MyRoleName = "Sensor" });
            myRoles.Insert(3, new MyRole { MyRoleId = "Student", MyRoleName = "Student" });

            return myRoles;
        }


    }
}