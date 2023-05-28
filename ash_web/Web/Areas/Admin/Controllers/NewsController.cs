using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;
using Microsoft.Ajax.Utilities;
using System.Drawing;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Web.Core;

namespace Web.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        readonly IProcessRepository _processRepository = new ProcessRepository();
        readonly IPositionRepository _positionRepository = new PositionRepository();
        readonly IConfigRepository _configRepository = new ConfigRepository();
        readonly ICategoryRepository _cateRepository = new CategoryRepository();
        readonly IHomeMenuRepository _menuRepository = new HomeMenuRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        readonly INewsHistoryRepository _newsHistoryRepository = new NewsHistoryRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IqProcedureRepository _qProcedureRepository = new qProcedureRepository();
        readonly IGroupUserRepository _groupuserRepository = new GroupUserRepository();
        public List<tbl_HomeMenu> lstCategory = new List<tbl_HomeMenu>();
        readonly IAdminMenuRepository _adminMenuRepository = new AdminMenuRepository();
        readonly IUserAdminRepository _repUserAdmin = new UserAdminRepository();
        public ActionResult Index(int isPublic = 0, int TraLai = 0, string Type = "")
        { 
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var listPosByCode = _positionRepository.GetListPosByCode("new");
            if (listPosByCode == null)
            {
                listPosByCode = new List<tbl_Position>();
            }
            TempData["LstPos"] = listPosByCode;

            var lstqProcedure = _qProcedureRepository.GetAll().FirstOrDefault();
            var qProcedureFirst = 0;
            if (lstqProcedure != null)
            {
                qProcedureFirst = lstqProcedure.ID;
            }
            TempData["qProcedureFirst"] = qProcedureFirst;
            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;

            var lstCate = Common.CreateLevel(_cateRepository.GetAll().OrderBy(g => g.Ordering).Where(g => g.Active && g.LangCode==langCode).ToList());
            TempData["CategoryNews"] = lstCate;
             
            var curl = HttpContext.Request.Url.AbsolutePath;
            var adminMenuID = _adminMenuRepository.GetAll().FirstOrDefault(g => g.Url.Trim() == curl);
          
            if (adminMenuID != null)
            {
                var act = new GetAction();
                var action = act.Get(User.GroupUser, adminMenuID.ID, User.isAdmin);
                TempData["action"] = action.ToList();
            }

            var lstUserAdmin = _userAdminRepository.GetAll().Where(x => x.ID != User.ID&&x.Active).ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;

            var lstQtxb = _qProcedureRepository.GetAll().OrderBy(g => g.Ordering).ToList();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();

            lstCategory = GetPermisCatNews().ToList();
            TempData["Category"] = Common.CreateLevel(lstCategory);
            
            var Status = _qProcedureRepository.GetAll().ToList();
            TempData["Status"] = Status.OrderBy(g => g.Ordering).ToList();

            TempData["IsPublish"] = isPublic; 
            TempData["TraLai"] = TraLai; 
            TempData["Type"] = Type; 

            return View();
        }
         
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string keySearch, int Status = 0, string Position = "", string categoryId = "", int page = 1, int CreatedBy = 0, string tuNgay = null, string denNgay = null, int luaChon = 1,int TraLai = 0, int IsPublish = 0,string Type = "")
        {
            // luachon: 1 - Title, 2 - Details, 3 - Author, 4-SourceNews
            var langcode = "vn";
            if (Session["langCodeDefaut"] != null)
                langcode = Session["langCodeDefaut"].ToString();

            var lstUserAdmin = _userAdminRepository.GetAll().Where(x => x.Active).ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            var lstqProcedure = _qProcedureRepository.GetAll();
            var qProcedureFirst = lstqProcedure.FirstOrDefault(g=>g.Code == "khoitao");
            TempData["qProcedureFirst"] = (qProcedureFirst != null ? qProcedureFirst.ID : 0);

            var allCat = _cateRepository.GetAll();
            var lstCategory = Common.CreateLevel(allCat.OrderBy(g => g.Ordering).ToList());
            TempData["Category"] = lstCategory;

            var allProcess = _processRepository.GetAll().ToList();

            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser; 

            #region Get list news 
            // phan trang store procedure 
            var lstNews = new List<tbl_News_Custom>();
            var pagesize = 20; // limit
            var condition = " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(Position))
            {
                condition += " And Position = '" + Position + "'";
            }
            if (Status > 0)
            {
                condition += " And Status = " + Status;
            }
            if (!string.IsNullOrEmpty(keySearch))
            {
                var fieldSearch = "Title";
                if (luaChon == 2)
                    fieldSearch = "Details";
                if (luaChon == 3)
                    fieldSearch = "Author";
                if (luaChon == 4)
                    fieldSearch = "SourceNews";
                condition += " And (LOWER(" + fieldSearch + ") like N^*^" + keySearch.Trim().ToLower() + "^-^ OR LOWER(" + fieldSearch + ") like N^*^" + HelperString.UnsignCharacter(keySearch.Trim().ToLower()) + "^-^)";
            }
            if (objUser.isAdmin == false)
            {
                if (!string.IsNullOrEmpty(objUser.QuyTrinhXuatBanID))
                {
                    var arrQuyTrinhXuatBanID = objUser.QuyTrinhXuatBanID.Split(',');
                    TempData["permQuyTrinhXuatBanID"] = arrQuyTrinhXuatBanID.ToList();
                    if (arrQuyTrinhXuatBanID.Contains(qProcedureFirst.ToString()))
                    {
                        condition += " And CreatedBy = " + User.ID;
                    }
                    else
                    {
                        var lst = new List<tbl_News_Custom>();
                        List<int> idProcess = allProcess.Where(x => arrQuyTrinhXuatBanID.Contains(x.ToProcedure.ToString())).Select(x => x.FKId).Distinct().DefaultIfEmpty().ToList();
                        foreach (var item in idProcess)
                        {
                            var objNews = lstNews.Where(x => x.ID == item || x.CreatedBy == User.ID || x.IsPublish).ToList();
                            lst.AddRange(objNews);
                        }
                        lstNews = lst.DistinctBy(x => x.ID).OrderByDescending(x => x.CreatedDate).ToList();
                    }

                }
            }
            if (CreatedBy > 0)
            {
                condition += " And CreatedBy = " + CreatedBy;
            }

            var lstloai = _configRepository.GetAll().Where(g => g.Code == Type && g.Status).Select(g=>g.ID).ToList();
            var strloai = string.Join(",", lstloai);
            if (lstloai != null && lstloai.Count > 0)
            { 
                condition += " AND NewMoney IN ('" + strloai.Replace(",", "','") + "')";
            }

            if (!string.IsNullOrEmpty(tuNgay) && !string.IsNullOrEmpty(denNgay))
            {
                var ngaydang = HelperDateTime.ConvertDate(tuNgay);
                var ngayket = HelperDateTime.ConvertDate(denNgay);
                if (ngaydang > ngayket)
                {
                    return Json(new { IsSuccess = false, Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" }, JsonRequestBehavior.AllowGet);
                }
            }
            if (!string.IsNullOrEmpty(tuNgay))
            {
                var ngaydang = HelperDateTime.ConvertDate(tuNgay + " 00:00");
                condition += " And CreatedDate >= '" + ngaydang + "'";
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                var ngayket = HelperDateTime.ConvertDate(denNgay + " 23:59");
                condition += " And CreatedDate <= '" + ngayket + "'";
            }


            condition += " And IsPublish = " + IsPublish;
            condition += " And IsTraLai = " + TraLai;

            var fullCatIdStr = "";
            if (!string.IsNullOrEmpty(categoryId))
            {
                var fullCatId = Common.GetChild(allCat, Convert.ToInt32(categoryId));
                fullCatIdStr = string.Join(",", fullCatId);
                //condition += " AND CategoryIdStr IN ('" + fullCatIdStr.Replace(",", "','") + "')";
                condition += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
            }
            var orderby = "ORDER BY CreatedDate DESC";
           
            var DicLevel = _newsRepository.spGetListNews(page, pagesize, "*","view_News_MultiCate", condition, orderby);
            var totalNews = 0;
            if (DicLevel.Count > 0)
            {
                var objDic = DicLevel.FirstOrDefault();
                lstNews = objDic.Key;
                totalNews = Convert.ToInt32(objDic.Value);
            }
            //var AllNews = _newsRepository.spGetListNews(page, totalNews, "*", condition, orderby);
            Session["lstNews"] = lstNews;
            #endregion 
            TempData["lstProcess"] = allProcess;
            var lstConfig = _configRepository.GetAll().OrderBy(g => g.Ordering).Where(x => x.Status && x.LangCode == langcode).ToList();
            TempData["lstConfig"] = lstConfig.ToList();

            TempData["lstNews"] = lstNews.ToList();
            TempData.Keep();
            TempData["totalNews"] = totalNews;
            //Quy trình xuất bản tin 
            TempData["QuyTrinhXuatBan"] = lstqProcedure.ToList(); 
            TempData["QuyTrinhXuatBanID"] = objUser.QuyTrinhXuatBanID;
            TempData["Type"] = Type;


            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/_ListData.cshtml", lstNews),
                totalPages = Math.Ceiling(((double)totalNews / pagesize)),
            }, JsonRequestBehavior.AllowGet);
        }
         
        public ActionResult Detail(int id)
        {
            var newsDetail = _newsRepository.Find(id); 
            var lstRelated = new List<tbl_News_Custom>();
            if (newsDetail.RelatedNews != null)
            {
                var condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
                condition += " AND ID IN ('" + newsDetail.RelatedNews.Replace(",", "','") + "')";
                var orderby = "ORDER BY CreatedDate DESC";
                var field = "ID,Title,Photo,Description,CategoryIdStr,CreatedDate,IsPublish,CateComma";
                var DicLevel = _newsRepository.spGetListNews(1, 100, field, "view_News_MultiCate", condition, orderby);
                if (DicLevel.Count > 0)
                {
                    var objDic = DicLevel.FirstOrDefault();
                    lstRelated = objDic.Key;
                }
            } 
            TempData["lstRelated"] = lstRelated; 
            return Json(RenderViewToString("~/Areas/Admin/Views/News/Detail.cshtml", newsDetail), JsonRequestBehavior.AllowGet);
        }
      

        #region ProcessNews
        public ActionResult ChangePublic(int id, int GenJson = 1)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            var obj = new qProcess();
            var objNew = _newsRepository.Find(id);
            if (objNew.IsPublish)
            {
                obj.Contents = "Hủy xuất bản";
            }
            else
            {
                obj.Contents = "Xuất bản tin";
            }
            ChangeProcess(id, "news", obj);

            objNew.IsPublish = !objNew.IsPublish; 
            var lstQtxb = _qProcedureRepository.GetAll().OrderBy(g => g.Ordering); 
            var qtxbID = lstQtxb.FirstOrDefault(g => g.isPublish).ID;
            objNew.Status = qtxbID;
            objNew.IdUserChoose = null;
            if (objUser.isAdmin)
            {
                if (objNew.IsPublish)
                {
                    objNew.IdUserChoose = null;
                }
                _newsRepository.Edit(objNew);
                obj.ToProcedure = qtxbID;
                _processRepository.Edit(obj);
                if(GenJson == 1)
                    GenNewsJson();
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thay đổi trạng thái thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (objUser.QuyTrinhXuatBanID.Split(',').Contains(qtxbID.ToString()) || objUser.isAdmin)
                {
                    if (objNew.IsPublish)
                    {
                        objNew.IdUserChoose = null;
                    }
                    _newsRepository.Edit(objNew);
                    obj.ToProcedure = qtxbID;
                    _processRepository.Edit(obj);
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Thay đổi trạng thái thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Bạn không có quyền thay đổi",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [Authorize(Roles = "Approved")]
        [HttpPost]
        public ActionResult Approved(int id)
        {
            var objNews = _newsRepository.Find(id);
            try
            {
                var obj = new qProcess();
                ChangeProcess(id, "new", obj);
                //Lưu lịch sử bài viết
               
                //var newsHistory = _newsHistoryRepository.GetAll().FirstOrDefault(g => g.NewsID == id);
                //var lstNewsVersion = new List<tbl_NewsVersion>();
                //var objNewsVersion = new tbl_NewsVersion();
                //Common.CopyDataObj2Obj(objNews, objNewsVersion);
                //objNewsVersion.LastModifieDate = DateTime.Now;
                //objNewsVersion.LastModifieBy = User.Username;
                //if (newsHistory != null)
                //{
                //    lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(newsHistory.Contents);
                //    objNewsVersion.Version = lstNewsVersion.Count + 1;
                //    lstNewsVersion.Add(objNewsVersion);
                //    newsHistory.Contents = HelperXml.SerializeList2Xml(lstNewsVersion);
                //    _newsHistoryRepository.Edit(newsHistory);
                //}
                //else
                //{
                //    objNewsVersion.Version = 1;
                //    lstNewsVersion.Add(objNewsVersion);
                //    newsHistory = new tbl_HistoryNews
                //    {
                //        NewsID = id,
                //        Contents = HelperXml.SerializeList2Xml(lstNewsVersion)
                //    };
                //    _newsHistoryRepository.Add(newsHistory);
                //}
               

                var currentQtxb = _qProcedureRepository.Find(Convert.ToInt32(objNews.Status));
                var lstQtxb = _qProcedureRepository.GetAll().OrderBy(g => g.Ordering);
                var objQtxb = lstQtxb.Where(g => g.Ordering > currentQtxb.Ordering).OrderBy(g => g.Ordering).FirstOrDefault();
                objNews.IdUserChoose = null;
                if (objQtxb != null) objNews.Status = objQtxb.ID;
                objNews.IsTraLai = false;
                _newsRepository.Edit(objNews);
                obj.ToProcedure = objQtxb.ID;
                _processRepository.Edit(obj);
                // Luu log
                LogController.AddLog(string.Format("Duyệt bài viết: {0} trạng thái: {1} ---> {2}", objNews.Title, currentQtxb.Name, objQtxb.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = string.Format("Duyệt bài thành công")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Duyệt bài thất bại")
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "Approved")]
        [HttpPost]
        public ActionResult ApprovedXB(int id)
        {
            var objNews = _newsRepository.Find(id);
            try
            {
                objNews.IdUserChoose = null;
                objNews.IsXBDuyet = true;
                _newsRepository.Edit(objNews);
                LogController.AddLog(string.Format("Duyệt bài viết: {0} trạng thái: {1} ---> {2}", objNews.Title, "Xuất bản", "Xuất bản"), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = string.Format("Duyệt bài thành công")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Duyệt bài thất bại")
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Approved")]
        [HttpPost]
        public ActionResult ApprovedDown(int id)
        {
            var objNews = _newsRepository.Find(id);
            try
            {
                //Lưu version bài viết
                var newsHistory = _newsHistoryRepository.GetAll().FirstOrDefault(g => g.NewsID == id);
                var lstNewsVersion = new List<tbl_NewsVersion>();
                var objNewsVersion = new tbl_NewsVersion();
                Common.CopyDataObj2Obj(objNews, objNewsVersion);
                objNewsVersion.LastModifieDate = DateTime.Now;
                objNewsVersion.LastModifieBy = User.Username;
                if (newsHistory != null)
                {
                    lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(newsHistory.Contents);
                    objNewsVersion.Version = lstNewsVersion.Count + 1;
                    lstNewsVersion.Add(objNewsVersion);
                    newsHistory.Contents = HelperXml.SerializeList2Xml(lstNewsVersion);
                    _newsHistoryRepository.Edit(newsHistory);
                }
                else
                {
                    objNewsVersion.Version = 1;
                    lstNewsVersion.Add(objNewsVersion);
                    newsHistory = new tbl_HistoryNews
                    {
                        NewsID = id,
                        Contents = HelperXml.SerializeList2Xml(lstNewsVersion)
                    };
                    _newsHistoryRepository.Add(newsHistory);
                }
                //
                var currentQtxb = _qProcedureRepository.Find(Convert.ToInt32(objNews.Status));
                var lstQtxb = _qProcedureRepository.GetAll().OrderBy(g => g.Ordering);
                var objQtxb = lstQtxb.Where(g => g.Ordering < currentQtxb.Ordering).OrderByDescending(g => g.Ordering).FirstOrDefault();
                if (objQtxb != null) objNews.Status = objQtxb.ID;
                _newsRepository.Edit(objNews);
                // Luu log
                LogController.AddLog(string.Format("Duyệt bài viết: {0} trạng thái: {1} ---> {2}", objNews.Title, currentQtxb.Name, objQtxb.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = string.Format("Duyệt bài thành công")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Duyệt bài thất bại")
                }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Add News

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            //ngôn ngữ
            var langcode = "vn";
            if (Session["langCodeDefaut"] != null)
                langcode = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langcode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            var lstCTV = _userAdminRepository.GetAll().Where(g => g.IsCTV == true).ToList();
            TempData["lstCTV"] = lstCTV;

            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;
            var Status = _qProcedureRepository.GetAll().ToList();
            if (User.isAdmin == false)
            {
                if (!string.IsNullOrEmpty(objUser.QuyTrinhXuatBanID))
                {
                    var arrQuyTrinhXuatBanID = objUser.QuyTrinhXuatBanID.Split(',');
                    Status = Status.Where(g => arrQuyTrinhXuatBanID.Contains(g.ID.ToString())).ToList();
                }
            }
            var Type = Request.QueryString["Type"];
            TempData["Type"] = Type;
            var lstQtxb = _qProcedureRepository.GetAll();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();
            TempData["Status"] = Status.OrderBy(g => g.Ordering).ToList();
            //vị trí
              var  listPosByCode = _positionRepository.GetListPosByCode("new");
            TempData["LstPos"] = listPosByCode;

            var lstConfig = _configRepository.GetAll().OrderBy(g => g.Ordering).Where(x => x.Status && x.LangCode == langcode&&!string.IsNullOrEmpty(x.Code)).ToList();
            if (!string.IsNullOrEmpty(Type))
            {
                lstConfig = lstConfig.Where(g =>g.Code!=null&& g.Code.Trim().ToLower() == Type.Trim().ToLower()).ToList();
            }
            TempData["Config"] = lstConfig;
            var lstCategory = Common.CreateLevel(_cateRepository.GetAll().Where(g=>g.LangCode == langcode && g.Active).ToList(),"Name");
            TempData["Category"] = lstCategory.ToList();
            TempData["ChuyenTrang"] = Common.CreateLevel(_menuRepository.GetAll().ToList());
            var IsPublic = Request.QueryString["isPublic"];
            TempData["IsPublic"] = IsPublic;
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_News obj, string isContinue, string isPublish, string isDuyet, double x1 = 0, double y1 = 0, double h = 0, double w = 0)
        {
            var type = Request["TypeHidden"];
            var IsPublicHidden = Request["IsPublicHidden"];
            var objNew = new List<tbl_News_Custom>();
            var condition = " WHERE Title = '"+ obj.Title.Trim()+"'";
            var orderby = "ORDER BY CreatedDate DESC";
            var DicLevel = _newsRepository.spGetListNews(1, 2, "ID", "view_News_MultiCate", condition, orderby);
            if (DicLevel.Count > 0)
            {
                var objDic = DicLevel.FirstOrDefault();
                objNew = objDic.Key;
            }
            if (objNew.Count >= 1)
            { 
                return Redirect("/Admin/News?sc=3&&isPublic=" + isPublish + "&Type=" + type);
            }
            var category = Request["CategoryIdStr"];
            var relatedNews = Request["RelatedNews"];
            var position = Request["position"];
            var nhuanbut = Request["NhuanBut"];
            obj.NhuanBut = Convert.ToInt32(RemoveChar(nhuanbut));
            obj.CategoryIdStr = category;
            obj.Position = position;
            obj.RelatedNews = relatedNews;
            obj.CreatedBy = User.ID;
            obj.IdUserChoose = User.ID;
            obj.CreatedDate = DateTime.Now;
            obj.TimeChoose = DateTime.Now; 
            var date = Request["EditDate"];
            if (!string.IsNullOrEmpty(date))
            {
                obj.CreatedDate = HelperDateTime.ConvertDate(date);
            }
            try
            { 
                //Lay tin sp
                obj.Title = obj.Title.Trim();

                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode;
                obj.IsDelete = 0; 

                _newsRepository.Add(obj);
                SaveHistory(obj);
                if (isPublish == "1")
                {
                    obj.IsPublish = true;
                    obj.IdUserChoose = null;
                    _newsRepository.Edit(obj);
                    GenNewsJson();
                }
                var objProcess = new qProcess();
                objProcess.Contents = "Bài viết mới";
                ChangeProcess(obj.ID, "new", objProcess);
                if (isDuyet == "1")
                {
                    Approved(obj.ID);
                }else if (isDuyet == "0")
                {
                    ApprovedXB(obj.ID);
                }
                LogController.AddLog(string.Format("Thêm bài viết: {0}", obj.Title), User.Username);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            if (isContinue == "1")
            {

                return Redirect("/Admin/News/Add?sc=4&&isPublic="+ IsPublicHidden+ "&Type="+ type);
            }
            if(string.IsNullOrEmpty(IsPublicHidden)&&(string.IsNullOrEmpty(type)))
            {
                if (obj.IsPublish)
                    IsPublicHidden = "1";
                else
                    IsPublicHidden = "0";
                if(obj.NewMoney.HasValue)
                {
                    var objConfig = _configRepository.Find(obj.NewMoney.Value);
                    if (objConfig != null&&!string.IsNullOrEmpty(objConfig.Code))
                    {
                        type = objConfig.Code;
                    }
                        
                }
                return Redirect("/Admin/News?sc=4&&isPublic=" + IsPublicHidden + "&Type=" + type);
            }
            return Redirect("/Admin/News?isPublic=" + (isPublish == "1" ? 1 : 0)+ "&Type="+type);
        }
        #endregion

        #region Edit News
        [Authorize]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id,int isPublic)
        {
            var type = Request.QueryString["Type"];
            TempData["Type"] = type;
            var lstNews = _newsRepository.GetAll_View().Where(x => x.IsPublish).ToList();
            TempData["lstNews"] = lstNews;
            var listPosByCode = _positionRepository.GetListPosByCode("new");
            TempData["LstPos"] = listPosByCode;

            var lstCTV = _userAdminRepository.GetAll().Where(g => g.IsCTV == true).ToList();

            TempData["lstCTV"] = lstCTV;

            TempData["ChuyenTrang"] = Common.CreateLevel(_menuRepository.GetAll().ToList());
            
            var news = _newsRepository.Find(id); 
            var lstConfig = _configRepository.GetAll().OrderBy(g => g.Ordering).Where(x => x.Status).ToList();
            if (!string.IsNullOrEmpty(type))
            {
                lstConfig = lstConfig.Where(g =>g.Code!=null&& g.Code.Trim().ToLower() == type.Trim().ToLower()).ToList();
            }
            TempData["Config"] = lstConfig.ToList();

            var lstCate = Common.CreateLevel(_cateRepository.GetAll().Where(g => g.Active).ToList(), "Name");
            TempData["Category"] = lstCate.ToList();

            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;
            TempData["news"] = news;
            //var lang = news.LangCode;
            lstCategory = GetPermisCatNews();
            ViewBag.categories = Common.CreateLevel(lstCategory);
            var lstQtxb = _qProcedureRepository.GetAll();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();
            var Status = _qProcedureRepository.GetAll().ToList();
            if (User.isAdmin == false)
            {
                if (!string.IsNullOrEmpty(objUser.QuyTrinhXuatBanID))
                {
                    var arrQuyTrinhXuatBanID = objUser.QuyTrinhXuatBanID.Split(',');
                    Status = Status.Where(g => arrQuyTrinhXuatBanID.Contains(g.ID.ToString())).ToList();
                }
            }
            TempData["Status"] = Status.OrderBy(g => g.Ordering).ToList();
            TempData["isPublic"] = isPublic;
            // lay json relatenews để giữ khi edit
            if (!string.IsNullOrEmpty(news.RelatedNews))
            {
                var orderby = " ORDER BY CreatedDate DESC ";
                var page = 1;
                var pagesize = 100; // limit
                var condition = " WHERE Id IN (" + news.RelatedNews + ") ";
                var field = "ID,Title,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney";
                var DicLevel = _newsRepository.spGetListNews(page, pagesize, field, "view_News_MultiCate", condition, orderby);
                var lstRelateNews = new List<Select2Result>();
                if (DicLevel.Count > 0)
                {
                    var objDic = DicLevel.FirstOrDefault();
                    var lstRelateNewsKey = objDic.Key;
                    if (lstRelateNewsKey != null)
                    {
                        foreach (var item in lstRelateNewsKey)
                        {
                            var objRelateNews = new Select2Result();
                            objRelateNews.id = item.ID;
                            objRelateNews.text = item.Title;
                            lstRelateNews.Add(objRelateNews);
                        }
                    }
                }
                var jsonRelateNews = JsonConvert.SerializeObject(lstRelateNews);
                ViewBag.jsonRelateNews = jsonRelateNews;
            }
            return View(news);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(tbl_News obj, string Publish, string isDuyet,string isContinue)
        {
            var type = Request["Type"];
            var IsPublish = Request["isPublicHiiden"];
            var objNews = _newsRepository.Find(obj.ID);
            var status_process = Request["Status_Process"];
            if (!string.IsNullOrEmpty(status_process))
            {
                qProcess objProcess = new qProcess();
                var attachfile = Request["Attachment_File"];
                var content = Request["Contents_Process"];
                objProcess.Attachment = attachfile;
                objProcess.TableCode = Request["Code"];
                objProcess.CreateDate = DateTime.Now;
                objProcess.Contents = content;
                objProcess.FromProcedure = Convert.ToInt32(obj.Status);
                objProcess.ToProcedure = Convert.ToInt32(status_process);
                objProcess.FKId = obj.ID;
                objProcess.FromId = User.ID;
                _processRepository.Add(objProcess); 
                obj.Status = Convert.ToInt32(status_process);
                obj.UserActId = User.ID;
                obj.ProcedureId = Convert.ToInt32(status_process);
                obj.IdUserChoose = null;
            }
            if(obj.CreatedBy != User.ID&&!User.isAdmin)
            {
                obj.IdUserChoose = null;
            }
           
            obj.ModifyDate = DateTime.Now;
            obj.ModifyBy = User.ID;
            var category = Request["CategoryIdStr"];
            var relatedNews = Request["RelatedNews"];
            var position = Request["position"];
            var nhuanbut = Request["NhuanBut"];
            obj.NhuanBut = Convert.ToInt32(RemoveChar(nhuanbut));
            var date = Request["EditDate"];
            if (!string.IsNullOrEmpty(date))
            {
                obj.CreatedDate = HelperDateTime.ConvertDate(date);
            }
            obj.Position = position;
            obj.RelatedNews = relatedNews;
            obj.CategoryIdStr = category;
            try
            {
                if (
                     objNews.CategoryIdStr != obj.CategoryIdStr ||
                     objNews.Title != obj.Title ||
                     objNews.Photo != obj.Photo ||
                     objNews.SubTitle != obj.SubTitle ||
                     objNews.Description != obj.Description ||
                     objNews.Details != obj.Details ||
                     objNews.Attachment != obj.Attachment ||
                     objNews.Author != obj.Author ||
                     objNews.SourceNews != obj.SourceNews ||
                     objNews.Tags != obj.Tags ||
                     objNews.NewMoney != obj.NewMoney ||
                     objNews.NhuanBut !=obj.NhuanBut ||
                     objNews.RelatedNews != obj.RelatedNews ||
                     objNews.Position != obj.Position ||
                     objNews.CreatedDate != obj.CreatedDate ||
                     objNews.Status !=obj.Status
                   )
                {
                    SaveHistory(obj);
                }
                _newsRepository.Edit(obj);

                if (Publish == "1")
                {
                    ChangePublic(obj.ID, 0);
                    //SaveHistory(obj);
                }
                if (isDuyet == "1")
                {
                    Approved(obj.ID);
                }
                GenNewsJson();
                LogController.AddLog(string.Format("Sửa bài viết: {0}", obj.Title), User.Username);
            }
            catch (Exception ex)
            {

            }
            if (isContinue == "1")
            {
                return Redirect("/Admin/News/Edit?id="+ obj.ID +"&sc=" + 2 + "&isPublic=" + IsPublish + "&Type="+type);
            }
          return Redirect("/Admin/News?isPublic=" + (Publish == "1" ? 1 : 0) + "&Type=" + type);
          //  return Redirect("/Admin/News?sc=" + 2 + "&isPublic=" + IsPublish + "&Type="+type);
        }
        #endregion

        #region Delete News

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var objPost = _newsRepository.Find(id);
                var objQuyTrinhXuatBan = _qProcedureRepository.GetAll().FirstOrDefault(g => g.isPublish == true);
                var objUser = _userAdminRepository.GetAll().FirstOrDefault(g => g.ID == User.ID);
                var objProcess = _processRepository.GetAll().Where(x => x.FKId == id).ToList();
                if (objProcess.Count > 0)
                {
                    List<int> idProcess = objProcess.Where(x => x.FKId == id).Select(x => x.Id).DefaultIfEmpty().ToList();
                    foreach (var item in idProcess)
                    {
                        _processRepository.Delete(item);
                    }
                }

                //if (User.isAdmin == false && objUser.QuyTrinhXuatBanID.Contains(objQuyTrinhXuatBan.ID.ToString())) 
                //{
                //    return Json(new
                //    {
                //        IsSuccess = false,
                //        Messenger = string.Format("Bạn không có quyền xóa tin xuất bản !")
                //    }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //if (objPost.CreatedBy != User.ID && User.isAdmin == false)
                //{
                //    return RedirectToAction("AccessDenined", "Error");
                //}
                var arrCMT = _newsRepository.GetAllCMT().Where(g => g.NewsID == id).Select(g => g.ID).ToArray();
                var strCMT = string.Join(",", arrCMT);
                DeleteAllCMT(strCMT);
                _newsRepository.Delete(id);
                GenNewsJson();
                LogController.AddLog(string.Format("Xóa bài viết: {0}", objPost.Title), User.Username);

                //}

            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa bài viết thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa bài viết thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var countId = arrid.Count();
            var count = 0;
            var lstQuyTrinhXuatBan = _qProcedureRepository.GetAll().FirstOrDefault(g => g.isPublish == true);
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _newsRepository.Find(Convert.ToInt32(item));
                    if (obj.Status != lstQuyTrinhXuatBan.ID || User.isAdmin != false)
                    {
                        _newsRepository.Delete(Convert.ToInt32(item));
                        count++;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            GenNewsJson();
            if (count == 0)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Bạn không có quyền xóa tin xuất bản"),
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (count < countId)
                {
                    var sub = countId - count;
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = string.Format("Xóa thành công {0} tin bài, còn {1} tin xuất bản", count, sub),
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = string.Format("Xóa thành công {0} tin bài", count),
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "ExpandNews")]
        #endregion

        #region RepComment
        public void GetNews(string urlAddress = "", int CategoryIdTinTuc = 0)
        {
            if (urlAddress != "")
            {
                var resultGet = HelperHtml.GetNews(urlAddress);
                foreach (var item in resultGet)
                {
                    var objNews = new tbl_News();
                    objNews.Title = item.Title.Text;
                    objNews.Description = HelperHtml.RemoveHtmlTags(item.Categories[2].Name);
                    // noi dung
                    objNews.Details = item.Summary.Text;
                    objNews.Details = objNews.Details.Replace("src=\"", "src=\"http://www.mps.gov.vn/");
                    // anh - photo
                    if (item.Links.Count > 1)
                    {
                        if (item.Links[1].Uri.ToString().Contains("image"))
                        {
                            //objNews.FeautureImage = item.Links[1].Uri.ToString();
                            //objNews.CropImage = item.Links[1].Uri.ToString();
                        }
                    }
                    // danh muc
                    if (CategoryIdTinTuc > 0)
                        // objNews.CategoryId = CategoryIdTinTuc;
                        objNews.Author = item.Authors[0].Email;
                    objNews.LangCode = "vn";
                    objNews.Status = 10;
                    //objNews.PageElementId = 1;
                    objNews.AllowComment = 1;
                    objNews.CreatedDate = DateTime.Now;
                    objNews.IsGetNews = 1;
                    objNews.CreatedBy = User.ID;  // developer
                    // kiem tra ton tai truoc khi add
                    var isExist = _newsRepository.GetAll().Count(g => g.Title == item.Title.Text);
                    if (isExist < 1)
                        _newsRepository.Add(objNews);
                }
            }
        }
        [Authorize(Roles = "Index")]
        public ActionResult Reply(int commentId)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser; 
            ViewBag.CommentId = commentId;
            return View();
        }
        public ActionResult ListDataReply(string TuNgay, string DenNgay, int id, int page = 1)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser; 
            TempData["cmtIDz"] = id;
            var obj = new tbl_NewsComment();
            TempData["cmtName"] = _newsRepository.FindCMT(id).NoiDung;
            var NewsId = _newsRepository.FindCMT(id).NewsID;
            TempData["cmtID"] = NewsId;
            TempData["newsName"] = _newsRepository.Find(NewsId).Title;

            var lstData = _newsRepository.GetAllCMT();
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Value.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Value.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            if (id > 0)
            {
                lstData = lstData.Where(g => g.CommentID == id);
            }
            var total = lstData.Count();
            lstData = lstData.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                cmtid = id,
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/ListDataReply.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult DetailsReply(int id)
        {
            var obj = _newsRepository.FindCMT(id);
            TempData["objCMT"] = _newsRepository.FindCMT(Convert.ToInt32(obj.CommentID));
            return Json(RenderViewToString("~/Areas/Admin/Views/News/DetailsReply.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DetailsReply(tbl_NewsComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                _newsRepository.EditCMT(obj);

                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Chỉnh sửa trả lời thành công",
                    cmtid = obj.CommentID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Chỉnh sửa trả lời thành công")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddRepCmt(int id)
        {
            TempData["objCMT"] = _newsRepository.FindCMT(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/News/AddRepCmt.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddRepCmt(tbl_NewsComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                obj.CreateDate = DateTime.Now;
                _newsRepository.AddCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới trả lời thành công",
                    cmtid = obj.CommentID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới trả lời thành công")
                }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region Khong dung toi
        public List<tbl_HomeMenu> newLstCat = new List<tbl_HomeMenu>();
        public List<tbl_HomeMenu> GetPermisCatNews()
        {
            var rowUser = _userAdminRepository.Find(User.ID);
            if (rowUser != null)
            {
                var allCat = _menuRepository.GetAll().Where(s => s.Url == null || s.Url == "#").Where(g => g.Status).ToList();
                if (User.isAdmin)
                    newLstCat = allCat;
                else
                {
                    var GroupUserID = rowUser.GroupUserID;
                    var objGroupUser = _groupuserRepository.Find(Convert.ToInt32(GroupUserID));
                    var lstPermission = HelperXml.DeserializeXml2List<Permission>(objGroupUser.PermissionCatNews);
                    var lstCatID = lstPermission.Select(g => g.AdminMenuId).ToList();
                    newLstCat = allCat.Where(g => lstCatID.Contains(g.ID) && g.Status).ToList();
                }
            }
            return newLstCat;
        }

        public List<tbl_HomeMenu> GetAllCategoryAdd_RESCURSIVE(int pid = 0)
        {
            var allCat = _menuRepository.GetAll().ToList();
            var lstcategories = _menuRepository.GetAll().Where(g => g.IsPermAdd && g.ParentID == pid).ToList();
            if (lstcategories != null)
            {
                foreach (var item in lstcategories)
                {
                    newLstCat.Add(item);
                    var itemc = allCat.Where(g => g.ParentID == item.ID);
                    if (itemc.Any())
                    {
                        GetAllCategoryAdd_RESCURSIVE(item.ID);
                    }
                }
            }
            return newLstCat;
        }
        public ActionResult Sent()
        {
            var abc = User.isAdmin;
            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            var lstQtxb = _qProcedureRepository.GetAll().OrderBy(g => g.Ordering).ToList();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();
            lstCategory = GetPermisCatNews().ToList();
            TempData["Category"] = Common.CreateLevel(lstCategory);
            var Status = _qProcedureRepository.GetAll().ToList();
            var XuatBan = _newsRepository.GetAll().Where(g => g.Status == 6).ToList();
            TempData["XuatBan"] = XuatBan;
            if (User.isAdmin == false)
            {
                var objUser = _userAdminRepository.Find(User.ID);
                if (!string.IsNullOrEmpty(objUser.QuyTrinhXuatBanID))
                {
                    var arrQuyTrinhXuatBanID = objUser.QuyTrinhXuatBanID.Split(',');
                    Status = Status.Where(g => arrQuyTrinhXuatBanID.Contains(g.ID.ToString())).ToList();
                }
            }
            TempData["Status"] = Status.OrderBy(g => g.Ordering).ToList();
            return View();
        }
        #endregion

        #region Comment
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListDataSent(string Title, int Status = 0, int CategoryId = 0, int page = 1, int CreatedBy = 0, string NgayDang = null, string NgayKet = null)
        {

            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;

            var lstCategory = _menuRepository.GetAll().OrderBy(g => g.Ordering).ToList();
            TempData["lstCategory"] = lstCategory;

            var objUser = _userAdminRepository.Find(User.ID);
            var lstChildCat = new List<int>();

            var lstNews = _newsRepository.GetAll().Where(g => g.Type == 1);

            if (!string.IsNullOrEmpty(Title))
            {
                lstNews =
                    lstNews.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Title.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Title.Trim().ToLower())) || (g.Author != null && HelperString.UnsignCharacter(g.Author.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Title.Trim().ToLower()))));
            }

            if (CategoryId > 0)
            {
                lstChildCat = _menuRepository.GetAll().Where(g => g.ParentID == CategoryId).Select(g => g.ID).ToList();
                lstChildCat.Add(CategoryId);
                //lstNews = lstNews.Where(g => lstChildCat.Contains(Convert.ToInt32(g.CategoryIdStr)));
                lstNews = lstNews.Where(g => lstChildCat.Intersect(g.CategoryIdStr.Split(',').Select(i => Convert.ToInt32(i))).Any()).OrderByDescending(g => g.CreatedDate).ToList();
            }

            lstNews = lstNews.OrderByDescending(g => g.CreatedDate);
            if (User.isAdmin == false)
            {
                if (!string.IsNullOrEmpty(objUser.QuyTrinhXuatBanID))
                {
                    var arrQuyTrinhXuatBanID = objUser.QuyTrinhXuatBanID.Split(',');
                    TempData["permQuyTrinhXuatBanID"] = arrQuyTrinhXuatBanID.ToList();
                    lstNews = lstNews.Where(g => g.CreatedBy == User.ID || arrQuyTrinhXuatBanID.Contains(g.Status.ToString()));
                    // phan quyen theo danh muc
                    var lstCatNews = GetPermisCatNews();
                    var arrCatNews = lstCatNews.Select(g => g.ID).ToList();
                   // lstNews = lstNews.Where(g => arrCatNews != null && arrCatNews.Contains(Convert.ToInt32(g.CategoryIdStr)));
                    lstNews = lstNews.Where(g => arrCatNews != null && arrCatNews.Intersect(g.CategoryIdStr.Split(',').Select(i => Convert.ToInt32(i))).Any()).OrderByDescending(g => g.CreatedDate).ToList();
                }
                else
                {
                    return RedirectToAction("AccessDenined", "Error");
                }
            }
            if (CreatedBy > 0)
            {
                lstNews = lstNews.Where(g => g.CreatedBy == CreatedBy);
            }
            if (!string.IsNullOrEmpty(NgayDang) && !string.IsNullOrEmpty(NgayKet))
            {

                var ngaydang = HelperDateTime.ConvertDate(NgayDang);
                var ngayket = HelperDateTime.ConvertDate(NgayKet);
                if (ngaydang > ngayket)
                {
                    Session["Messenger"] = new Notified { Value = EnumNotifield.Error, Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" };
                }
                else
                {
                    lstNews = lstNews.Where(s => s.CreatedDate.Date >= ngaydang && s.CreatedDate.Date <= ngayket);
                }
            }
            var totalNews = lstNews.ToList().Count();
            TempData["totalNews"] = totalNews;
            //var rowLimit = Webconfig.RowLimit;
            var rowLimit = 40;
            lstNews = lstNews.Skip((page - 1) * rowLimit).Take(rowLimit);
            //Quy trình xuất bản tin
            var lstQtxb = _qProcedureRepository.GetAll();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();
            var qtxbId = objUser.QuyTrinhXuatBanID;
            TempData["QuyTrinhXuatBanID"] = qtxbId;
            TempData["lstNews"] = lstNews.ToList();

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/_ListDataSent.cshtml", lstNews),
                totalPages = Math.Ceiling(((double)totalNews / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListDataCmt(int id, int page = 1)
        {
            TempData["cmtIDz"] = id;
            var lstData = _newsRepository.GetAllCMT().Where(g => g.CommentID == id).ToList();

            //TempData["Title"] = _newsRepository.Find(id).ID;
            var total = lstData.Count();
            lstData = lstData.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/ListDataRep.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult Comment(int NewsID, int idcomment = 0)
        {
            var objUser = _userAdminRepository.Find(User.ID); 
            if (objUser.isAdmin || objUser.IsDuyetComment)
                TempData["checkuser"] = true;
            else TempData["checkuser"] = false;

            ViewBag.NewsID = NewsID;
            ViewBag.idcomment = idcomment;
            return View();
        }
        [Authorize(Roles = "View")]
        public ActionResult ListDataComment(string TuNgay, string DenNgay, int id, int page = 1, int idcomment = 0)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;  
            var lstData = _newsRepository.GetAllCMT();
            TempData["tieude"] = _newsRepository.Find(id).Title;
            TempData["cmtid"] = _newsRepository.Find(id).ID;
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Value.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Value.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            if (id > 0)
            {
                lstData = lstData.Where(g => g.NewsID == id);
            }
            if (idcomment > 0)
            {
                lstData = lstData.Where(g => g.ID == idcomment);
            }
            TempData["News"] = _newsRepository.GetAll().ToList();
            var total = lstData.Count();
            lstData = lstData.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit);
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/ListDataComment.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult DetailComment(int id)
        {
            var obj = _newsRepository.FindCMT(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/News/DetailsComment.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult DetailReply(int id)
        {
            var obj = _newsRepository.FindCMT(id);
            TempData["objCMT"] = _newsRepository.FindCMT(Convert.ToInt32(obj.CommentID));
            return Json(RenderViewToString("~/Areas/Admin/Views/News/DetailsReply.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult EditComment(int id)
        {
            var obj = _newsRepository.FindCMT(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/News/EditComment.cshtml", obj), JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "View")]
        [HttpPost]
        public ActionResult EditComment(tbl_NewsComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                _newsRepository.EditCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Chỉnh sửa trả lời thành công",
                    cmtid = obj.NewsID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Chỉnh sửa trả lời không thành công")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "View")]
        public ActionResult EditReply(int id)
        {
            var obj = _newsRepository.FindCMT(id);
            TempData["objCMT"] = _newsRepository.FindCMT(Convert.ToInt32(obj.CommentID));
            return Json(RenderViewToString("~/Areas/Admin/Views/News/EditReply.cshtml", obj), JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "View")]
        [HttpPost]
        public ActionResult EditReply(tbl_NewsComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                _newsRepository.EditCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Chỉnh sửa trả lời thành công",
                    cmtid = obj.CommentID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Chỉnh sửa trả lời không thành công")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _newsRepository.FindCMT(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _newsRepository.EditCMT(obj);
            GenNewsJson();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult EditAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _newsRepository.FindCMT(Convert.ToInt32(item));
                    obj.Status = true;
                    _newsRepository.EditCMT(obj);
                    LogController.AddLog(string.Format("Kích hoạt bình luận: {0}", obj.ID), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Kích hoạt thành công {0} bình luận", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            try
            {
                var objPost = _newsRepository.FindCMT(id);
                // ten bai viet
                var tenbaiviet = _newsRepository.Find(objPost.NewsID).Title;
                //
                var arrRep = _newsRepository.GetAllCMT().Where(g => g.CommentID == id).Select(g => g.ID).ToArray();
                var strRep = string.Join(",", arrRep);
                DeleteAllRep(strRep);
                _newsRepository.DeleteCMT(id);

                LogController.AddLog(string.Format("Xóa bình luận: {0} trong bài viết {1}", objPost.NoiDung, tenbaiviet), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa bình luận thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa bình luận thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAllRep(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _newsRepository.FindCMT(Convert.ToInt32(item));
                    // ten bai viet
                    var tenbaiviet = _newsRepository.Find(obj.NewsID).Title;
                    //
                    _newsRepository.DeleteCMT(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa bình luận: {0} trong bài viết {1}", obj.NoiDung, tenbaiviet), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} bình luận", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAllCMT(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var obj = _newsRepository.FindCMT(Convert.ToInt32(item));
                        var tenbaiviet = "";
                        // ten bai viet
                        if (obj != null)
                        {
                            var objBaiViet = _newsRepository.Find(obj.NewsID);
                            if(objBaiViet != null)
                            {
                                tenbaiviet = objBaiViet.Title;
                            }
                        }
                        
                        //
                        _newsRepository.DeleteCMT(Convert.ToInt32(item));
                        LogController.AddLog(string.Format("Xóa bình luận: {0} trong bài viết {1}", obj.NoiDung, tenbaiviet), User.Username);
                        count++;
                    } 
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} bình luận", count),
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Thống kê
        public ActionResult ThongKe(string keySearch, int page = 1, int CreatedBy = 0,string categoryId="", string tuNgay = null, string denNgay = null, int luaChon = 1, int TraLai = 0, int IsPublish = 1, string Type = "")
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var langcode = "vn";
            if (Session["langCodeDefaut"] != null)
                langcode = Session["langCodeDefaut"].ToString();

            var lstUserAdmin = _userAdminRepository.GetAll().Where(x => x.Active).ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            var lstqProcedure = _qProcedureRepository.GetAll();
            var qProcedureFirst = lstqProcedure.FirstOrDefault(g => g.Code == "khoitao");
            TempData["qProcedureFirst"] = (qProcedureFirst != null ? qProcedureFirst.ID : 0);

            var allCat = _cateRepository.GetAll();
            var lstCategory = Common.CreateLevel(allCat.OrderBy(g => g.Ordering).ToList());
            TempData["Category"] = lstCategory;

            var allProcess = _processRepository.GetAll().ToList();

            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;

            #region Get list news 
            // phan trang store procedure 
            var lstNews = new List<tbl_News_Custom>();
            var pagesize = 5000; // limit
            var condition = " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(keySearch))
            {
                var fieldSearch = "Title";
                if (luaChon == 2)
                    fieldSearch = "Details";
                if (luaChon == 3)
                    fieldSearch = "Author";
                if (luaChon == 4)
                    fieldSearch = "SourceNews";
                condition += " And (LOWER(" + fieldSearch + ") like N^*^" + keySearch.Trim().ToLower() + "^-^ OR LOWER(" + fieldSearch + ") like N^*^" + HelperString.UnsignCharacter(keySearch.Trim().ToLower()) + "^-^)";
            }
            if (objUser.isAdmin == false)
            {
                if (!string.IsNullOrEmpty(objUser.QuyTrinhXuatBanID))
                {
                    var arrQuyTrinhXuatBanID = objUser.QuyTrinhXuatBanID.Split(',');
                    TempData["permQuyTrinhXuatBanID"] = arrQuyTrinhXuatBanID.ToList();
                    if (arrQuyTrinhXuatBanID.Contains(qProcedureFirst.ToString()))
                    {
                        condition += " And CreatedBy = " + User.ID;
                    }
                    else
                    {
                        var lst = new List<tbl_News_Custom>();
                        List<int> idProcess = allProcess.Where(x => arrQuyTrinhXuatBanID.Contains(x.ToProcedure.ToString())).Select(x => x.FKId).Distinct().DefaultIfEmpty().ToList();
                        foreach (var item in idProcess)
                        {
                            var objNews = lstNews.Where(x => x.ID == item || x.CreatedBy == User.ID || x.IsPublish).ToList();
                            lst.AddRange(objNews);
                        }
                        lstNews = lst.DistinctBy(x => x.ID).OrderByDescending(x => x.CreatedDate).ToList();
                    }

                }
            }
            if (CreatedBy > 0)
            {
                condition += " And CreatedBy = " + CreatedBy;
            }
            //if (string.IsNullOrEmpty(tuNgay) && string.IsNullOrEmpty(denNgay))
            //{
            //    lstNews = lstNews.Where(g => g.CreatedDate != null && g.CreatedDate.Date >= startDate && g.CreatedDate.Date <= endDate).ToList();
            //}
            var lstloai = _configRepository.GetAll().Where(g => g.Code == Type && g.Status).Select(g => g.ID).ToList();
            var strloai = string.Join(",", lstloai);
            if (lstloai != null && lstloai.Count > 0)
            {
                condition += " AND NewMoney IN ('" + strloai.Replace(",", "','") + "')";
            }

            if (!string.IsNullOrEmpty(tuNgay) && !string.IsNullOrEmpty(denNgay))
            {
                var ngaydang = HelperDateTime.ConvertDate(tuNgay);
                var ngayket = HelperDateTime.ConvertDate(denNgay);
                if (ngaydang > ngayket)
                {
                    return Json(new { IsSuccess = false, Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" }, JsonRequestBehavior.AllowGet);
                }
            }
            if (!string.IsNullOrEmpty(tuNgay))
            {
                var ngaydang = HelperDateTime.ConvertDate(tuNgay + " 00:00");
                condition += " And CreatedDate >= '" + ngaydang + "'";
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                var ngayket = HelperDateTime.ConvertDate(denNgay + " 23:59");
                condition += " And CreatedDate <= '" + ngayket + "'";
            }
            
            if (string.IsNullOrEmpty(tuNgay) && string.IsNullOrEmpty(denNgay))
            {
                condition += " And CreatedDate >= '" + startDate + "'";
                condition+= " And CreatedDate <= '" + endDate + "'";
            }

            condition += " And IsPublish = " + IsPublish;
            condition += " And IsTraLai = " + TraLai;

            var fullCatIdStr = "";
            if (!string.IsNullOrEmpty(categoryId))
            {
                var fullCatId = Common.GetChild(allCat, Convert.ToInt32(categoryId));
                fullCatIdStr = string.Join(",", fullCatId);
                //condition += " AND CategoryIdStr IN ('" + fullCatIdStr.Replace(",", "','") + "')";
                condition += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
            }
            var orderby = "ORDER BY CreatedDate DESC";
            var field = "ID,Title,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney";
            var DicLevel = _newsRepository.spGetListNews(page, pagesize, field, "view_News_MultiCate", condition, orderby);
            var totalNews = 0;
            if (DicLevel.Count > 0)
            {
                var objDic = DicLevel.FirstOrDefault();
                lstNews = objDic.Key;
                totalNews = Convert.ToInt32(objDic.Value);
            }
            
            #endregion 
            TempData["lstProcess"] = allProcess;
            var lstConfig = _configRepository.GetAll().OrderBy(g => g.Ordering).Where(x => x.Status && x.LangCode == langcode).ToList();
            TempData["lstConfig"] = lstConfig.ToList();

            TempData["lstNews"] = lstNews.ToList();
            TempData.Keep();
            TempData["totalNews"] = lstNews.Count();
            //Quy trình xuất bản tin 
            TempData["QuyTrinhXuatBan"] = lstqProcedure.ToList();
            TempData["QuyTrinhXuatBanID"] = objUser.QuyTrinhXuatBanID;
            
            
            if (!string.IsNullOrEmpty(tuNgay) && !string.IsNullOrEmpty(denNgay))
            {
                var ngaydang = HelperDateTime.ConvertDate(tuNgay);
                Session["tuNgay"] = tuNgay;
                var ngayket = HelperDateTime.ConvertDate(denNgay);
                Session["denNgay"] = denNgay;
                if (ngaydang > ngayket)
                {
                    return Json(new { IsSuccess = false, Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    lstNews = lstNews.Where(g => g.CreatedDate.Date >= ngaydang && g.CreatedDate.Date <= ngayket.Date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(keySearch))
            {
                var fieldSearch = "Title";
                if (luaChon == 2)
                    fieldSearch = "Details";
                if (luaChon == 3)
                    fieldSearch = "Author";
                if (luaChon == 4)
                    fieldSearch = "SourceNews";
                lstNews = lstNews.Where(g => (HelperString.UnsignCharacter(fieldSearch.Trim().ToLower())).Contains(HelperString.UnsignCharacter(keySearch.Trim().ToLower()))).ToList();
            }



            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var listPosByCode = _positionRepository.GetListPosByCode("new");
            TempData["LstPos"] = listPosByCode;
            var lstCate = Common.CreateLevel(_cateRepository.GetAll().OrderBy(g => g.Ordering).Where(g => g.Active && g.LangCode == langCode).ToList());
            TempData["CategoryNews"] = lstCate;

            var curl = HttpContext.Request.Url.AbsolutePath;
            var adminMenuID = _adminMenuRepository.GetAll().FirstOrDefault(g => g.Url.Trim() == curl);

            if (adminMenuID != null)
            {
                var act = new GetAction();
                var action = act.Get(User.GroupUser, adminMenuID.ID, User.isAdmin);
                TempData["action"] = action.ToList();
            }
            

            var lstQtxb = _qProcedureRepository.GetAll().OrderBy(g => g.Ordering).ToList();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();

            //lstCategory = GetPermisCatNews().ToList();
            //TempData["Category"] = Common.CreateLevel(lstCategory);

            var lstStatus = _qProcedureRepository.GetAll().ToList();
            TempData["Status"] = lstStatus.OrderBy(g => g.Ordering).ToList();
            
            var objkhoitao = _qProcedureRepository.GetAll().FirstOrDefault(g => g.Code == "khoitao");
            var objBienTap = _qProcedureRepository.GetAll().FirstOrDefault(g => g.Code == "bientap");
            TempData["objkhoitao"] = objkhoitao;
            TempData["objBienTap"] = objBienTap;
            Session["lstNews"] = lstNews;
            return View(lstNews);
          
        }
        #endregion
        #region ExportExcel
        public string ExportExcel()
        {
            DateTime nowDay = DateTime.Now;
            var startDate = new DateTime(nowDay.Year, nowDay.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var tuNgay = Session["tuNgay"];
            var denNgay = Session["denNgay"];
            if(tuNgay==null&& denNgay==null)
            {
                tuNgay = startDate;
                denNgay = endDate;
            }

            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            var lstCategory = Common.CreateLevel(_cateRepository.GetAll().OrderBy(g => g.Ordering).ToList());
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_News_Custom>)Session["lstNews"];
                TempData.Keep();
                var ws = package.Workbook.Worksheets.Add("Sheet1");

                ws.Cells["A1:C1"].Merge = true;
                ws.Cells["G1:I1"].Merge = true;
                ws.Cells["G2:I2"].Merge = true;
                ws.Cells["A2:C2"].Merge = true;
                ws.Cells["A4:I4"].Merge = true;
                ws.Cells["A5:I5"].Merge = true;
                ws.Cells["G6:I6"].Merge = true;

                ws.Cells["A1"].Value = "BAN TUYÊN GIÁO TRUNG ƯƠNG";
                ws.Cells["G1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                ws.Cells["A2"].Value = "TẠP CHÍ TUYÊN GIÁO";
                ws.Cells["G2"].Value = "Độc lập - tự do - hạnh phúc";
                ws.Cells["A4"].Value = "BÁO CÁO TÌNH HÌNH NHUẬN BÚT";
                if (string.IsNullOrEmpty(tuNgay.ToString()) || string.IsNullOrEmpty(denNgay.ToString()))
                {
                    ws.Cells[5, 1].Value = "";
                }
                else
                {
                    ws.Cells[5, 1].Value = "("+ tuNgay + " - "+ denNgay + ")";
                }

                ws.Cells[5, 4].Value = denNgay;
                ws.Cells["G6:I6"].Value = "Ngày giờ xuất báo cáo: " + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);
                ws.Cells[7, 1].Value = "STT";
                ws.Cells[7, 2].Value = "TÊN BÀI VIẾT";
                ws.Cells[7, 3].Value = "TÁC GIẢ";
                ws.Cells[7, 4].Value = "SỐ TÀI KHOẢN";
                ws.Cells[7, 5].Value = "NGƯỜI KHỞI TẠO";
                ws.Cells[7, 6].Value = "NGƯỜI BIÊN TẬP";
                ws.Cells[7, 7].Value = "NGÀY ĐĂNG";
                ws.Cells[7, 8].Value = "LOẠI TIN BÀI";
                ws.Cells[7, 9].Value = "NB";
                ws.Cells["A7:I7"].Style.Font.Bold = true;
                ws.Cells["A1:I6"].Style.Font.Size = 13;
                ws.Cells["A7:I7"].Style.Font.Size = 12;
                ws.Cells["A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A1:C2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:C2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["G1:I2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["G1:I2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                ws.Cells["A4:I4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A4:I4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A4:I4"].Style.Font.Bold = true;

                ws.Cells["A5:I5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A5:I5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                
                ws.Cells["G6:I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["A:XFH"].Style.Font.Name = "Times New Roman";

                ws.Row(7).Height = 20;
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
              //  ws.Column(2).AutoFit();
                ws.Column(7).Style.Numberformat.Format = "#,#";
                int iRow = 7;
                int stt = 0;
                string loaitinbai = "";
                var objkhoitao = _qProcedureRepository.GetAll().FirstOrDefault(g => g.Code == "khoitao");
                var objBienTap = _qProcedureRepository.GetAll().FirstOrDefault(g => g.Code == "bientap");
                foreach (var item in Data)
                {
                    var quytrinh = _processRepository.GetAll().Where(g => g.FKId == item.ID).ToList();
                    var SoTaiKhoan = "";
                    var objUser = new tbl_UserAdmin();
                    var idTacGia = item.CreatedBy;
                    if (item.CTVID > 0)
                    {
                        idTacGia = item.CTVID;
                    }
                    objUser = _repUserAdmin.Find(idTacGia.Value);
                    if (objUser != null)
                        SoTaiKhoan = objUser.SoTaiKhoan;
                    if (item.NewMoney.HasValue)
                    {
                        var objLoaiTin = _configRepository.Find(item.NewMoney.Value);
                        if (objLoaiTin != null)
                            loaitinbai = objLoaiTin.Name;
                    }
                    var nguoikhoitao = "";
                    var objProcess_khoitao = _processRepository.GetAll().FirstOrDefault(g => g.FKId == item.ID
                    && g.FromProcedure == objkhoitao.ID);
                    if(objProcess_khoitao!=null)
                    {
                        var objUserkhoitao = _userAdminRepository.Find(objProcess_khoitao.FromId);
                        if (objUserkhoitao != null)
                            nguoikhoitao = objUserkhoitao.FullName;
                    }
                    var nguoibientap = "";
                    var objProcess_bientap = _processRepository.GetAll().LastOrDefault(g => g.FKId == item.ID && g.ToProcedure == objBienTap.ID);
                    if(objProcess_bientap!=null)
                    {
                        var objUserBienTap = _userAdminRepository.Find(objProcess_bientap.FromId);
                        if (objUserBienTap != null)
                            nguoibientap = objUserBienTap.FullName;
                    }
                    stt++;
                    iRow++;
                    ws.Cells["A" + iRow].Value = stt;
                    ws.Cells["B" + iRow].Value = item.Title;
                    ws.Cells["C" + iRow].Value = item.Author;
                    ws.Cells["D" + iRow].Value = SoTaiKhoan;
                    ws.Cells["E" + iRow].Value = nguoikhoitao;
                    ws.Cells["F" + iRow].Value = nguoibientap;
                    ws.Cells["G" + iRow].Value = HelperDateTime.ConvertDateToString(item.CreatedDate);
                    ws.Cells["H" + iRow].Value = loaitinbai;
                    ws.Cells["I" + iRow].Value = item.NhuanBut;
                    ws.Cells["I" + iRow].Style.Numberformat.Format = "#,#";

                    ws.Cells["A7" + ":I" + iRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":I" + iRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":I" + iRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":I" + iRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //Căn vị trí
                    ws.Cells["A" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B8" + ":I" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["E8" + ":E" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A7" + ":I" + iRow].Style.Font.Size = 11;
                    ws.Cells["A8" + ":I" + iRow].Style.WrapText = true;

                }
                ws.Cells["H" + (Data.Count + 8) + ":I" + (Data.Count + 8)].Merge = true;
                ws.Cells[Data.Count + 8, 7].Value = "Tổng cộng: ";
                ws.Cells[Data.Count + 8, 7].Style.Font.Bold = true;
                ws.Cells[Data.Count + 8, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells[Data.Count + 8, 8].Formula = string.Format("=SUM(I8:I{0})", iRow);
                ws.Cells[Data.Count + 8, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[Data.Count + 8, 8].Style.Numberformat.Format = "#,#";
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("NhuanButTin-{0}-{1}-{2}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        #endregion

        #region Có mấy hàm ở trong này
        [HttpPost]
        public ActionResult LoadRelatedNews()
        {
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var orderby = "ORDER BY CreatedDate DESC";
            var page = 1;
            var pagesize = 100; // limit
            var condition = " WHERE 1=1 ";
            var field = "ID,Title,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney";
            var DicLevel = _newsRepository.spGetListNews(page, pagesize, field,"view_News_MultiCate", condition, orderby);
            var lstNews = new List<tbl_News_Custom>();
            if (DicLevel.Count > 0)
            {
                var objDic = DicLevel.FirstOrDefault();
                lstNews = objDic.Key;
            }
            TempData["lstNews"] = lstNews;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/RelatedNews.cshtml"),
            }, JsonRequestBehavior.AllowGet);
        }
        public static string RemoveChar(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                var charsToRemove = new string[] { ".", "," };

                foreach (var c in charsToRemove)
                {
                    str = str.Replace(c, string.Empty);
                }
            }
            return str;
        }
        public void ChangeProcess(int id, string code, qProcess obj)
        {
            var objNews = _newsRepository.Find(id);
            obj.FKId = id;
            obj.FromId = User.ID;
            obj.TableCode = code;
            obj.FromProcedure = Convert.ToInt32(objNews.Status);
            obj.CreateDate = DateTime.Now;
            _processRepository.Add(obj);
        }
        public void SaveHistory(tbl_News obj)
        {
            var currentNews = obj;
            var newsHistory = _newsHistoryRepository.GetAll().FirstOrDefault(g => g.NewsID == obj.ID);
            var lstNewsVersion = new List<tbl_NewsVersion>();
            var objNewsVersion = new tbl_NewsVersion();
            Common.CopyDataObj2Obj(currentNews, objNewsVersion);
            objNewsVersion.LastModifieDate = DateTime.Now;
            objNewsVersion.LastModifieBy = User.Username;
            if (newsHistory != null)
            {
                lstNewsVersion = HelperXml.DeserializeXml2List<tbl_NewsVersion>(newsHistory.Contents);
                objNewsVersion.Version = lstNewsVersion.Count + 1;
                lstNewsVersion.Add(objNewsVersion);
                newsHistory.Contents = HelperXml.SerializeList2Xml(lstNewsVersion);
                _newsHistoryRepository.Edit(newsHistory);
            }
            else
            {
                objNewsVersion.Version = 1;
                lstNewsVersion.Add(objNewsVersion);
                newsHistory = new tbl_HistoryNews
                {
                    NewsID = obj.ID,
                    Contents = HelperXml.SerializeList2Xml(lstNewsVersion)
                };
                _newsHistoryRepository.Add(newsHistory);
            }
           
        }
        #endregion

        public ActionResult Choose(int id)
        {
            string message = "";
            var obj = _newsRepository.Find(id);
            if (obj.IdUserChoose == null)
            {
                obj.IdUserChoose = User.ID;
                obj.TimeChoose = DateTime.Now;
                message = "Lựa chọn tin bài thành công!";
            }
            else
            {
                if (obj.IdUserChoose == User.ID)
                {
                    obj.IdUserChoose = null;
                    message = "Hủy lựa chọn tin bài thành công!";
                }
                else 
                {
                    message = _userAdminRepository.GetAll().FirstOrDefault(g => g.ID == User.ID).FullName + " đang chọn.";
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                _newsRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = "Chọn/Hủy chọn thất bại!";
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = message,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetNewsRow(int id)
        {
            string message = "";
            var obj = _newsRepository.Find(id);
            if (obj.IdUserChoose == null)
            {
                obj.IdUserChoose = User.ID;
                obj.TimeChoose = DateTime.Now;
                message = "Lựa chọn tin bài thành công!";
            }
            else
            {
                var objProcedure = _qProcedureRepository.Find(Convert.ToInt32(obj.Status));
                var objUser = _userAdminRepository.Find(User.ID);
                var lstProcedure = _qProcedureRepository.GetAll().Where(g => g.Ordering >= objProcedure.Ordering && objUser.QuyTrinhXuatBanID.Split(',').Contains(g.ID.ToString())).OrderByDescending(g=>g.Ordering).ToList();
                var minutescheck = ConfigTextController.GetValueCFT("minutescheck", "15");
                if (obj.TimeChoose != null && Convert.ToDateTime(obj.TimeChoose).AddMinutes(Convert.ToInt32(minutescheck)) < DateTime.Now && lstProcedure != null && lstProcedure.Count > 0)
                {
                    obj.IdUserChoose = User.ID;
                    obj.Status = lstProcedure.FirstOrDefault().ID;
                    obj.TimeChoose = DateTime.Now;
                    message = "Lựa chọn tin bài thành công!";
                }
                else
                {
                    message = "Chưa đến thời gian lấy tin bài!";
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = message
                    }, JsonRequestBehavior.AllowGet);
                } 
            }
            try
            {
                _newsRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = "Chọn/Hủy chọn thất bại!";
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = message,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReplaceRow(int id)
        {

            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
            {
                langCode = Session["langCodeDefaut"].ToString();
            }
            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;
            if (objUser.isAdmin == false)
            {
                if (!string.IsNullOrEmpty(objUser.QuyTrinhXuatBanID))
                {
                    var arrQuyTrinhXuatBanID = objUser.QuyTrinhXuatBanID.Split(',');
                    TempData["permQuyTrinhXuatBanID"] = arrQuyTrinhXuatBanID.ToList();
                }
                else
                {
                    return RedirectToAction("AccessDenined", "Error");
                }
            }

            var obj = _newsRepository.Find(id); 
            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            var lstqProcedure = _qProcedureRepository.GetAll().ToList(); 
            var qProcedureFirst = lstqProcedure.Where(x => x.Code == "khoitao").Select(x => x.ID).FirstOrDefault();
            TempData["qProcedureFirst"] = qProcedureFirst;
            var lstCategory = Common.CreateLevel(_cateRepository.GetAll().OrderBy(g => g.Ordering).ToList());
            TempData["Category"] = lstCategory; 
            var objkhoitao = _qProcedureRepository.GetAll().FirstOrDefault(g => g.Code == "khoitao");
            TempData["objkhoitao"] = objkhoitao;
            var lstProcess = _processRepository.GetAll().ToList();
            TempData["lstProcess"] = lstProcess;
            var lstConfig = _configRepository.GetAll().OrderBy(g => g.Ordering).Where(x => x.Status && x.LangCode == langCode).ToList();
            TempData["lstConfig"] = lstConfig.ToList(); 
            //Quy trình xuất bản tin
            var lstQtxb = _qProcedureRepository.GetAll();
            TempData["QuyTrinhXuatBan"] = lstQtxb.ToList();
            var qtxbId = objUser.QuyTrinhXuatBanID;
            TempData["QuyTrinhXuatBanID"] = qtxbId;

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/ReplaceRow.cshtml", obj),
            }, JsonRequestBehavior.AllowGet);
        }
        public class Select2Model
        {
            public string id { get; set; }
            public string text { get; set; }
        }
        public ActionResult GetEmployeeList(string q)
        {

            var list = new List<Select2Model>();

            list.Add(new Select2Model()
            {
                text = "India",
                id = "101"
            });
            list.Add(new Select2Model()
            {
                text = "Srilanka",
                id = "102"
            });
            list.Add(new Select2Model()
            {
                text = "Singapore",
                id = "103"
            });

            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                list = list.Where(x => x.text.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRelateNews(int pageNum = 1, int pagesize = 10, string keySearch = "")
        {
            //Get the paged results and the total count of the results for this query.  
            var lstNewsCustoms = new List<tbl_News_Custom>();
            var condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
            if (!string.IsNullOrEmpty(keySearch))
            {
                var fieldSearch = "Title";
                condition += " And (LOWER(" + fieldSearch + ") like N^*^" + keySearch.Trim().ToLower() + "^-^ OR LOWER(" + fieldSearch + ") like N^*^" + HelperString.UnsignCharacter(keySearch.Trim().ToLower()) + "^-^)";
            }
            var orderby = "ORDER BY CreatedDate DESC";
            var field = "ID,Title,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney";
            var DicLevel = _newsRepository.spGetListNews(pageNum, pagesize, field,"view_News_Multicate", condition, orderby);
            var totalNews = 0;
            if (DicLevel.Count > 0)
            {
                var objDic = DicLevel.FirstOrDefault();
                lstNewsCustoms = objDic.Key;
                totalNews = Convert.ToInt32(objDic.Value);
            }
            //Translate the attendees into a format the select2 dropdown expects
            var ListSelect2Result = new List<Select2Result>();
            var objSelect2Result = new Select2Result();
            if (lstNewsCustoms != null)
            {
                foreach (var item in lstNewsCustoms)
                {
                    objSelect2Result = new Select2Result();
                    objSelect2Result.text = item.Title;
                    objSelect2Result.id = item.ID;
                    ListSelect2Result.Add(objSelect2Result);
                }
            }
            return Json(new
            {
                Data = ListSelect2Result,
                page = pageNum,
                pagesize = pagesize,
                Total = totalNews
            }, JsonRequestBehavior.AllowGet);
        }
        public class Select2Result
        {
            public int id { get; set; }
            public string text { get; set; }
        } 
        public void GenNewsJson()
        {
            ILimitRepository _limitRepository = new LimitRepository();
            var limithot1 = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code.Trim() == "tin-tuc-hot-tren-slide");

            var limithot1value = (limithot1 != null ? limithot1.Value : 20);
            var lstNewHome1 = new List<tbl_News_Custom>();
            var condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE() ";
            condition += " AND Position LIKE '%new_hot%'";
            var orderby = "ORDER BY CreatedDate DESC";
            var field = "ID,Title,Photo,Description,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney,Position,CategoryIdStr";
            var DicLevel = _newsRepository.spGetListNews(1, limithot1value, field,"view_News_MultiCate", condition, orderby);
            if (DicLevel.Count > 0)
            {
                var objDic = DicLevel.FirstOrDefault();
                lstNewHome1 = objDic.Key;
            }
            var _lstNewHome1 = new List<tbl_News_Custom>();
            var _condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate > GETDATE() ";
            _condition += " AND Position LIKE '%new_hot%'";
            var _orderby = "ORDER BY CreatedDate DESC";
            var _DicLevel = _newsRepository.spGetListNews(1, 100, field, "view_News_MultiCate", _condition, _orderby);
            if (_DicLevel.Count > 0)
            {
                var objDic = _DicLevel.FirstOrDefault();
                _lstNewHome1 = objDic.Key;
            }
            lstNewHome1.AddRange(_lstNewHome1);
            Common.genCommonFileJson(lstNewHome1, "lstNewHome1.json");

            var limithot2 = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code.Trim() == "tin-tuc-hot-slide");
            var limithot2value = (limithot2 != null ? limithot1.Value : 4);
            var lstNewHome2 = new List<tbl_News_Custom>();
            var condition1 = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
            condition1 += " AND Position LIKE '%new_underhot%'";
            var orderby1 = "ORDER BY CreatedDate DESC";
            var DicLevel1 = _newsRepository.spGetListNews(1, limithot2value, field, "view_News_MultiCate", condition1, orderby1);
            if (DicLevel1.Count > 0)
            {
                var objDic = DicLevel1.FirstOrDefault();
                lstNewHome2 = objDic.Key;
            }
            var _lstNewHome2 = new List<tbl_News_Custom>();
            var _condition1 = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate > GETDATE()";
            _condition1 += " AND Position LIKE '%new_underhot%'";
            var _orderby1 = "ORDER BY CreatedDate DESC";
            var _DicLevel1 = _newsRepository.spGetListNews(1, 100, field, "view_News_MultiCate", _condition1, _orderby1);
            if (_DicLevel1.Count > 0)
            {
                var objDic = _DicLevel1.FirstOrDefault();
                _lstNewHome2 = objDic.Key;
            }
            lstNewHome2.AddRange(_lstNewHome2);
            Common.genCommonFileJson(lstNewHome2, "lstNewHome2.json");

            //Ban can biet
            var objlimit = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code == "ban-can-biet");
            TempData["limit"] = (objlimit != null ? objlimit.Value : 5);
            var objcate = _cateRepository.GetAll().FirstOrDefault(g => g.Position != null && g.Position.Split(',').Contains("category_right") && g.Active);
            var lstNewRight1 = new List<tbl_News_Custom>();
            var _lstNewRight1 = new List<tbl_News_Custom>();
            if (objcate != null)
            {
                var allCat = _cateRepository.GetAll();
                var fullCatId = Common.GetChild(allCat, objcate.ID);
                var fullCatIdStr = string.Join(",", fullCatId);
                // phan trang store procedure
                var pagesize = (objlimit != null ? objlimit.Value : 5);
                var condition_bcb = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
                if (!string.IsNullOrEmpty(fullCatIdStr))
                {
                    condition_bcb += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
                }
                var orderby_bcb = "ORDER BY CreatedDate DESC";
                field = "ID,Title,Photo,Description,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney,Position,CategoryIdStr,CateComma";
                var DicLevel_bcb = _newsRepository.spGetListNews(1, pagesize, field,"view_News_MultiCate", condition_bcb, orderby_bcb);
                
                if (DicLevel_bcb.Count > 0)
                {
                    var objDic = DicLevel_bcb.FirstOrDefault();
                    lstNewRight1 = objDic.Key;
                }   
                // them cac tin Hen Gio Xuat ban vao nua
                var _condition_bcb = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate > GETDATE()";
                if (!string.IsNullOrEmpty(fullCatIdStr))
                {
                    _condition_bcb += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
                }
                var _orderby_bcb = "ORDER BY CreatedDate DESC";
                field = "ID,Title,Photo,Description,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney,Position,CategoryIdStr,CateComma";
                var _DicLevel_bcb = _newsRepository.spGetListNews(1, 100, field,"view_News_MultiCate", _condition_bcb, _orderby_bcb);
                
                if (_DicLevel_bcb.Count > 0)
                {
                    var objDic = _DicLevel_bcb.FirstOrDefault();
                    _lstNewRight1 = objDic.Key;
                }
                lstNewRight1.AddRange(_lstNewRight1);
            }           
            Common.genCommonFileJson(lstNewRight1, "bancanbiet.json");

            //Su kin noi bat            
            var objlimit_sknb = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code == "su-kien-noi-bat");

            var limitval = (objlimit_sknb != null ? objlimit_sknb.Value : 5);
            var lstNewRight_sknb = new List<tbl_News_Custom>();
            var condition_sknb = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
            condition_sknb += " AND Position LIKE '%new_sukien%'";
            var orderby_sknb = "ORDER BY CreatedDate DESC";
            field = "ID,Title,Photo,Description,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney,Position,CategoryIdStr";
            var DicLevel_sknb = _newsRepository.spGetListNews(1, limitval, field, "view_News_MultiCate", condition_sknb, orderby_sknb);
            if (DicLevel_sknb.Count > 0)
            {
                var objDic = DicLevel_sknb.FirstOrDefault();
                lstNewRight_sknb = objDic.Key;
            }
            // Them tin HEN GIO XUAT BAN
            var _lstNewRight_sknb = new List<tbl_News_Custom>();
            var _condition_sknb = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate > GETDATE()";
            _condition_sknb += " AND Position LIKE '%new_sukien%'";
            var _orderby_sknb = "ORDER BY CreatedDate DESC";
            field = "ID,Title,Photo,Description,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney,Position,CategoryIdStr";
            var _DicLevel_sknb = _newsRepository.spGetListNews(1, 100, field, "view_News_MultiCate", _condition_sknb, _orderby_sknb);
            if (_DicLevel_sknb.Count > 0)
            {
                var objDic = _DicLevel_sknb.FirstOrDefault();
                _lstNewRight_sknb = objDic.Key;
            }
            lstNewRight_sknb.AddRange(_lstNewRight_sknb);
            Common.genCommonFileJson(lstNewRight_sknb, "sukiennoibat.json");

            GenListHome();
        }
        public void GenListHome()
        {
            ILimitRepository _limitRepository = new LimitRepository();
            var allCat = _cateRepository.GetAll();
            var limit = _limitRepository.GetAll().ToList();
            int pagesize;
            var orderby = "ORDER BY CreatedDate DESC";
            var arrayCount = new[] { 1, 2, 3 };
            for (int i = 1; i <= arrayCount.Length; i++)
            {
                var limit1 = limit.FirstOrDefault(g => g.Code == "tin-trang-chu-loai-" + i);

                var lstCateHome1 = allCat.Where(g => g.Position != null && g.Position.Split(',').Contains("category_center" + i) && g.Active).OrderBy(x => x.Ordering).ToList();
                if (lstCateHome1 != null)
                {
                    pagesize = (limit1 != null ? limit1.Value : 5);
                    foreach (var item in lstCateHome1)
                    {
                        var condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
                        var fullCatId = Common.GetChild(allCat, item.ID);
                        var fullCatIdStr = string.Join(",", fullCatId);
                        if (!string.IsNullOrEmpty(fullCatIdStr))
                        {
                            condition += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
                        }
                        var field = "ID,Title,Photo,Description,Author,CreatedDate,NhuanBut,CreatedBy,NewMoney,Position,CategoryIdStr,CateComma";
                        var DicLevel = _newsRepository.spGetListNews(1, pagesize, field,"view_News_MultiCate", condition, orderby);
                        var lstHomeNews = new List<tbl_News_Custom>();
                        if (DicLevel.Count > 0)
                        {
                            var objDic = DicLevel.FirstOrDefault();
                            lstHomeNews = objDic.Key;
                        }
                        // Them cac tin HEN GIO XUAT BAN
                        var _condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate > GETDATE()";
                        if (!string.IsNullOrEmpty(fullCatIdStr))
                        {
                            _condition += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
                        }
                        var _DicLevel = _newsRepository.spGetListNews(1, 100, field,"view_News_MultiCate", _condition, orderby);
                        var _lstHomeNews = new List<tbl_News_Custom>();
                        if (DicLevel.Count > 0)
                        {
                            var objDic = _DicLevel.FirstOrDefault();
                            _lstHomeNews = objDic.Key;
                        }
                        lstHomeNews.AddRange(_lstHomeNews);
                        string strFileName = item.ID + ".json";
                        Common.genCommonFileJson(lstHomeNews, strFileName);
                    }
                }
            }
        }
    }
}
