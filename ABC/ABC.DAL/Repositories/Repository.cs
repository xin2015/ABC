using ABC.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DAL.Repositories
{
    public class Repository<TEntity> where TEntity : IEntity
    {
        protected IAccessContext Context { get; set; }
        public Repository(IAccessContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Context.GetAll<TEntity>();
        }
    }
}
