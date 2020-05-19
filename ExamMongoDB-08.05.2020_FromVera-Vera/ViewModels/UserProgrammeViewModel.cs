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
    public class UserProgrammeViewModel
    {      
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string NormalizedEmail { get; set; }
        public string Email { get; set; }
        public string NormalizedUserName { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        ///////////////////
        //[BsonElement("fname")]
        [Display(Name = "First Name")]
        public string Fname { get; set; }

        //[BsonElement("lname")]
        [Display(Name = "Last Name")]
        public string Lname { get; set; }


        [BsonElement("_id")]
        [DisplayName("Programme Code")]
        public string ProgrammeCode { get; set; }

        [BsonElement("ProgrammeName")]
        [DisplayName("Programme Name")]
        public string ProgrammeName { get; set; }

        public List<Programme> Programmes { get; set; }

        //[BsonElement("_id")]
        [DisplayName("My Role Id")]
        public string MyRoleId { get; set; }

        [DisplayName("My Role Name")]
        public string MyRoleName { get; set; }
        public List<MyRole> MyRoles { get; set; }
    }
}
