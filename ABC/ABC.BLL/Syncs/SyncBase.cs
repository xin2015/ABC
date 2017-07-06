using ABC.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL.Syncs
{
    public interface ISync
    {
        void Sync();
        void Cover();
        void Cover(List<MissingDataRecord> list);
        void Cover(DateTime beginTime, DateTime endTime);
    }

    class SyncBase
    {
    }
}
