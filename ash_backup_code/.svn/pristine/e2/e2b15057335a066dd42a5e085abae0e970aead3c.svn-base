
using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class eMagazineController : BaseController
    {
        readonly IeMagazineRepository _eMagazineRepository = new eMagazineRepository();
        readonly ISubeMagazineRepository _SubeMagazineRepository = new SubeMagazineRepository();

        public ActionResult Index()
        {
            var lsteMagazine = _eMagazineRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering)
                .ThenByDescending(g => g.CreatedDate).ToList();
            return View(lsteMagazine);
        }
        public ActionResult Detail(int id)
        {
            var objeMagazine = _eMagazineRepository.Find(id);
            var lstSub = _SubeMagazineRepository.GetAll().Where(g => g.IDParent == id).OrderBy(g=>g.Ordering).ToList();
            TempData["lstSub"] = lstSub;
            return View(objeMagazine);
        }
        
    }
}
