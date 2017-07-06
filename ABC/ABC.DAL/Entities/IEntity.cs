using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DAL.Entities
{
    public interface IEntity
    {
    }

    public class Entity : IEntity
    {

    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}
