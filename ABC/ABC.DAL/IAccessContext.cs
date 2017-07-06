using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DAL
{
    public interface IAccessContext
    {
        IQueryable<TEntity> GetAll<TEntity>();

        void Add(object entity);
        void Add(IEnumerable entities);
    }
}
