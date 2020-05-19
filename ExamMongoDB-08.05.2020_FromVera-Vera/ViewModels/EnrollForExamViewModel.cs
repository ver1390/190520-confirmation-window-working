using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.ViewModels
{
    public class EnrollForExamViewModel
    {
        [BsonElement("_id")]
        [Display(Name = "Subject Code - ExamId")]
        public string IdExam { get; set; }


        [BsonElement("SubjectName")]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [BsonElement("ExamDate")]
        [Display(Name = "Exam Date")]
        public DateTime ExamDate { get; set; }

       
        public string UserName { get; set; }
        public string RoomId { get; set; }
        public string Mark { get; set; }
        public bool Enrolled { get; set; }
        public string ProgrammeCode { get; set; }
        public string StatusMessage { get; set; }
        public string StatusMessage1 { get; set; }


    }
}
