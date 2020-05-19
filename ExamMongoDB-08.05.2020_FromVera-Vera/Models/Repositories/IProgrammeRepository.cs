using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMongoDB.Models.Repositories
{
    public interface IProgrammeRepository<TEntity>
    {
        IEnumerable<TEntity> GetAllProgrammes();
        Programme GetProgrammeById(string id);
        IList<TEntity> List();
    }
}
