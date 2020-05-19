using ExamMongoDB.ConfigMongoDB;
using ExamMongoDB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.Models.Repositories
{
    public class ExamEnrollmentRepository : IExamEnrollmentRepository
    {
        private readonly IDatabaseSettings _context;
        private readonly IExamEnrollmentRepository _enrollmentRepository;

        public ExamEnrollmentRepository(IDatabaseSettings context)
        {
            _context = context;

        }
       

        public IEnumerable<ExamEnrollmentViewModel> GetAllEnrollment()
        {
            return _enrollmentRepository
                          .GetAllEnrollment()     
                          .ToList();
        }
    }
}
