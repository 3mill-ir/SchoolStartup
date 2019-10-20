using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Log_DAL
    {
        private SCEntities db;
        public Log_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<SchoolLog> List()
        {
            var Log = db.SchoolLog;
            return Log.ToList();
        }

        public SchoolLog Details(int id)
        {
            SchoolLog Log = db.SchoolLog.Find(id);
            if (Log == null)
            {
                return null;
            }
            return Log;
        }

        public void Create(SchoolLog Log)
        {
            db.SchoolLog.Add(Log);
            db.SaveChanges();
        }

    }

}
