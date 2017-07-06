using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DAL.Entities
{
    public class MissingDataRecord : Entity
    {
        public string Code { get; set; }
        public DateTime Time { get; set; }
        public string Conditions { get; set; }
        public bool Status { get; set; }
        public int MissTimes { get; set; }
        public string Exception { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
