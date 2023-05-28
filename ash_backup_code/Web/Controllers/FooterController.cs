using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class FooterController : Controller
    {
        //
        // GET: /MenuFooter/
        readonly ICategoryRepository _cateRepository = new CategoryRepository();
        readonly IAdvRepository _advRepository = new AdvRepository();
        readonly IFooterRepository _footerRepository = new FooterRepository();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HomeFooter()
        {
            var social = _advRepository.GetAll().Where(x => x.Position != null && x.Position.Split(',').Contains("associate") && x.Status).OrderBy(x => x.Ordering).ThenByDescending(g=>g.CreatedDate).ToList();
            TempData["Banner"] = social;

            var footer = _footerRepository.GetAll().Where(x => x.Status).First();

            var lstCateFooter = _cateRepository.GetAll().Where(g => g.Position != null);
            if (lstCateFooter != null)
            {
                lstCateFooter = lstCateFooter.Where(g => g.Position.Trim().Contains("footer_menu")).OrderBy(x => x.Ordering).Take(12).ToList();
            }
            TempData["lstCateFooter"] = lstCateFooter;
            return View(footer);
        }
    }
}
