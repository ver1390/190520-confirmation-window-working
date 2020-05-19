using ExamMongoDB.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ExamMongoDB.ConfigMongoDB;
using Microsoft.AspNetCore.Mvc;
using ExamMongoDB.ViewModels;

namespace ExamMongoDB.Models.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly IDatabaseSettings _context;

        public ExamRepository(IDatabaseSettings context)
        {
            _context = context;

        }

        //public void CreateExam(Exam exam, string idProgramme)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool DeteleExam(string idExam)
        //{
        //    throw new NotImplementedException();
        //}

      
        public IEnumerable<Exam> GetAllExams() // // bring a list of exams
        {
            return _context
                        .Exams
                        .Find(_ => true)
                        .ToList();
        }

       

        public Exam GetExamById(string id) // used in ExamContoller with details and enroll for exam
        {
            String idMongo = new String(id);
            FilterDefinition<Exam> filter = Builders<Exam>.Filter.Eq(m => m.idExam, idMongo);
            return _context
                  .Exams
                  .Find(filter)
                  .FirstOrDefault();
        }

        public IEnumerable<Exam> GetExamsByProgrammeCode(string programme)
        {
            String idMongo = new String(programme);
            FilterDefinition<Exam> filter = Builders<Exam>.Filter.Eq(m => m.ProgrammeCode, idMongo);
            return _context
                          .Exams
                          .Find(filter)
                          .ToList();
        }

        //tareq commented this out, try to comment out as needed, this method is not used or called
        //public async Task Enroll(StudentExamEnrollmentArrayViewModel model)
        //{
        //    try
        //    {
        //        var TestStudentNr = "ITISelev"; // Must be received from the profile information depending on which user is logged in
        //        var filter = Builders<Student>.Filter.Eq(s => s.UserName, TestStudentNr);
        //        var student = new Student();
        //        student.ExamEnrollment.FirstOrDefault().SubjectCode = model.SubjectCode;
        //        student.ExamEnrollment.FirstOrDefault().RoomId = model.RoomId;
        //        student.ExamEnrollment.FirstOrDefault().Mark = model.Mark;

        //        //                var jsonDoc = Newtonsoft.Json.JsonConvert.SerializeObject(newObject);
        //        //                var bsonDoc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jsonDoc);



        //        // PROBLEM! Inserts an object with an extra line for object type _t: Subject. Have to figure out how to remove it!


        //        var update = Builders<Student>.Update.AddToSet(student.ExamEnrollment.AsEnumerable().FirstOrDefault().SubjectCode, (Student)student);  // With AddToSet we have a built in security that only adds the ListElement if it's unique
        //        var result = await _context.Students.UpdateOneAsync(filter, update);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

       

        void IExamRepository.Enroll(StudentExamEnrollmentArrayViewModel model)
        {
            //write code to insert a new array element in ExamEnrollment
            //insert a new element in ExamEnrollment
            //then go to ExamController.cs,
            //code in video
            //ObjectId idCategoryMongo = new ObjectId(idCategory);
            //book.CategoryId = idCategoryMongo;
            //_context.Books.InsertOne(book);

            throw new NotImplementedException();
            
        }

        
    }
}
