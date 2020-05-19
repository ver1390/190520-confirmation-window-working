using AspNetCore.Identity.Mongo.Model;
using ExamMongoDB.Identity;
using ExamMongoDB.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExamMongoDB.ViewModels
{
    public class StudentExamEnrollmentArrayViewModel
    {
        //public string Id { get; set; } // if we activat this attribute, it will apear in ExamEnrollmrntArray in the student collection

        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; }

      

        [Display(Name = "Room Id")]
        public string RoomId { get; set; }
        public string Mark { get; set; }

      
    }
}
