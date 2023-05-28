using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    
    public class AdvHomeController : BaseController
    {
        private readonly IAdvRepository _advRepository = new AdvRepository();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdvRight1()
        {
            var file = Common.GetPath("~/Content/Json/adv_right1.json");
            var data = System.IO.File.ReadAllText(file);
            var lstAdvRight1 = JsonConvert.DeserializeObject<List<tbl_Advert>>(data);
            TempData["lstAdvRight1"] = lstAdvRight1;
            return View();
        }
        public ActionResult AdvRight2()
        {
            var file = Common.GetPath("~/Content/Json/adv_right2.json");
            var data = System.IO.File.ReadAllText(file);
            var lstAdvRight2 = JsonConvert.DeserializeObject<List<tbl_Advert>>(data); 
            TempData["lstAdvRight2"] = lstAdvRight2;
            return View();
        }
        public ActionResult AdvHome1()
        {
               var  lstAdvHome1 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center1")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
              var  lstAdvHome2 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center1_mobile")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            
            TempData["lstAdvHome1"] = lstAdvHome1;
            TempData["lstAdvHome2"] = lstAdvHome2;
            return View();
        }
        public ActionResult AdvHome2()
        {
         
             var  lstAdvHome1 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center2")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
               var lstAdvHome2 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center2_mobile")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            TempData["lstAdvHome1"] = lstAdvHome1;
            TempData["lstAdvHome2"] = lstAdvHome2;
            return View();
        }
        public ActionResult AdvHome3()
        {
            var lstAdvHome1 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center_left")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();

            var lstAdvHome2 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center_left_mobile")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            TempData["lstAdvHome1"] = lstAdvHome1;
            TempData["lstAdvHome2"] = lstAdvHome2;
            return View();
        }
        public ActionResult AdvHome4()
        {
            var lstAdvHome1 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center_right")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            var lstAdvHome2 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_center_right_mobile")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            TempData["lstAdvHome1"] = lstAdvHome1;
            TempData["lstAdvHome2"] = lstAdvHome2;
            return View();
        }
        public ActionResult AdvHome5()
        {
            var lstAdvHome1 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("qc-chay-doc-trai")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList(); 
            TempData["lstAdvHome1"] = lstAdvHome1; 
            return View();
        }
        public ActionResult AdvHome6()
        {
            var lstAdvHome1 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("qc-chay-doc-phai")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList(); 
            TempData["lstAdvHome1"] = lstAdvHome1; 
            return View();
        }
    }
}
