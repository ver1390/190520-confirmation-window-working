﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.ViewModels
{
    public class StudentExamEnrollmentViewModel
    {
        //[BsonElement("_id")]
        //[DisplayName("Programme Code")]
        ////public ObjectId Id { get; set; }
        //public string ProgrammeId { get; set; }

        //[DisplayName("Programme Name")]
        //public string ProgrammeName { get; set; }


        public List<ListExamEnrollmentViewModel> ListExamEnrollmentViewModels { get; set; }

        //[Display(Name = "Subject Code")]
        //public string SubjectCode { get; set; }

        //[Display(Name = "Subject Name")]
        //public string SubjectName { get; set; }

        //[BsonElement("date")]
        //[Display(Name = "Exam Date")]
        //public Int32 ExamDate { get; set; }

        //[Display(Name = "Room Id")]
        //public string RoomId { get; set; }
        //public string Mark { get; set; }

        //[Display(Name = "Enrolled for Exam")]
        //public bool Enrolled { get; set; }



    }
}
