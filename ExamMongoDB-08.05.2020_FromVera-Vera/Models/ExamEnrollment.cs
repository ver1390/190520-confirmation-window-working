//using MongoDB.Bson.Serialization.Attributes;
//using System;
//using System.ComponentModel.DataAnnotations;

//namespace ExamMongoDB.Models
//{
//    public class ExamEnrollment
//    {
//        [Display(Name = "User Name")]
//        public string Username { get; set; }
//        public bool IsEmailConfirmed { get; set; }
//        public string StatusMessage { get; set; }


//        [Display(Name = "Subject Code")]
//        public string SubjectCode { get; set; }
//        [Display(Name = "Subject Name")]
//        public string SubjectName { get; set; }


//        [BsonElement("ExamDate")]
//        [Display(Name = "Exam Date")]
//        public DateTime ExamDate { get; set; }
//        [Display(Name = "Room Id")]
//        public string RoomId { get; set; }
//        public string Mark { get; set; }
//        public bool Enrolled { get; set; }
//    }
//}

