using PublicService.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class ConfigNoteController : BaseController
    {
        readonly IConfigNoteRepository RepConfig = new ConfigNoteRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var obj = RepConfig.GetAll().FirstOrDefault();
            return View(obj);
        }
        [Authorize(Roles = "Index")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(tbl_configLuuY obj)
        {
            try
            {
                RepConfig.Edit(obj);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
