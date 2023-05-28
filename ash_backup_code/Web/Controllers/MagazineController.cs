using System; 
using System.Linq; 
using System.Web.Mvc; 
using Web.BaseSecurity; 
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{ 
    public class MagazineController : BaseController
    {
        readonly IFieldFileRep _fielfileRep = new FieldFileRep();
        readonly IMagazineRepository _magazineRepository = new MagazineRepository();
        readonly ILimitRepository _limitRepository = new LimitRepository();
        public ActionResult Index()
        { 
            return View();
        }
        public ActionResult Detail(int id)
        {
            var objMagazine = _magazineRepository.Find(id); 
            return View();
        }

        public ActionResult ListData(string year,string title,int page = 1)
        {
            var langCodeHome = "vn";
            if (Session["langCodeHome"] != null)
                langCodeHome = Session["langCodeHome"].ToString();
            var objfielfile = _fielfileRep.GetData().Where(x => x.Code == "tapchi").ToList();
            var objlimit = _limitRepository.GetAll().FirstOrDefault(g => g.Code.Trim() == "danh-sach-tap-chi");
            var lstMagazine = _magazineRepository.GetAll().Where(g => g.Status&&g.LangCode== langCodeHome).OrderBy(g=>g.Ordering).ThenByDescending(g=>g.CreatedDate).ToList();
            if (Convert.ToInt32(year) > 0)
                lstMagazine = lstMagazine.Where(g => g.Year == Convert.ToInt32(year)).ToList();
            if (title != "")
                lstMagazine = lstMagazine.Where(g => g.Title == title).ToList(); 
            var totalMagazine = lstMagazine.ToList().Count;
            var rowLimit = (objlimit!= null ? objlimit.Value : 12);
            var lstMagazines = lstMagazine.Skip((page - 1) * rowLimit).Take(rowLimit);
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/Magazine/ListData.cshtml", lstMagazines),
                totalPages = Math.Ceiling(((double)totalMagazine / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
