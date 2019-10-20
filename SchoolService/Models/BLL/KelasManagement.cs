using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using SchoolService.Areas.Admin3mill.Models;

namespace SchoolService.Models.BLL
{
    public class KelasManagement
    {
        public List<SelectListItem> ComboBoxeKelasha(int MadreseId)
        {
            Kelas_DAL dal = new Kelas_DAL(new SCEntities());
            List<SelectListItem> Result = new List<SelectListItem>();
            var Classes = dal.ListSchoolClasses(MadreseId);
            foreach (var item in Classes)
            {
                var m = new SelectListItem();
                m.Text = item.Text;
                m.Value = item.Value + "";
                Result.Add(m);
            }
            return Result;
        }

        public List<Kelas> ListKelas(int MadreseId)
        {
            Kelas_DAL KD = new Kelas_DAL(new SCEntities());
            return KD.List(MadreseId);
        }
        public string AddKelas(Kelas model, int MadreseId, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaameKelas))
            {
                ModelState.AddModelError("NaameKelas", Resource.Resource.View_ValidationError);
                return "error";
            }
            model.NaameKelas = model.NaameKelas.Trim();
            SCEntities db = new SCEntities();
            Kelas_DAL dal = new Kelas_DAL(db);
            if (dal.isExist(model, MadreseId) != null)
            {
                ModelState.AddModelError("NaameKelas", "فیلد مورد نظر تکراری می باشد");
                return "error";
            }
            model.isDeleted = false;
            dal.Create(model);
            return "success";
        }

        public string EditKelas(Kelas model)
        {
            Kelas_DAL KD = new Kelas_DAL(new SCEntities());
            KD.Edit(model);
            return "success";
         
        }

        public string DeleteKelas(int ID, int MadreseId)
        {
            var db = new SCEntities();
            Kelas_DAL DD = new Kelas_DAL(db);
            if (DD.Delete(ID, MadreseId) ==null)
                return "error";
            else 
            return "success";
        }


        public Kelas DetailKelas(int ID, int MadreseId)
        {
            Kelas_DAL KD = new Kelas_DAL(new SCEntities());
            return KD.Details(ID, MadreseId);
        }


        public string ChangeRizNomreKelasi(List<Nomre_Model> model,int F_MadreseId)
        {
            NomreKelasi_DAL DD = new NomreKelasi_DAL(new SCEntities());
            if (DD.ChangeRizNomreKelasi(model, F_MadreseId) == 1)
                return "success";
            else
                return "error";
        }
    }
}