using ABC.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DAL.Repositories
{
    public class MissingDataRecordRepository : Repository<MissingDataRecord>
    {
        static int maxMissTimes;

        static MissingDataRecordRepository()
        {
            maxMissTimes = 30;
        }

        public MissingDataRecordRepository(IAccessContext context) : base(context)
        {
        }

        public IQueryable<MissingDataRecord> Get(string code)
        {
            return GetAll().Where(o => o.Code == code && !o.Status && o.MissTimes <= maxMissTimes);
        }

        public IQueryable<MissingDataRecord> Get(string code, DateTime minTime)
        {
            return Get(code).Where(o => o.Time >= minTime);
        }

        public MissingDataRecord FirstOrDefault(string code, DateTime time, string conditions)
        {
            IQueryable<MissingDataRecord> query = GetAll().Where(o => o.Code == code && !o.Status && o.Time == time);
            if (!string.IsNullOrEmpty(conditions))
            {
                query = query.Where(o => o.Conditions == conditions);
            }
            return query.FirstOrDefault();
        }
    }
}
