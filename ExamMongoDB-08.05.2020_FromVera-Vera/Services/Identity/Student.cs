using AspNetCore.Identity.Mongo.Model;
using ExamMongoDB.Models;
using ExamMongoDB.ViewModels;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamMongoDB.Identity
{
    public class Student : MongoUser
    {
            public Student()
            {          
                Programmes = new Programme();
                ExamEnrollment = new List<StudentExamEnrollmentArrayViewModel>();
            }

            //[BsonElement("programme")]
            //[Required]
            public Programme Programmes { get; set; }

            [BsonElement("ExamEnrollment")]
            //[Required]
            public List<StudentExamEnrollmentArrayViewModel> ExamEnrollment { get; set; } // this code create emptry ExamEnrollment []

            //[BsonElement("fname")]
            [Display(Name = "First Name")]
            public string Fname { get; set; }

            //[BsonElement("lname")]
            [Display(Name = "Last Name")]
            public string Lname { get; set; }
    }
}
