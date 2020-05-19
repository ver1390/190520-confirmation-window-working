using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.Models
{
    public class Programme
    {
        
        [BsonElement("_id")]
        [DisplayName("Programme Code")]     
        public string ProgrammeCode { get; set; }

        [BsonElement("ProgrammeName")]
        [DisplayName("Programme Name")]
        public string ProgrammeName { get; set; }

    }
}
