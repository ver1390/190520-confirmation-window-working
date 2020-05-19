using ExamMongoDB.Identity;
using ExamMongoDB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.Models.Repositories
{
    public interface IExamRepository
    {
      
        IEnumerable<Exam> GetAllExams();       
        Exam GetExamById(string id);
        //void EnrollForExam(Student user);

        IEnumerable<Exam> GetExamsByProgrammeCode(string id);

        void Enroll(StudentExamEnrollmentArrayViewModel model);

        //void UpdateExam(string idExam, Exam exam, string idProgramme);


        //bool DeteleExam(string idExam);

    }
}
