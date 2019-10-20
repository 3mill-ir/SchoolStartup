using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace SchoolService.Models.DAL
{
    public class UserInformation_DAL
    {
        private SCEntities db;
        public UserInformation_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<UserInformation> List() 
        {
            var UserInformation = db.UserInformation.Where(u => u.isDeleted == false);
            return UserInformation.ToList();
        }

        public UserInformation Details(int id)
        {
            UserInformation UserInformation = db.UserInformation.Find(id);
            if (UserInformation == null)
            {
                return null;
            }
            return UserInformation;
        }

        public string Create(UserInformation UserInformation)
        {
            db.UserInformation.Add(UserInformation);
            db.SaveChanges();
            return UserInformation.ID;
        }
 

        public void Edit(UserInformation UserInformation)
        {
            db.Entry(UserInformation).State = EntityState.Modified;
            db.SaveChanges();
        }


        public int Delete(string id)
        {
            var UserInformation = db.UserInformation.Find(id);
            if (UserInformation != null)
            {
                UserInformation.isDeleted = true;
                return db.SaveChanges();
            }
            return 0;
        }
        public int DeleteModir(string id)
        {
            var UserInformation = db.UserInformation.Find(id);
            if (UserInformation != null)
            {
                UserInformation.isDeleted = true;
                UserInformation.Madaares.ModirID = null;
               db.SaveChanges();
               return UserInformation.Madaares.ID;
            }
            return 0;
        }

        public int ChangeStatus(string id)
        {
            var UserInformation = db.UserInformation.Find(id);
            if (UserInformation != null)
            {
                UserInformation.Status = !UserInformation.Status;
                return db.SaveChanges();
            }
            return 0;
        }

    }
}