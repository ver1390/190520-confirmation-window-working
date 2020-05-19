using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.Models
{
    public class Exam 
    {

        [BsonElement("_id")]
        [Display(Name = "Subject Code - ExamId")]
        public string idExam { get; set; }


        [BsonElement("SubjectName")]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [BsonElement("ExamDate")]
        [Display(Name = "Exam Date")]
        public DateTime ExamDate { get; set; }


        [BsonElement("Programme")]
        public string ProgrammeCode { get; set; } 

      
    }
}
