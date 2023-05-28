using System.Collections.Generic;
using System.Linq; 
using System.Web.Mvc;
using Newtonsoft.Json;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class LinkWebsiteController : BaseController
    {
        //
        // GET: /LinkWebsite/
        readonly ILinkWebsiteRepository _linkWebsiteRepository = new LinkWebsiteRepository();
        public ActionResult Index(string type)
        {
            var file = Common.GetPath("~/Content/Json/linkwebsite.json");
            var data = System.IO.File.ReadAllText(file);
            var lstLinkWebsite = JsonConvert.DeserializeObject<List<tbl_LinkWebsite>>(data); 
            return View(lstLinkWebsite);
        }
        public ActionResult HomeLinkWebSite()
        {
            var langCode = "vn";
            if (Session["langCodeHome"] != null)
                langCode = Session["langCodeHome"].ToString();
            var lstLinkWebSite = _linkWebsiteRepository.GetAll().Where(g => g.Status&&g.LangCode==langCode).OrderBy(g=>g.Ordering).ToList();
            TempData["lstLinkWebSite"] = lstLinkWebSite;
            return View();
        }
    }
}
