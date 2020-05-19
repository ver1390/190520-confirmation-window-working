using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ExamMongoDB.Identity.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "User Name")]
        public string Username { get; set; }

        //    [BsonElement("fname")]
        [Display(Name = "First Name")]
        public string Fname { get; set; }

        //  [BsonElement("lname")]
        [Display(Name = "Last Name")]
        public string Lname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
