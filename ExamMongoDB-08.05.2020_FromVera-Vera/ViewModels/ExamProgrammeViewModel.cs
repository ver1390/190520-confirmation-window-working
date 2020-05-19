using ExamMongoDB.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.ViewModels
{
    public class ExamProgrammeViewModel
    {
        [BsonElement("_id")]
        [Display(Name = "Subject Code - ExamId")]
        public string ExamId { get; set; }


        [BsonElement("subjectName")]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [BsonElement("ExamDate")]
        [Display(Name = "Exam Date")]
        public DateTime ExamDate { get; set; }


        //[BsonElement("programme")]
        //public string ProgrammeCode { get; set; }


        //[BsonElement("_id")]
        //[DisplayName("Programme Code")]
        //public string ProgrammeCode { get; set; }

        //[BsonElement("programmeName")]
        //[DisplayName("Programme Name")]
        //public string ProgrammeName { get; set; }


        //public List<Programme> Programmes { get; set; }





    }
}
