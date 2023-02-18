using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_0517.Models.Repository
{
    interface IRepository<TEntity> : IDisposable
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetByID(int id);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        bool Insert(TEntity entity);
    }
}
