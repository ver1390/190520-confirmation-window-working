using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExamMongoDB.Identity;
using AspNetCore.Identity.Mongo;

using ExamMongoDB.Models;
using ExamMongoDB.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ExamMongoDB.ConfigMongoDB;
using Microsoft.AspNetCore.Authorization;
using Policy;
using ExamMongoDB.ViewModels;
using ExamMongoDB.Controllers;

namespace ExamMongoDB
{
    public class Startup
    {
        private string ConnectionString => Configuration.GetConnectionString("DefaultConnection");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

     

        public void ConfigureServices(IServiceCollection services)
        {
            //////////////// Tareq
            services.AddControllersWithViews();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IProgrammeRepository<Programme>, ProgrammeRepository>();
            services.AddScoped<IProgrammeRepository<MyRole>, MyRoleRepository>();
            services.AddScoped<IExamEnrollmentRepository, ExamEnrollmentRepository>();
            services.AddScoped <StudentViewModel>();
            services.AddScoped<EnrollForExamViewModel>();

            services.Configure<Settings>(
     options =>
     {
         options.ConnectionString =
          Configuration.GetSection("MongoDB:ConnectionString").Value;
         options.Database = Configuration.GetSection("MongoDb:Database").Value;
     });
            services.AddSingleton<IMongoClient, MongoClient>(
      _ => new MongoClient(Configuration.GetSection("MongoDb:ConnectionString").Value));

            services.AddTransient<IDatabaseSettings, DatabaseSettings>();
            ///////////////////
            services.AddIdentityMongoDbProvider<Student>(identity =>
                {
                    identity.Password.RequireDigit = false;
                    identity.Password.RequireLowercase = false;
                    identity.Password.RequireNonAlphanumeric = false;
                    identity.Password.RequireUppercase = false;
                    identity.Password.RequiredLength = 1;
                    identity.Password.RequiredUniqueChars = 0;
                } ,
                mongo =>
                {
                    mongo.ConnectionString = ConnectionString;
                }
            );

            /////////////////

          

            ///////////////////
            services.AddRazorPages();

           
            services.AddSingleton<IAuthorizationHandler, HasClaimHandler>();


            ///////////////////

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
