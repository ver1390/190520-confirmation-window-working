using ExamMongoDB.Controllers;
using ExamMongoDB.Identity;
using ExamMongoDB.Models;
using ExamMongoDB.Models.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.ConfigMongoDB
{
    public interface IDatabaseSettings
    {
        IMongoCollection<Exam> Exams { get; }

        IMongoCollection<Programme> Programmes { get; }
        IMongoCollection<Student> Students { get; }
        IMongoCollection<MyRole> MyRoles { get; }

        //IMongoCollection<ExamEnrollment> Enrollments { get; }
    }
}
