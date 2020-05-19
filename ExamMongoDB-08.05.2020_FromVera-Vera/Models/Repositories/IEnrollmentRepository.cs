using ExamMongoDB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.Models.Repositories
{
    public interface IExamEnrollmentRepository
    {
        IEnumerable<ExamEnrollmentViewModel> GetAllEnrollment();
        
    }
}
