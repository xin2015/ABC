using ABC.DAL;
using ABC.DAL.Entities;
using ABC.DAL.Repositories;
using Common.Logging;
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

    public abstract class SyncBase<TEntity> : ISync where TEntity : ICodeTimeEntity
    {
        private IAccessContext context;
        protected CodeTimeRepository<TEntity> Repository { get; set; }
        protected MissingDataRecordRepository MDRRepository { get; set; }
        protected ILog Logger { get; set; }
        protected string Code { get; set; }
        protected TimeSpan Interval { get; set; }

        public SyncBase(IAccessContext context)
        {
            this.context = context;
            Repository = new CodeTimeRepository<TEntity>(context);
            MDRRepository = new MissingDataRecordRepository(context);
            Code = typeof(TEntity).Name;
        }

        #region private
        protected abstract DateTime GetTime();

        protected virtual bool IsSynchronized(DateTime time)
        {
            return Repository.Contain(time);
        }

        protected virtual void SyncStart(DateTime time)
        {

        }

        protected abstract string[] GetConditionsArray(DateTime time);

        protected virtual void AddEntity(IEnumerable<TEntity> entities)
        {
            context.Add(entities);
        }

        protected abstract IEnumerable<TEntity> GetSyncData(DateTime time, string conditions);

        protected virtual void Sync(DateTime time, string conditions)
        {
            try
            {
                AddEntity(GetSyncData(time, conditions));
            }
            catch (Exception e)
            {
                MissingDataRecord record = new MissingDataRecord()
                {
                    Code = Code,
                    Time = time,
                    Conditions = conditions,
                    Exception = e.Message,
                    CreationTime = DateTime.Now
                };
                context.Add(record);
            }
        }

        protected virtual void SyncEnd(DateTime time)
        {

        }

        protected virtual void Sync(DateTime time)
        {
            SyncStart(time);
            string[] conditionsArray = GetConditionsArray(time);
            foreach (string conditions in conditionsArray)
            {
                Sync(time, conditions);
            }
            SyncEnd(time);
        }

        protected virtual List<MissingDataRecord> GetRecords()
        {
            return MDRRepository.Get(Code).ToList();
        }

        protected virtual IEnumerable<TEntity> GetCoverData(MissingDataRecord mdr)
        {
            return GetSyncData(mdr.Time, mdr.Conditions);
        }

        protected virtual void Cover(MissingDataRecord mdr)
        {
            try
            {
                AddEntity(GetCoverData(mdr));
                mdr.Status = true;
            }
            catch (Exception e)
            {
                mdr.Exception = e.Message;
                mdr.MissTimes += 1;
            }
            mdr.LastModificationTime = DateTime.Now;
        }

        protected abstract bool IsSynchronized(DateTime time, string conditions);

        protected virtual MissingDataRecord GetRecord(DateTime time, string conditions)
        {
            return MDRRepository.FirstOrDefault(Code, time, conditions);
        }

        protected virtual void SyncOrCover(DateTime time, string conditions)
        {
            if (!IsSynchronized(time, conditions))
            {
                MissingDataRecord mdr = GetRecord(time, conditions);
                if (mdr == null)
                {
                    Sync(time, conditions);
                }
                else
                {
                    Cover(mdr);
                }
            }
        }
        #endregion

        #region public
        public void Sync()
        {
            try
            {
                DateTime time = GetTime();
                if (!IsSynchronized(time))
                {
                    Sync(time);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Sync failed.", e);
            }
        }

        public void Cover()
        {
            List<MissingDataRecord> mdrList = GetRecords();
            Cover(mdrList);
        }

        public void Cover(List<MissingDataRecord> list)
        {
            try
            {
                list.ForEach(o => Cover(o));
            }
            catch (Exception e)
            {
                Logger.Error("Cover failed.", e);
            }
        }

        public void Cover(DateTime beginTime, DateTime endTime)
        {
            try
            {
                for (DateTime time = beginTime; time <= endTime; time = time.Add(Interval))
                {
                    if (IsSynchronized(time))
                    {
                        string[] conditionsArray = GetConditionsArray(time);
                        foreach (string conditions in conditionsArray)
                        {
                            SyncOrCover(time, conditions);
                        }
                    }
                    else
                    {
                        Sync(time);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Cover failed.", e);
            }
        }
        #endregion
    }
}
