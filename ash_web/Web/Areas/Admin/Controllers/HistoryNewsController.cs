using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class HistoryNewsController : BaseController
    {
        readonly IqProcedureRepository _qProcedureRepository = new qProcedureRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly INewsHistoryRepository _historyRepository = new NewsHistoryRepository();
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        readonly ICategoryRepository _categoryRepository = new CategoryRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        readonly IqProcedureRepository _quyTrinhXuatBanRepository = new qProcedureRepository();
        //
        // GET: /Admin/HistoryNews/
        [Authorize(Roles = "Index")]
        public ActionResult Index(int id)
        {
            TempData["NewsID"] = id;
            TempData["ID"] = id;
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(int page = 1)
        {
            var lstqProcedure = _qProcedureRepository.GetAll().ToList();
            var qProcedureID = lstqProcedure.Where(x => x.isPublish == true).Select(x => x.ID).FirstOrDefault();
            TempData["lstqProcedure"] = lstqProcedure;
            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;
            var lstQtxb = _qProcedureRepository.GetAll();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();

            var id = (int)TempData["NewsID"];
            var objNews = _newsRepository.Find(id);
            TempData["News"] = objNews;
            var lstNewsVersion = new List<tbl_NewsVersion>();
            var history = _historyRepository.GetAll().FirstOrDefault(g => g.NewsID == id);
            if (history != null)
            {
                lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(history.Contents);
            }
            lstNewsVersion = lstNewsVersion.OrderByDescending(g => g.LastModifieDate).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/HistoryNews/_ListData.cshtml", lstNewsVersion),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Index")]
        public ActionResult ViewVersionNews(int newsid, int version)
        {
            var objHistory = _historyRepository.GetAll().FirstOrDefault(x=>x.NewsID== newsid);
            if (objHistory != null)
            {
                var lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(objHistory.Contents);
                var objNewsVersion = lstNewsVersion.FirstOrDefault(g => g.Version == version);
                if (objNewsVersion != null)
                {
                    var lstPageElement = _pageElementRepository.GetAll();
                    TempData["PageElements"] = lstPageElement;
                    var lstCatgory = _categoryRepository.GetAll();
                    TempData["Category"] = lstCatgory;
                    var lstLanguages = _languagesRepository.GetAll();
                    TempData["Languages"] = lstLanguages;
                    TempData["OldVersion"] = lstNewsVersion.FirstOrDefault(g => g.Version == version - 1);
                    return Json(RenderViewToString("~/Areas/Admin/Views/HistoryNews/ViewVersionNews.cshtml", objNewsVersion), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Edit")]
        public ActionResult Restore(int newsid, int version)
        {
            var objHistory = _historyRepository.GetAll().FirstOrDefault(g => g.NewsID == newsid);
            if (objHistory != null)
            {
                var lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(objHistory.Contents);
                var objNewsVersion = lstNewsVersion.FirstOrDefault(g => g.Version == version);
                if (objNewsVersion != null)
                {
                    try
                    {
                        var objNews = new tbl_News();
                        Common.CopyDataObj2Obj(objNewsVersion, objNews);
                        var objQtxb = _quyTrinhXuatBanRepository.GetAll().OrderBy(g => g.Ordering).FirstOrDefault();
                        objNews.Status = objQtxb.ID;
                        _newsRepository.Edit(objNews);
                        return Json(new
                        {
                            IsSuccess = true,
                            Messenger = "Khôi phục thành công",
                        }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {

                        return Json(new
                        {
                            IsSuccess = false,
                            Messenger = "Khôi phục thất bại",
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Không tìm thấy phiên bản",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectVersion(int id)
        {
            var objHistory = _historyRepository.GetAll().FirstOrDefault(x => x.NewsID == id);
            if (objHistory != null)
            {
                TempData["id"] = id;
                var lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(objHistory.Contents);
                return Json(RenderViewToString("~/Areas/Admin/Views/HistoryNews/SelectVersion.cshtml", lstNewsVersion), JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Viewchange(int id, int oldId, int newId)
        {
            var objHistory = _historyRepository.GetAll().FirstOrDefault(x => x.NewsID == id);
            if (objHistory != null)
            {
                var lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(objHistory.Contents);
                var objNewsVersionOld = lstNewsVersion.FirstOrDefault(g => g.Version == oldId);
                var objNewsVersionNew = lstNewsVersion.FirstOrDefault(g => g.Version == newId);
                if (objNewsVersionOld != null && objNewsVersionNew != null)
                {
                    TempData["OldVersion"] = objNewsVersionOld;
                    TempData["NewVersion"] = objNewsVersionNew;
                    return Json(RenderViewToString("~/Areas/Admin/Views/HistoryNews/ViewChange.cshtml"), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
