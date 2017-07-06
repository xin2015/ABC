using ABC.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DAL.Repositories
{
    public class CodeTimeRepository<TEntity> : Repository<TEntity> where TEntity : ICodeTimeEntity
    {
        public CodeTimeRepository(IAccessContext context) : base(context)
        {

        }

        public bool Contain(DateTime time)
        {
            return GetAll().Any(o => o.Time == time);
        }
    }
}
