using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechFloor.Object;

namespace TechFloor
{
    public class JobManager
    {
        protected JobManager() { }

        private object syncobj = new object();
        
        protected Dictionary<string, ThreeField<string, List<string>, List<string>>> jobs = new Dictionary<string, ThreeField<string, List<string>, List<string>>>();

        public int AddJob(string jobid, string user, List<string> uids)
        {
            if (jobs.Count > 0 && jobs.ContainsKey(jobid))
                return jobs.Count;

            List<string> sids_ = new List<string>();

            if ((App.MainSequence as ReelTowerGroupSequence).PickingProvideJob(jobid, user, uids, ref sids_))
            {
                lock (syncobj)
                {
                    if (uids.Count == sids_.Count)
                        jobs.Add(jobid, new ThreeField<string, List<string>, List<string>>(user, uids, sids_));
                }
            }

            return jobs.Count;
        }

        public bool RemoveJob(string jobid)
        {
            bool result_ = false;

            if (jobs.Count > 0 && jobs.ContainsKey(jobid))
            {
                lock (syncobj)
                {
                    result_ = jobs.Remove(jobid);
                }
            }

            return result_;
        }

        public bool RemoveItem(string jobid, string uid)
        {
            bool result_ = false;

            if (jobs.Count > 0 && jobs.ContainsKey(jobid))
            {
                lock (syncobj)
                {
                    if (jobs[jobid].second.Count > 0)
                        result_ = jobs[jobid].second.Remove(uid);
                }
            }

            return result_;
        }

        public void ClearAll()
        {
            lock (syncobj)
            {
                jobs.Clear();
            }
        }

        public bool IsCompleted(string jobid)
        {
            if (jobs.Count > 0 && jobs.ContainsKey(jobid))
                return jobs[jobid].second.Count <= 0;
            return true;
        }
    }
}
