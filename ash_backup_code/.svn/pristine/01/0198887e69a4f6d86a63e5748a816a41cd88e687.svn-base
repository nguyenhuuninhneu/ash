using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Web.Areas.Admin.Controllers;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class ProcessController : BaseController
    {
        //
        // GET: /Admin/O/
        readonly INewsRepository _newRepository = new NewsRepository();
        readonly IqProcedureRepository _qProcedureRepository = new qProcedureRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IImageCategoryRepository _imgCateRepository = new ImageCategoryRepository();
        readonly ICategoryRepository _cateRepository = new CategoryRepository();
        readonly IProcessRepository _processRepository = new ProcessRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult ViewProcess(int FkId, string Code)
        {
            var objNews = _newRepository.Find(FkId);
            var lstCategory = _cateRepository.GetAll().ToList();
            var lstProcedure = _qProcedureRepository.GetAll().ToList();
            TempData["lstProcedure"] = lstProcedure;
            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            TempData["FkId"] = FkId;
            TempData["objNews"] = objNews;
            TempData["lstCategory"] = lstCategory;
            var objProcess = _processRepository.GetAll().Where(x=>x.FKId== FkId).ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/Process/_ViewProcess.cshtml", objProcess), JsonRequestBehavior.AllowGet); 
        }
        [HttpPost]
        public ActionResult ProcessTransfer(int FkId, string Code)
        {
            var lstCategory = _cateRepository.GetAll().ToList();
            var objNews = _newRepository.Find(FkId);
            var status = Convert.ToInt32(objNews.Status);
            var currentQtxb = _qProcedureRepository.Find(status);
            var lstQtxb = _qProcedureRepository.GetAll().OrderBy(g => g.Ordering);
            var objQtxb = lstQtxb.Where(g => g.Ordering > currentQtxb.Ordering).OrderBy(g => g.Ordering).FirstOrDefault();
            TempData["objQtxb"] = objQtxb;
            var Status = _qProcedureRepository.GetAll().Where(g => g.ID != status).ToList();
            TempData["Status"] = Status.OrderBy(g => g.Ordering).ToList();
            TempData["lstCategory"] = lstCategory;
            TempData["FkId"] = FkId;
            TempData["Code"] = Code;
            TempData["objNews"] = objNews;
            return View();
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add(int FkId,string Code, string Return = "")
        {
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();

            var lstCate = _cateRepository.GetAll();
            var objNews = _newRepository.Find(FkId);
            if (objNews.IsPublish)
            {
                objNews.IsPublish = false;
                _newRepository.Edit(objNews);
            }
            var status = Convert.ToInt32(objNews.Status);
            TempData["statusOld"] = status;  

            var lstProcess = _processRepository.GetAll().Where(g => g.FKId == FkId).Select(g=>g.FromProcedure).ToList(); 
            var objxuatban = _qProcedureRepository.GetAll().FirstOrDefault(g => g.Code == "xuatban"); 

            var Status = _qProcedureRepository.GetAll().Where(g => g.ID != status && g.LangCode == langCode).ToList();
            if (Return == "1")
            {
                var objProcedure = _qProcedureRepository.Find(Convert.ToInt32(status));
                Status = Status.Where(g => objProcedure != null && g.Ordering < objProcedure.Ordering).ToList();
            }
            else
            {
                if (lstProcess != null && lstProcess.Count > 0 && objxuatban != null && objNews.Status == objxuatban.ID)
                {
                    Status = Status.Where(g => lstProcess.Contains(g.ID)).ToList();
                }
            }
            TempData["Status"] = Status;

            TempData["FkId"] = FkId;
            TempData["Code"] = Code;
            TempData["objNews"] = objNews;
            TempData["lstCate"] = lstCate;
           return Json(RenderViewToString("~/Areas/Admin/Views/Process/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(qProcess obj)
        { 
            try
            { 
                var objNews = _newRepository.Find(obj.FKId); 
                obj.FromProcedure = Convert.ToInt32(objNews.Status);
                var status = Request["Status"];
                var statusOld = Request["statusOld"];
                var objkhoitao = _qProcedureRepository.GetAll().FirstOrDefault(g => g.Code == "khoitao");

                var objProcedureOld = _qProcedureRepository.Find(Convert.ToInt32(statusOld));
                var objProcedureNew = _qProcedureRepository.Find(Convert.ToInt32(status));

                switch (obj.TableCode)
                {
                    case "news":
                        
                        objNews.UserActId = User.ID;
                        objNews.ProcedureId = Convert.ToInt32(status);
                        objNews.Status =Convert.ToInt32(status);
                        objNews.IdUserChoose = null;
                        objNews.IsPublish = false;
                        if (objkhoitao != null && Convert.ToInt32(status) == objkhoitao.ID)
                            objNews.IdUserChoose = objNews.CreatedBy;
                        if (objProcedureOld != null && objProcedureNew != null && objProcedureOld.Ordering > objProcedureNew.Ordering) objNews.IsTraLai = true;
                        else objNews.IsTraLai = false;

                        _newRepository.Edit(objNews);

                        var newscol = new NewsController();
                        newscol.GenNewsJson ();

                        break;
                }
                obj.FromId = User.ID;
                obj.CreateDate = DateTime.Now;
                obj.ToProcedure= Convert.ToInt32(status);
                _processRepository.Add(obj);
                LogController.AddLog(string.Format("Chuyển xử lý: {0}", obj.FKId), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Chuyển xử lý thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Chuyển xử lý thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _processRepository.Find(id);
                _processRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa xử lý: {0}", obj.FKId), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa xử lý thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa xử lý thành công",
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
