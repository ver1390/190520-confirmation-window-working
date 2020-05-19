using ExamMongoDB.Controllers;
using ExamMongoDB.Identity;
using ExamMongoDB.Models;
using ExamMongoDB.Models.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.ConfigMongoDB
{
    public class DatabaseSettings : IDatabaseSettings
    {
        private readonly IMongoDatabase _db;

        public DatabaseSettings(IOptions<Settings> options, IMongoClient client)
        {
            _db = client.GetDatabase(options.Value.Database);
        }
        public IMongoCollection<Exam> Exams => _db.GetCollection<Exam>("exam");

        public IMongoCollection<Programme> Programmes => _db.GetCollection<Programme>("Programmes");

        public IMongoCollection<Student> Students => _db.GetCollection<Student>("student");
        public IMongoCollection<MyRole> MyRoles => _db.GetCollection<MyRole>("Role");

       

        //public IMongoCollection<ExamEnrollment> Enrollments => _db.GetCollection<ExamEnrollment>("enrollment");


    }
}
