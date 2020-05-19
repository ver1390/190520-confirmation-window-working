using AspNetCore.Identity.Mongo.Model;
using ExamMongoDB.Identity;
using ExamMongoDB.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExamMongoDB.ViewModels
{
    public class ExamEnrollmentViewModel
    {
       
        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; }

        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [BsonElement("ExamDate")]
        [Display(Name = "Exam Date")]
        public DateTime ExamDate { get; set; }

        [Display(Name = "Room Id")]
        public string RoomId { get; set; }
        public string Mark { get; set; }

        //[Display(Name = "Enrolled for Exam")]
        //public bool Enrolled { get; set; }
    }
}
