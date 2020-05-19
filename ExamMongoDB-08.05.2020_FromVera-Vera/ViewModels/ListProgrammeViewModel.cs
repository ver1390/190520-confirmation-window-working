using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamMongoDB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson.Serialization.Attributes;

namespace ExamMongoDB.ViewModels
{
    public class ListProgrammeViewModel
    {
        public IEnumerable<Programme> Programmes { get; set; }
       
    }
}