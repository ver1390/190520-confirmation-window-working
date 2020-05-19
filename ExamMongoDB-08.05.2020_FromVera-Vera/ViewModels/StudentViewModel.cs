using System;
using AspNetCore.Identity.Mongo.Model;
using ExamMongoDB.Models;
using ExamMongoDB.ViewModels;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ExamMongoDB.ViewModels
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
          //  Programmes = new Programme();
            ExamEnrollment = new List<ExamEnrollmentViewModel>();
        }

        public string Id { get; set; }

        //public DateTimeOffset? LockoutEnd { get; set; }

        //public bool TwoFactorEnabled { get; set; }

        //public bool PhoneNumberConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        //public string ConcurrencyStamp { get; set; }

        //public string SecurityStamp { get; set; }

        public string PasswordHash { get; set; }

        //public bool EmailConfirmed { get; set; }

        public string NormalizedEmail { get; set; }

        public string Email { get; set; }

        public string NormalizedUserName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //public bool LockoutEnabled { get; set; }

        //public int AccessFailedCount { get; set; }

        public string AuthenticatorKey { get; set; }

        ///////////////////
        //[BsonElement("programme")]
       // public Programme Programmes { get; set; }


        //[BsonElement("_id")]
        //[DisplayName("Programme Code")]
        //public string ProgrammeCode { get; set; }

        //[BsonElement("programmeName")]
        //[DisplayName("Programme Name")]
        //public string ProgrammeName { get; set; }


        [BsonElement("ExamEnrollment")]
        public List<ExamEnrollmentViewModel> ExamEnrollment { get; set; } // this code create emptry ExamEnrollment []

        //[BsonElement("fname")]
        [Display(Name = "First Name")]
        public string Fname { get; set; }

        //[BsonElement("lname")]
        [Display(Name = "Last Name")]
        public string Lname { get; set; }

    }
}
