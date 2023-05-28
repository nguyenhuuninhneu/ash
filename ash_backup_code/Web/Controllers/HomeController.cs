using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;
using Web.Core;
using Web.Areas.Admin.Controllers;
using Web.Model.CustomModel;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Web.Controllers
{ 
    public class HomeController : BaseController
    { 
        readonly IAdvRepository _advRepository = new AdvRepository(); 
        readonly ICategoryRepository _cateRepository = new CategoryRepository(); 
        readonly INewsRepository _newsRepository = new NewsRepository();
        readonly ISlideImagesRepository _slideImagesRepository = new SlideImagesRepository(); 
        readonly IHomeMenuRepository _menuhomecategory = new HomeMenuRepository(); 
        readonly IAccessViewRespository _accessViewRespository = new AccessViewRepository();
        readonly IAccessOnlineRespository _accessOnlineRespository = new AccessOnlineRepository();
        readonly IMagazineRepository _magazineRespository = new MagazineRepository();
        readonly IFooterRepository _footerRepository = new FooterRepository();
        readonly ILanguagesRepository _languagesRep = new LanguagesRepository();
        readonly IqProcedureRepository _qProcedureRepository = new qProcedureRepository();
        public ActionResult Index()
        {
            InitPageElementId(); 
            return View();
        }
         
        public ActionResult Search()
        {
            var langCode = "vn";
            if (Session["langCodeHome"] != null)
                langCode = Session["langCodeHome"].ToString();
            var lstCate = _cateRepository.GetAll().Where(g=>g.Active && g.LangCode == langCode).OrderBy(g=>g.Ordering).ThenBy(g=>g.Name).ToList();
            lstCate = Common.CreateLevel(lstCate.ToList());
            TempData["Category"] = lstCate;
            return View();
        }
        [HttpPost]
        public ActionResult Search(string keySearch, string categoryId, int luaChon = 1, int tuychon = 0, int page = 1, int pagesize = 15, string tuNgay = null, string denNgay = null)
        {
            // tuy chon: 1 - chinhxac, 0 - gan dung
            // luachon: 1 - Title, 2 - Details, 3 - Author, 4-SourceNews
            var langCode = "vn";
            if (Session["langCodeHome"] != null)
                langCode = Session["langCodeHome"].ToString();
            string notif = "";
            if (string.IsNullOrEmpty(keySearch) && string.IsNullOrEmpty(categoryId) && luaChon == 0 && string.IsNullOrEmpty(tuNgay) && string.IsNullOrEmpty(denNgay))
            {
                notif = @ConfigTextController.GetValueCFT("khongtimthayketquanao");
                TempData["ThongBao"] = notif;
                return Json(new
                {
                    Success = true,
                    viewContent = RenderViewToString("~/Views/Home/_ListData.cshtml"),
                }, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(tuNgay) && !string.IsNullOrEmpty(denNgay))
            {

                var ngaydang = HelperDateTime.ConvertDate(tuNgay);
                var ngayket = HelperDateTime.ConvertDate(denNgay);
                if (ngaydang > ngayket)
                {
                    Session["Messenger"] = new Notified { Value = EnumNotifield.Error, Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" };
                }
            }
            var qProcedureID = 14;
            var lstqProcedure = _qProcedureRepository.GetAll().ToList();
            if(lstqProcedure != null && lstqProcedure.Count > 0)
                qProcedureID = lstqProcedure.Where(x => x.isPublish).Select(x => x.ID).FirstOrDefault();
            // phan trang store procedure
            var lstNews = new List<tbl_News_Custom>();
            var condition = " WHERE Status = "+ qProcedureID;
            if (!string.IsNullOrEmpty(keySearch))
            {
                var fieldSearch = "Title";
                if (luaChon == 2)
                    fieldSearch = "Details";
                if (luaChon == 3)
                    fieldSearch = "Author";
                if (luaChon == 4)
                    fieldSearch = "SourceNews";
                if(tuychon == 1) // tim chinh xac
                    condition += " And (LOWER(" + fieldSearch + ") like N^*^" + keySearch.Trim().ToLower() + "^-^ OR LOWER(" + fieldSearch + ") like N^*^" + HelperString.UnsignCharacter(keySearch.Trim().ToLower()) + "^-^)";
                else // tim gan dung
                    condition += " And (LOWER(" + fieldSearch + ") like N^*^" + keySearch.Trim().ToLower() + "^-^ OR LOWER(" + fieldSearch + ") like N^*^" + HelperString.UnsignCharacter(keySearch.Trim().ToLower()) + "^-^)";
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
            var fullCatIdStr = "";
            if (!string.IsNullOrEmpty(categoryId))
            {
                var allCat = _cateRepository.GetAll();
                var fullCatId = Common.GetChild(allCat, Convert.ToInt32(categoryId));
                fullCatIdStr = string.Join(",", fullCatId);
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
            //var lstObj = _newsRepository.GetAll().Where(g => g.IsPublish && g.CreatedDate.Date <= DateTime.Now.Date && g.LangCode == langCode).ToList(); 

            //if (!string.IsNullOrEmpty(keySearch))
            //{
            //    bool checkspace = (keySearch.Trim().Contains(' ') ? true : false);
            //    if (!checkspace) tuychon = 1;

            //    if (tuychon == 1)
            //    {
            //        if (luaChon == 0)
            //        {
            //            lstObj = lstObj.Where(g => HelperString.UnsignCharacter(g.Title).Trim().ToLower().Contains(HelperString.UnsignCharacter(keySearch).Trim().ToLower()) ||
            //                         HelperString.UnsignCharacter(g.Details).Trim().ToLower().Contains(HelperString.UnsignCharacter(keySearch).Trim().ToLower())).ToList();
            //        }
            //        if (luaChon == 1)
            //        {
            //            lstObj = lstObj.Where(g => g.Title != null && HelperString.UnsignCharacter(g.Title).Trim().ToLower().Contains(HelperString.UnsignCharacter(keySearch).Trim().ToLower())).ToList();
            //        }
            //        if (luaChon == 2)
            //        {
            //            lstObj = lstObj.Where(g => g.Details != null && HelperString.UnsignCharacter(g.Details).Trim().ToLower().Contains(HelperString.UnsignCharacter(keySearch).Trim().ToLower())).ToList();
            //        }
            //        if (luaChon == 3)
            //        {
            //            lstObj = lstObj.Where(g => g.Author != null && HelperString.UnsignCharacter(g.Author).Trim().ToLower().Contains(HelperString.UnsignCharacter(keySearch).Trim().ToLower())).ToList();
            //        }
            //        if (luaChon == 4)
            //        {
            //            lstObj = lstObj.Where(g => g.SourceNews != null && HelperString.UnsignCharacter(g.SourceNews).Trim().ToLower().Contains(HelperString.UnsignCharacter(keySearch).Trim().ToLower())).ToList();
            //        }

            //    }
            //    else
            //    {
            //        if (luaChon == 0)
            //        {
            //            // var lstid = new List<int>();
            //            lstObj = lstObj.Where(g => (!String.IsNullOrEmpty(g.Title) && HelperString.UnsignCharacter(g.Title).Trim().ToLower().Split(' ').Intersect(HelperString.UnsignCharacter(keySearch).Trim().ToLower().Split(' ')).Count() == keySearch.Trim().Split(' ').Count()) || (!String.IsNullOrEmpty(g.Details) && HelperString.UnsignCharacter(g.Details).Trim().ToLower().Split(' ').Intersect(HelperString.UnsignCharacter(keySearch).Trim().ToLower().Split(' ')).Count() == keySearch.Trim().Split(' ').Count())).ToList(); 
            //        }
            //        if (luaChon == 1)
            //        {
            //            lstObj = lstObj.Where(g => !string.IsNullOrEmpty(g.Title) && HelperString.UnsignCharacter(g.Title).Trim().ToLower().Split(' ').Intersect(HelperString.UnsignCharacter(keySearch).Trim().ToLower().Split(' ')).Count() == keySearch.Trim().Split(' ').Count()).ToList(); 
            //        }
            //        if (luaChon == 2)
            //        {
            //            lstObj = lstObj.Where(g => !string.IsNullOrEmpty(g.Details) && HelperString.UnsignCharacter(g.Details).Trim().ToLower().Split(' ').Intersect(HelperString.UnsignCharacter(keySearch).Trim().ToLower().Split(' ')).Count() == keySearch.Trim().Split(' ').Count()).ToList(); 
            //        }
            //        if (luaChon == 3)
            //        {
            //            lstObj = lstObj.Where(g => !string.IsNullOrEmpty(g.Author) && HelperString.UnsignCharacter(g.Author).Trim().ToLower().Split(' ').Intersect(HelperString.UnsignCharacter(keySearch).Trim().ToLower().Split(' ')).Count() == keySearch.Trim().Split(' ').Count()).ToList(); 
            //        }
            //        if (luaChon == 4)
            //        {
            //            lstObj = lstObj.Where(g => !string.IsNullOrEmpty(g.SourceNews) && HelperString.UnsignCharacter(g.SourceNews).Trim().ToLower().Split(' ').Intersect(HelperString.UnsignCharacter(keySearch).Trim().ToLower().Split(' ')).Count() == keySearch.Trim().Split(' ').Count()).ToList(); 
            //        }

            //    }

            //}
            TempData["ThongBao"] = notif;
            TempData["Key"] = keySearch;
            TempData["Count"] = totalNews;
            return Json(new
            {
                Success = true,
                viewContent = RenderViewToString("~/Views/Home/_ListData.cshtml", lstNews),
                totalPages = Math.Ceiling(((double)totalNews / pagesize))
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HomeBanner()
        {
            var file = Common.GetPath("~/Content/cate.json");
            var data = System.IO.File.ReadAllText(file);            
            var lstCate = JsonConvert.DeserializeObject<List<tbl_CategoryJson>>(data);
            
            var langCode = "vn";
            if(Session["langCodeHome"]!=null)
            {
                langCode = Session["langCodeHome"].ToString();
            }
            var banner = _advRepository.GetAll().FirstOrDefault(x => x.Position == "banner" && x.Status && x.LangCode == langCode);
            var bannerdautrang = _advRepository.GetAll().FirstOrDefault(x => x.Position == "bannerdautrang" && x.Status && x.LangCode == langCode); 
           
            TempData["Banner"] = banner;
            TempData["bannerdautrang"] = bannerdautrang;
            var lstCateMainMenu = lstCate.Where(g => g.cate.Active && g.cate.LangCode==langCode).OrderBy(x => x.cate.Ordering).ToList();
             
            var lstmagazineTitle = _magazineRespository.GetAll().Where(g => g.Status&&g.LangCode==langCode).OrderBy(g => g.Title).Select(g=>g.Title).Distinct().ToList();
            TempData["lstmagazineTitle"] = lstmagazineTitle;

            var lstmagazineYearFirst =
                _magazineRespository.GetAll().Where(g => g.Status&&g.LangCode == langCode).OrderBy(g => g.Year).Select(g=>g.Year).FirstOrDefault();
            TempData["lstmagazineYearFirst"] = lstmagazineYearFirst;
            var lstmagazineYearLast =
                _magazineRespository.GetAll().Where(g => g.Status&&g.LangCode==langCode).OrderByDescending(g => g.Year).Select(g=>g.Year).FirstOrDefault();
            TempData["lstmagazineYearLast"] = lstmagazineYearLast;

            return View(lstCateMainMenu);
        }


        public ActionResult Favicon()
        {
            var favicon = _advRepository.GetAll().FirstOrDefault(x => x.Position != null && x.Position.Split(',').Contains("favicon") && x.Status);
            return View(favicon);
        } 

        public List<PathWay> getPathWay()
        {
            int id = 0;
            var uri = HttpContext.Request.Url.AbsoluteUri;
            var objHomeMenu = _menuhomecategory.GetAll().FirstOrDefault(g => g.Url != null && uri.Contains(g.Url));
            if (objHomeMenu != null)
            {
                id = objHomeMenu.ID;
            }
            var lstPathWay = new List<PathWay>();
            var lstResult = FindParentMenu(lstPathWay, id);
            return lstResult;

        }
        public List<PathWay> FindParentMenu(List<PathWay> lstPathWay, int catid = 0)
        {
            if (catid == 0)
            {
                return lstPathWay;
            }
            else
            {
                var objMenu = _menuhomecategory.Find(catid);
                if (objMenu != null)
                {
                    var obj = new PathWay();
                    obj.Name = objMenu.Name;
                    obj.Link = objMenu.Url;
                    lstPathWay.Insert(0, obj);

                    lstPathWay = FindParent(lstPathWay, objMenu.ParentID);

                }
                return lstPathWay;
            }
        }

        public ActionResult PathWay()
        {
            var Module = Request.RequestContext.RouteData.Values["controller"].ToString();
            var Action = Request.RequestContext.RouteData.Values["action"].ToString();
            var listPathWay = new List<PathWay>();
            var obj = new PathWay();
            var uri = HttpContext.Request.Url.AbsoluteUri;
            var objHomeMenu = _menuhomecategory.GetAll().FirstOrDefault(g => g.Url != null && g.Url.Trim() == uri.Trim());
            if (objHomeMenu != null)
            {
                listPathWay = getPathWay();
            }
            else
            {
                switch (Module.ToLower())
                {
                    case "home":
                        if (Action.ToLower() == "search")
                        { 
                            obj.Name = ConfigTextController.GetValueCFT("ketquatimkiem");
                            obj.Link = "/pages/search.html?key=";
                            obj.Active = "active";
                            listPathWay.Add(obj);
                        }
                        break;
                    case "category":
                    case "news":
                        if (!string.IsNullOrEmpty(Request["keyword"]))
                        {
                            obj.Name = ConfigTextController.GetValueCFT("timkiem");
                            obj.Active = "active";
                            listPathWay.Add(obj);
                            break;
                        }
                        else
                        {
                            listPathWay = getAllPathWayHome();
                            break;
                        }

                    case "image":
                        obj.Name = @ConfigTextController.GetValueCFT("thuvienanh");
                        obj.Link = "/pages/images.html";
                        obj.Active = "active";
                        listPathWay.Add(obj);
                        break;
                    case "video":
                        obj.Name = @ConfigTextController.GetValueCFT("video");
                        obj.Link = "/pages/video.html";
                        obj.Active = "active";
                        listPathWay.Add(obj);
                        break;
                    case "sitemap":
                        obj.Name = @ConfigTextController.GetValueCFT("sodowebsite");
                        obj.Link = "/pages/so-do-cong.html";
                        obj.Active = "active";
                        listPathWay.Add(obj);
                        break;
                    case "topic":
                        obj.Name = @ConfigTextController.GetValueCFT("topic");
                        obj.Link = "#";
                        obj.Active = "active";
                        listPathWay.Add(obj);
                        break;
                    case "magazine":
                        obj.Name = @ConfigTextController.GetValueCFT("magazine");
                        obj.Link = "/pages/tap-chi.html";
                        obj.Active = "active";
                        listPathWay.Add(obj);
                        break;
                    case "emagazine":
                        obj.Name = @ConfigTextController.GetValueCFT("emagazine","E - Magazine");
                        obj.Link = "/pages/eMagazine.html";
                        obj.Active = "active";
                        listPathWay.Add(obj);
                        break;

                }
            }

            return View(listPathWay);
        }
        public ActionResult Hotnewrights()
        {
            // anh ben phai
            var ImgRight = _slideImagesRepository.GetAll().Where(g => g.Status && g.Type == (int)Webconfig.SlideImages.Phai1).OrderBy(g => g.Ordering).ToList();
            ViewBag.ImgRight = ImgRight;
            // video trang chu
            var limit_video = Convert.ToInt32(ConfigTextController.GetValueCFT("limit_video"));
            //var homeVideo = _videoRepository.GetAll().Where(g => g.Status && g.IsHome).OrderBy(g => g.Ordering).ToList();
            //ViewBag.homeVideo = homeVideo;
            // album hien thi trang chu 
            //
            var limit_tinnoibat = Convert.ToInt32(ConfigTextController.GetValueCFT("limit_tinnoibat"));
           // var lstmodel = _newsRepository.GetAll().Where(g => g.Type == 1).OrderByDescending(s => s.CreatedDate).ToList().Take(limit_tinnoibat);
            return PartialView();
        }
        public List<PathWay> getAllPathWayHome()
        {
            var Module = Request.RequestContext.RouteData.Values["controller"].ToString();
            var Action = Request.RequestContext.RouteData.Values["action"].ToString();
            int cid = 0;
            if (Module == "Category" && (Action == "Index"))
            {
                if (Request.RequestContext.RouteData.Values["id"] != null)
                    cid = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"].ToString());
                if (!string.IsNullOrEmpty(Request["id"]))
                    cid = Convert.ToInt32(Request["id"]);
            }
            if (Module == "News" && Action == "Detail")
            { 
                var catid = Request.RequestContext.RouteData.Values["catid"];
                try
                {
                    cid = Convert.ToInt32(catid); 
                }
                catch (Exception e)
                {
                    cid = 0;
                }

                if (cid == 0)
                {
                    int id = Convert.ToInt32(Request["id"]);
                    if (Request.RequestContext.RouteData.Values["id"] != null)
                        id = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"].ToString());
                    var objNews = _newsRepository.Find(id);
                    if (objNews != null && !String.IsNullOrEmpty(objNews.CategoryIdStr))
                    {
                        var arrID = objNews.CategoryIdStr.Split(',');
                        cid = Convert.ToInt32(arrID[0]);
                    }
                }
            }
            var lstPathWay = new List<PathWay>();
            var lstResult = FindParent(lstPathWay, cid);
            return lstResult;

        }
        public List<PathWay> getAllPathWayNews()
        {
            var Action = Request.RequestContext.RouteData.Values["action"].ToString();
            int cid = 0;
            if (Action == "GetAllNewsByCat")
            {
                if (Request.RequestContext.RouteData.Values["catid"] != null)
                    cid = Convert.ToInt32(Request.RequestContext.RouteData.Values["catid"].ToString());
                if (!string.IsNullOrEmpty(Request["catid"]))
                    cid = Convert.ToInt32(Request["catid"]);
            }
            if (Action == "Details")
            {
                int id = Convert.ToInt32(Request["id"]);
                if (Request.RequestContext.RouteData.Values["id"] != null)
                    id = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"].ToString());
                var objNews = _newsRepository.Find(id);
                //if (objNews != null)
                //{
                //    cid = objNews.CategoryId;
                //}
            }

            var lstPathWay = new List<PathWay>();
            var lstResult = FindParent(lstPathWay, cid);

            return lstResult;

        } 
        public List<PathWay> FindParent(List<PathWay> lstPathWay, int catid = 0,int catroot = 0)
        { 
            if (catid == 0)
            {
                return lstPathWay;
            }
            else
            {
                int root = 0;
                if (catroot > 0) root = catroot;
                else root = catid;

                var objCategory = _cateRepository.Find(catid); 

                if (objCategory != null)
                { 
                    if (objCategory.ParentID == 0)
                    {
                        var allCat = _cateRepository.GetAll();
                        var lstCate = allCat.Where(g => g.ParentID == 0 && g.ID == catid && g.Active).ToList();
                        var lstCateCon = allCat.Where(g => g.ParentID == objCategory.ID && g.Active).ToList();
                        if (lstCateCon.Count > 0)
                            lstCate.AddRange(lstCateCon);
                        if (lstCate != null)
                        {
                            foreach (var item in lstCate)
                            {
                                var obj = new PathWay();
                                obj.Name = item.Name;
                                obj.Link = "/pages/category/" + item.ID + "/" + HelperString.RemoveMark(item.Name) + ".html";
                                lstPathWay.Insert(0, obj);
                                if (item.ID == root) obj.Active = "active";
                                else obj.Active = "";
                            }
                        }
                    }
                    else
                    {
                        lstPathWay = FindParent(lstPathWay, objCategory.ParentID, root);
                    } 
                }
                return lstPathWay;
            }
        }
        protected void Application_Access()
        {
            var Nowday = DateTime.Now;
            var SessionID = HttpContext.Session.SessionID;
            try
            {
                var modelsession = _accessOnlineRespository.Find(SessionID);
                var modelaccess = _accessViewRespository.GetAll().FirstOrDefault();
                if (modelaccess != null)
                {
                    if (modelsession == null)
                    {
                        var objsession = new tbl_AccessOnline();
                        objsession.Session = SessionID;
                        objsession.Time = Nowday;
                        _accessOnlineRespository.Add(objsession);
                        modelaccess.TongCong = modelaccess.TongCong + 1;
                        // kiem tra thang
                        if (Nowday.Month == modelaccess.NgayThongKe.Month)
                        {
                            modelaccess.ThangNay = modelaccess.ThangNay + 1;
                        }
                        else
                        {
                            modelaccess.ThangTruoc = modelaccess.ThangNay;
                            modelaccess.ThangNay = 1;
                        }
                        // kiem tra tuan
                        var startOfWeek = DateTime.Today.AddDays(-1 * (int)(Nowday.DayOfWeek));
                        var startDBWeek = modelaccess.NgayThongKe.AddDays(-1 * (int)(modelaccess.NgayThongKe.DayOfWeek));
                        if (startOfWeek.Date == startDBWeek.Date)
                        {
                            modelaccess.TuanNay = modelaccess.TuanNay + 1;
                        }
                        else
                        {
                            modelaccess.TuanTruoc = modelaccess.TuanNay;
                            modelaccess.TuanNay = 1;
                        }
                        // kiem tra ngay
                        if (Nowday.Date == modelaccess.NgayThongKe.Date)
                        {
                            modelaccess.HomNay = modelaccess.HomNay + 1;
                        }
                        if (Nowday.Date != modelaccess.NgayThongKe.Date)
                        {
                            modelaccess.HomTruoc = modelaccess.HomNay;
                            modelaccess.HomNay = 1;
                        }
                        modelaccess.NgayThongKe = Nowday;
                        _accessViewRespository.Edit(modelaccess);
                    }
                }
                else
                {
                    if (modelsession == null)
                    {
                        var objsession = new tbl_AccessOnline();
                        objsession.Session = SessionID;
                        objsession.Time = Nowday;
                        _accessOnlineRespository.Add(objsession);
                    }
                    var modelaccess1 = new tbl_AccessView();
                    modelaccess1.TongCong = 1;
                    modelaccess1.TuanTruoc = 1;
                    modelaccess1.TuanNay = 1;
                    modelaccess1.NgayThongKe = DateTime.Now;
                    modelaccess1.ThangNay = 1;
                    modelaccess1.ThangTruoc = 1;
                    modelaccess1.HomNay = 1;
                    modelaccess1.HomTruoc = 1;
                    _accessViewRespository.Add(modelaccess1);
                }
                var lstmodelseesion = _accessOnlineRespository.GetAll().Where(s => (s.Time.Minute < Nowday.Minute - 10 && s.Time.Hour == Nowday.Hour)|| (s.Time.Hour < Nowday.Hour && Nowday.Minute>=10) || s.Time.Month<Nowday.Month || s.Time.Date < Nowday.Date||s.Time.Year<Nowday.Year);
                foreach (var model in lstmodelseesion)
                {
                    _accessOnlineRespository.Delete(model);
                }
            }
            catch (Exception)
            {

            }
        }
        internal static string GetLocalIPv4()
        {
            string output = "";

            var HostEntry = System.Net.Dns.GetHostEntry((Dns.GetHostName()));
            if (HostEntry.AddressList.Length > 0)
            {
                foreach (var ip in HostEntry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        output = ip.ToString();
                    }
                }
            }

            return output;
        }
        public ActionResult ViewFile(string linkdown)
        {
            TempData["linkdown"] = linkdown;
            return View();
        }
        public ActionResult HomeFooter()
        { 
            var social = _advRepository.GetAll().Where(x => x.Position != null && x.Position.Split(',').Contains("associate") && x.Status).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            TempData["Banner"] = social;

            var file = Common.GetPath("~/Content/Json/footer.json");
            var data = System.IO.File.ReadAllText(file);
            var footer = JsonConvert.DeserializeObject<tbl_Footer>(data);   
            return View(footer);
        }
        public ActionResult HomeHeader()
        {
            if (Session["PC"] == null)
            {
                Session["PC"] = "0";
            } 
            var Module = Request.RequestContext.RouteData.Values["controller"].ToString();
            var Action = Request.RequestContext.RouteData.Values["action"].ToString();
            if (Module.ToLower() == "news" && Action.ToLower() == "detail")
            {
                var id = Request.RequestContext.RouteData.Values["id"];
                var objNews = _newsRepository.Find(Convert.ToInt32(id));
                TempData["objNews"] = objNews;
            }
            else
            {
                TempData["objNews"] = null;
            }
            return View();
        }
        public ActionResult menuLanguages()
        {
            var lstLanguages = _languagesRep.GetAll().ToList();
            TempData["lstLanguages"] = lstLanguages;
            return View();
        }
        public ActionResult ChangeLanguage(string languages)
        {
            Session["langCodeHome"] = "vn";
            if (!string.IsNullOrEmpty(languages))
            {
                Session["langCodeHome"] = languages;
            }
            return Json(new
            {
                IsSuccess = true,
                Messages = string.Format("Bạn đã chọn ngôn ngữ tiếng anh để nhập dữ liệu")
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangMode(string value)
        {
            if (value == "1") Session["PC"] = "0";
            else Session["PC"] = "1";
            return Json(new
                {
                    IsSuccess = true,
                }, JsonRequestBehavior.AllowGet);
        }
    }
}
