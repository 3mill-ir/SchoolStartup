using PagedList;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SchoolService.Controllers
{
    [Authorize]
    public class AndroidServiceController : ApiController
    {

        #region Moallem

        [HttpPost]
        public string BarnameHaftegi(int MoallemId)
        {
            var asm = new AndroidServicesManagement();
            return asm.BarnameHaftegi(MoallemId);
        }
        [HttpPost]
        public string SabteNomreKelasi(int DaneshAmoozId, int BarnameHaftegiId, string Tarikh, int Hafte, string Nomre, string Tozih)
        {
            var asm = new AndroidServicesManagement();
            return asm.SetClassScore(DaneshAmoozId, BarnameHaftegiId, Tarikh, Hafte, Nomre, Tozih);
        }

        [HttpPost]
        public string SabteNomreEmtehani(int DaneshAmoozId, int BarnameHaftegiId, string Nomre, string Tozih)
        {
            var asm = new AndroidServicesManagement();
            return asm.SetExamScore(DaneshAmoozId, BarnameHaftegiId, Nomre, Tozih);
        }

        [HttpPost]
        public string HuzurGhiabeKelasi(int DaneshAmoozId, int BarnameHaftegiId, float Takhir, string Tozih, int Hafte, string Tarikh)
        {
            var asm = new AndroidServicesManagement();
            return asm.SetHuzurGhiabeKelasi(DaneshAmoozId, BarnameHaftegiId, Takhir, Tozih, Hafte, Tarikh);
        }

        [HttpPost]
        public string ListDaneshAmoozan(int KelasId)
        {
            var asm = new AndroidServicesManagement();
            return asm.ListStudents(KelasId);
        }

        #endregion

        #region Moaven

        [HttpPost]
        public string SabteFarakhan(int KarmandId, int KelasId, string Movzoo, string Matn, string TarikheFarakhan)
        {
            var asm = new AndroidServicesManagement();
            return asm.SetFarakhan(KarmandId, KelasId, Movzoo, Matn, TarikheFarakhan);
        }

        [HttpPost]
        public string MoavenRoyateFarakhan(int KelasId)
        {
            var asm = new AndroidServicesManagement();
            return asm.MoavenGetFarakhan(KelasId);
        }

        [HttpPost]
        public string ListeDaneshAmoozaneMadrese(int MoavenId)
        {
            var asm = new AndroidServicesManagement();
            return asm.ListeDaneshAmoozaneMadrese(MoavenId);
        }

        [HttpPost]
        public string ListeMaghateBeHamraheKelasHa(int MoavenId)
        {
            var asm = new AndroidServicesManagement();
            return asm.ListeMaghateBeHamraheKelasHa(MoavenId);
        }

        [HttpPost]
        public string ListeDorooseKelasha(int KelasId)
        {
            var asm = new AndroidServicesManagement();
            return asm.ListeDorooseKelasha(KelasId);
        }
        [HttpPost]
        public string MoavenHuzurGhiabeKelasi(int DaneshAmoozId, int BarnameHaftegiId, float Takhir, string Tozih, int Hafte, string Tarikh)
        {
            var asm = new AndroidServicesManagement();
            return asm.SetHuzurGhiabeKelasi(DaneshAmoozId, BarnameHaftegiId, Takhir, Tozih, Hafte, Tarikh);
        }

        [HttpPost]
        public string TashvighoTanbiheKelasi(int DaneshAmoozId, int BarnameHaftegiId, string Tarikh, int Hafte, int Emtiaz, string Tozihat)
        {
            var asm = new AndroidServicesManagement();
            return asm.SetTashvighoTanbiheKelasi(DaneshAmoozId, BarnameHaftegiId, Tarikh, Hafte, Emtiaz, Tozihat);
        }

        [HttpPost]
        public string OmureMali(int DaneshAmoozId)
        {
            var asm = new AndroidServicesManagement();
            return asm.OmureMali(DaneshAmoozId);
        }

        #endregion

        #region Ovlia
        [HttpPost]
        public string RoyateFarakhan(int DaneshAmoozId)
        {
            var asm = new AndroidServicesManagement();
            return asm.GetFarakhan(DaneshAmoozId);
        }

        [HttpPost]
        public string PishNemayesheOvlia(int DaneshAmoozId)
        {
            var asm = new AndroidServicesManagement();
            return asm.OvliaPreview(DaneshAmoozId);
        }

        [HttpPost]
        public string GozaresheHuzurghiab(int DaneshAmoozId, string mah)
        {
            var asm = new AndroidServicesManagement();
            return asm.GozaresheHuzurGhiab(DaneshAmoozId, mah);
        }

        [HttpPost]
        public string TakalifeDarsi(int DaneshAmoozId, string mah)
        {
            var asm = new AndroidServicesManagement();
            return asm.TakalifeDarsi(DaneshAmoozId, mah);
        }

        [HttpPost]
        public string TashvighVaTanbih(int DaneshAmoozId, string mah)
        {
            var asm = new AndroidServicesManagement();
            return asm.TashvighVaTanbih(DaneshAmoozId, mah);
        }

        [HttpPost]
        public string ListDoroos(int DaneshAmoozId)
        {
            var asm = new AndroidServicesManagement();
            return asm.ListDoroos(DaneshAmoozId);
        }

        [HttpPost]
        public string DaryafteNomarateDars(int DaneshAmoozId, int F_BarnameHaftegiId)
        {
            var asm = new AndroidServicesManagement();
            return asm.ListNomarateDars(DaneshAmoozId, F_BarnameHaftegiId);
        }

        [HttpPost]
        public string DaryafteKarname(int DaneshAmoozId)
        {
            var asm = new AndroidServicesManagement();
            return asm.KarnameDaneshAmuz(DaneshAmoozId);
        }

        #endregion

        #region Chat
        [HttpPost]
        public string ErsalePayamBe(string Text, string From_Id, string To_Id)
        {
            var asm = new AndroidServicesManagement();
            return asm.SendChatMessageTo(Text, From_Id, To_Id);
        }

        [HttpPost]
        public string DaryafteChat(string From_Id, string To_Id, int? page)
        {
            var asm = new AndroidServicesManagement();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            int total;
            return asm.GetChat(From_Id, To_Id, pageNumber, pageSize, out total);
        }

        [HttpPost]
        public string ListeMokhatabineOvlia(int DaneshAmuzId)
        {
            var asm = new AndroidServicesManagement();
            return asm.ListOvliaContacts(DaneshAmuzId);
        }
        #endregion
    }
}
