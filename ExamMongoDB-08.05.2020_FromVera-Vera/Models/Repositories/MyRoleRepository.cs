using ExamMongoDB.ConfigMongoDB;
using ExamMongoDB.Controllers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.Models.Repositories
{
    public class MyRoleRepository : IProgrammeRepository<MyRole>
    {
        private readonly IDatabaseSettings _context;

        public MyRoleRepository(IDatabaseSettings context)
        {
            _context = context;
        }
        public IEnumerable<Programme> GetAllProgrammes()
        {
            return _context
                     .Programmes
                     .Find(_ => true)
                     .ToList();
        }

        public Programme GetProgrammeById(string id)
        {
            String idMongo = new String(id);
            FilterDefinition<Programme> filter = Builders<Programme>.Filter.Eq(m => m.ProgrammeCode, idMongo);
            return _context
                  .Programmes
                  .Find(filter)
                  .FirstOrDefault();
        }

        public IList<Programme> List()
        {
            return _context
                     .Programmes
                     .Find(_ => true)
                     .ToList();
        }

        public IList<MyRole> MyRole()
        {
            return _context
                     .MyRoles
                     .Find(_ => true)
                     .ToList();
        }

        IEnumerable<MyRole> IProgrammeRepository<MyRole>.GetAllProgrammes()
        {
            throw new NotImplementedException();
        }

        IList<MyRole> IProgrammeRepository<MyRole>.List()
        {
            return _context
                      .MyRoles
                      .Find(_ => true)
                      .ToList();
        }
    }
}
