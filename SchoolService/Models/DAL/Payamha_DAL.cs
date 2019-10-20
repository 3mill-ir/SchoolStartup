using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using SchoolService.Models.AndroidJsonModel;

namespace SchoolService.Models.DAL
{
    public class Payamha_DAL
    {
        private SCEntities db;
        public Payamha_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public dynamic List(string From_Id, string To_Id, int pageNumber, int pageSize, out int total)
        {
            var Payamha = db.Payamha.Where(u => u.isDeleted == false && ((u.F_FromID == From_Id && u.F_ToID == To_Id) || (u.F_ToID == From_Id && u.F_FromID == To_Id))).OrderByDescending(u => u.CreatedDateOnUTC).Select(y => new { y.CreatedDateOnUTC, y.F_FromID, y.F_ToID, y.Text, y.ID }).ToPagedList(pageNumber, pageSize);
            var Result = new List<Chat_Model>();
            foreach (var item in Payamha.OrderBy(u => u.ID))
            {
                if (item.F_FromID == From_Id)
                    Result.Add(new Chat_Model(1, item.Text, item.CreatedDateOnUTC.Value.ToShortTimeString().ToString(), item.ID));
                else
                    Result.Add(new Chat_Model(2, item.Text, item.CreatedDateOnUTC.Value.ToShortTimeString().ToString(), item.ID));
            }
            total = Payamha.TotalItemCount;
            return Result;
        }

        public Payamha Details(int id)
        {
            Payamha Payamha = db.Payamha.Find(id);
            if (Payamha == null)
            {
                return null;
            }
            return Payamha;
        }

        public void Create(Payamha Payamha)
        {
            db.Payamha.Add(Payamha);
            db.SaveChanges();
        }

        public void Edit(Payamha Payamha)
        {
            db.Entry(Payamha).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            Payamha Payamha = db.Payamha.Find(id);
            if (Payamha != null)
            {
                Payamha.isDeleted = true;
                db.SaveChanges();
            }

        }

    }
}