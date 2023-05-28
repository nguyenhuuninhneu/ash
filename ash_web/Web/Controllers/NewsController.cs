using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class NewsController : BaseController
    { 
        readonly IImageCategoryRepository _imgCateRepository = new ImageCategoryRepository(); 
        readonly ICategoryRepository _cateRepository = new CategoryRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        readonly IHomeMenuRepository _categoryRepository = new HomeMenuRepository(); 
        readonly IqProcedureRepository _quyTrinhXuatBanRepository = new qProcedureRepository();
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository(); 
        readonly ILimitRepository _limitRepository = new LimitRepository();

        public ActionResult HomeNew()
        {
            var objNews = _newsRepository.GetAll().Where(g => g.Type == 1 && g.CreatedDate.Date <= DateTime.Now.Date).OrderByDescending(g => g.CreatedDate).Take(8)
                .ToList();
            // lay trang thai xuat ban
            var tblQuyTrinhXuatban = _quyTrinhXuatBanRepository.GetAll().FirstOrDefault(g => g.isPublish);
            if (tblQuyTrinhXuatban != null)
            {
                var ttXuatBan = tblQuyTrinhXuatban.ID;
                objNews = objNews.Where(g => g.Status == ttXuatBan).ToList();
            }
            //
            return View(objNews);
        }
           
        //Tin tức
        public ActionResult NewsPart1()
        {
            var file = Common.GetPath("~/Content/Json/lstNewHome1.json");
            var data = System.IO.File.ReadAllText(file);
            var lstNewHome1 = JsonConvert.DeserializeObject<List<tbl_News_Custom>>(data);
            ILimitRepository _limitRepository = new LimitRepository();
            var limithot1 = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code.Trim() == "tin-tuc-hot-tren-slide"); 
            var limithot1value = (limithot1 != null ? limithot1.Value : 20);
            TempData["lstNewHome1"] = lstNewHome1.Where(g=>g.CreatedDate <= DateTime.Now).OrderByDescending(g=>g.CreatedDate).Take(limithot1value).ToList();

            var file1 = Common.GetPath("~/Content/Json/lstNewHome2.json");
            var data1 = System.IO.File.ReadAllText(file1);
            var lstNewHome2 = JsonConvert.DeserializeObject<List<tbl_News_Custom>>(data1);
            var limithot2 = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code.Trim() == "tin-tuc-hot-slide");
            var limithot2value = (limithot2 != null ? limithot1.Value : 4);
            lstNewHome2 = lstNewHome2.Where(g => g.CreatedDate <= DateTime.Now).OrderByDescending(g => g.CreatedDate)
                .Take(limithot2value).ToList();
            return View(lstNewHome2);
        }

        public ActionResult NewsPart2()
        {
            var allCat = _cateRepository.GetAll();
            var arrayCount = new[] { 1, 2, 3 };
            var limit = _limitRepository.GetAll().ToList();
            int pagesize;
            for (int i = 1; i <= arrayCount.Length; i++)
            { 
                var limit1 = limit.FirstOrDefault(g => g.Code == "tin-trang-chu-loai-" + i);
                var lstCateHome1 = allCat.Where(g => g.Position != null && g.Position.Split(',').Contains("category_center" + i) && g.Active).OrderBy(x => x.Ordering).ToList();
                if (lstCateHome1 != null)
                {
                    pagesize = (limit1 != null ? limit1.Value : 5);
                    foreach (var item in lstCateHome1)
                    {
                        string strFileName = "~/Content/Json/" + item.ID + ".json";
                        var file = Common.GetPath(strFileName);
                        var data = System.IO.File.ReadAllText(file);
                        var LstNews = JsonConvert.DeserializeObject<List<tbl_News_Custom>>(data);
                        item.LstNews = LstNews.Where(g => g.CreatedDate <= DateTime.Now).OrderByDescending(g => g.CreatedDate).Take(pagesize).ToList();
                    }
                }
                TempData["CateHome" + i] = lstCateHome1;
            } 
            return View();
        }

        public ActionResult BanCanBiet()
        { 
            var file = Common.GetPath("~/Content/Json/bancanbiet.json");
            var data = System.IO.File.ReadAllText(file);
            var lstNewRight1 = JsonConvert.DeserializeObject<List<tbl_News_Custom>>(data);

            var langCode = "vn";
            if (Session["langCodeHome"] != null)
            {
                langCode = Session["langCodeHome"].ToString();
            } 
            var objcate = _cateRepository.GetAll().FirstOrDefault(g =>g.Position != null && g.Position.Split(',').Contains("category_right") && g.Active && g.LangCode == langCode);

            var objlimit = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code == "ban-can-biet");
            var pagesize = (objlimit != null ? objlimit.Value : 5);
            TempData["lstNewRight1"] = lstNewRight1.Where(g=>g.CreatedDate < DateTime.Now).OrderByDescending(g=>g.CreatedDate).Take(pagesize).ToList(); 
            return View(objcate);
        }

        public ActionResult SuKienNoiBat()
        {
            var file = Common.GetPath("~/Content/Json/sukiennoibat.json");
            var data = System.IO.File.ReadAllText(file);
            var lstSKNB = JsonConvert.DeserializeObject<List<tbl_News_Custom>>(data);
            var objlimit_sknb = _limitRepository.GetAll().FirstOrDefault(g => g.Status && g.Code == "su-kien-noi-bat"); 
            var limitval = (objlimit_sknb != null ? objlimit_sknb.Value : 5);
            lstSKNB = lstSKNB.Where(g => g.CreatedDate < DateTime.Now).OrderByDescending(g => g.CreatedDate).Take(limitval).ToList();
            return View(lstSKNB);
        }

        public ActionResult Detail(int id = 0)
        { 

            var objNews = _newsRepository.Find(id);
            if (objNews == null)
                return Redirect("/"); 
            var otherNews = new List<tbl_News_Custom>();
            var Pagesize = 10;
            var lmitBVK = _limitRepository.GetAll().FirstOrDefault(g => g.Code == "bai-viet-khac");
            if (lmitBVK != null)
                Pagesize = lmitBVK.Value;

            var rqcatid = Request.RequestContext.RouteData.Values["catid"].ToString();
            var catid = 0;
            try
            {
                catid = Convert.ToInt32(rqcatid);
            }
            catch (Exception e)
            {
                if (!String.IsNullOrEmpty(objNews.CategoryIdStr))
                    catid = Convert.ToInt32(objNews.CategoryIdStr.Split(',')[0]);
                else catid = 0;
            }
            
            TempData["catid"] = catid.ToString();

            var allCat = _cateRepository.GetAll();
            var fullCatIdStr = "";
            if (catid > 0)
            {
                var fullCatId = Common.GetChild(allCat, catid);
                fullCatIdStr = string.Join(",", fullCatId);
                var condition = " WHERE 1=1 AND IsPublish = 'True' AND ID <> "+id+" AND CreatedDate <= GETDATE() AND CreatedDate <= '" + objNews.CreatedDate + "'";
                if (!string.IsNullOrEmpty(fullCatIdStr))
                {
                    condition += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
                }
                var orderby = "ORDER BY CreatedDate DESC";
                var field = "ID,Title,Photo,Description,CategoryIdStr,CreatedDate,IsPublish,CateComma";
                var DicLevel = _newsRepository.spGetListNews(1, Pagesize, field, "view_News_MultiCate", condition, orderby);
                if (DicLevel.Count > 0)
                {
                    var objDic = DicLevel.FirstOrDefault();
                    otherNews = objDic.Key;
                }
            }
            TempData["otherNews"] = otherNews;

            objNews.ViewCount = objNews.ViewCount + 1;
            _newsRepository.Edit(objNews);
            var lstcatImg = _imgCateRepository.GetAll().ToList();
            TempData["LstCateImg"] = lstcatImg;
            var lstNews = otherNews;
            TempData["lstNews"] = lstNews;
            var lstCateRight = _cateRepository.GetAll().Where(g => g.Position != null);
            if (lstCateRight != null)
            {
                lstCateRight = lstCateRight.Where(g => g.Position.Trim().Contains("category_right"))
                    .OrderBy(x => x.Ordering).ToList();
            }
            TempData["lstCateRight"] = lstCateRight;
            var detailNews = _newsRepository.Find(id);

            var lstRelated = new List<tbl_News_Custom>();
            if (objNews.RelatedNews != null)
            {
                var condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
                condition += " AND ID IN ('" + objNews.RelatedNews.Replace(",", "','") + "')";
                var orderby = "ORDER BY CreatedDate DESC";
                var field = "ID,Title,Photo,Description,CategoryIdStr,CreatedDate,IsPublish,CateComma";
                var DicLevel = _newsRepository.spGetListNews(1, 100, field,"view_News_MultiCate", condition, orderby);
                if (DicLevel.Count > 0)
                {
                    var objDic = DicLevel.FirstOrDefault();
                    lstRelated = objDic.Key;
                }
            }
            TempData["lstRelated"] = lstRelated;

            return View(detailNews);
        }

        //Kết thúc
        public ActionResult MenuCategory()
        {
            var Action = Request.RequestContext.RouteData.Values["action"].ToString();
            int catid = 0;
            if (Action == "GetAllNewsByCat")
            {
                if (Request.RequestContext.RouteData.Values["catid"] != null)
                    catid = Convert.ToInt32(Request.RequestContext.RouteData.Values["catid"].ToString());
                if (Request["catid"] != null)
                    catid = Convert.ToInt32(Request["catid"]);
            }
            if (Action == "Details")
            {
                int id = Convert.ToInt32(Request["id"]);
                if (Request.RequestContext.RouteData.Values["id"] != null)
                    id = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"].ToString());
                var objNews = _newsRepository.Find(id);
                //if (objNews != null)
                //{
                //    catid = objNews.CategoryId;
                //}
            }
            if (catid > 0)
            {
                var lstCategory = _categoryRepository.GetAll().ToList();
                var arrAllParentID = Common.GetAllParent(lstCategory, catid);
                if (arrAllParentID.Contains(30))
                {
                    var lstNoiBat = _newsRepository.GetAll().Where(g => g.Type == 2 && g.Status == 6 && g.CreatedDate <= DateTime.Now).Take(5).ToList();
                    ViewBag.lstNoiBat = lstNoiBat;

                    var lstXemnhieu = _newsRepository.GetAll().Where(g => g.Status == 6 && g.CreatedDate <= DateTime.Now)
                        .OrderByDescending(g => g.ViewCount).ThenByDescending(g => g.CreatedDate).Take(5).ToList();
                    ViewBag.lstXemnhieu = lstXemnhieu;
                }
                ViewBag.catid_act = 30;
            }
            var pid = _categoryRepository.Find(catid).ParentID;
            if (pid == 0)
                pid = catid;
            var pcat = _categoryRepository.Find(pid);
            ViewBag.pid = pid;
            ViewBag.pname = pcat.Name;
            var MenuCategory = _categoryRepository.GetAll().Where(g => g.ParentID == pid).ToList();
            return View(MenuCategory);
        }

        public void FindParentTreeCatForNews(tbl_Category objcat, List<tbl_Category> lstCatsCategories,
            ref List<tbl_Category> lstCategories)
        {
            lstCategories.Add(objcat);
            var parent = lstCatsCategories.FirstOrDefault(g => g.ID == objcat.ParentID);
            if (parent != null && parent.ID != 0)
            {
                FindParentTreeCatForNews(parent, lstCatsCategories, ref lstCategories);
            }
        }

        public void RenderTreeCat(tbl_HomeMenu parent, List<tbl_HomeMenu> lstCategories, ref string linkCat)
        {
            if (parent != null)
            {
                linkCat += "<li><a href='/News/GetAllNewsByCat?catid=" + parent.ID + "'>" + parent.Name +
                           "</a> <i class='glyphicon glyphicon-menu-right'></i></li>";
                var objChild = lstCategories.FirstOrDefault(g => g.ParentID == parent.ID);
                if (objChild != null)
                {
                    RenderTreeCat(objChild, lstCategories, ref linkCat);
                }
            }
        }

        public ActionResult Search(string keyword, int page = 1)
        {
            var allPage = _pageElementRepository.GetAll().ToList();
            ViewBag.allPage = allPage.ToList();

            //if (string.IsNullOrEmpty(keyword))
            //{
            //    return View((new List<view_NEWS_LISTDATA>()).ToPagedList(page, Webconfig.RowLimit));
            //}
            //var pageElementId = GetPageElementId();
            //var lstNews = _newsRepository.Search().Where(g=>g.PageElementId == pageElementId).ToList();

            var lstNews = _newsRepository.GetAll().Where(s => s.CreatedDate <= DateTime.Now).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                lstNews =
                    lstNews.Where(
                        g =>
                            (g.Title != null &&
                             HelperString.UnsignCharacter(g.Title)
                                 .ToLower()
                                 .Trim()
                                 .Contains(HelperString.UnsignCharacter(keyword).ToLower().Trim())) ||
                            (g.Description != null &&
                             HelperString.UnsignCharacter(g.Description)
                                 .ToLower()
                                 .Trim()
                                 .Contains(HelperString.UnsignCharacter(keyword).ToLower().Trim())) ||
                            (g.Tags != null &&
                             HelperString.UnsignCharacter(g.Tags)
                                 .ToLower()
                                 .Trim()
                                 .Contains(HelperString.UnsignCharacter(keyword).ToLower().Trim())) ||
                            (g.Details != null &&
                             HelperString.UnsignCharacter(g.Details)
                                 .ToLower()
                                 .Trim()
                                 .Contains(HelperString.UnsignCharacter(keyword).ToLower().Trim()))
                    ).ToList();
            }
            ViewBag.KeySearch = keyword;
            return View(lstNews.ToPagedList(page, Webconfig.RowLimit));
        }

        public ActionResult CommentManager(int id)
        {
            var allcomment = _newsRepository.GetAllCMT().Where(g => g.NewsID == id && g.Status).ToList();
            TempData["totalComment"] = allcomment.Count;
            TempData["id"] = id;
            return View();
        }

        public ActionResult CommentAjax(int id, int page = 1)
        {
            var allcomment = _newsRepository.GetAllCMT().Where(g => g.NewsID == id && g.Status).OrderByDescending(g => g.CreateDate).ToList();
            TempData["allcomment"] = allcomment;
            var lstcomment = allcomment.Where(g => g.CommentID == 0);
            var totalComment = lstcomment.ToList().Count;
            var rowLimit = 5;
            var lstcomments = lstcomment.Skip((page - 1) * rowLimit).Take(rowLimit);
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/News/CommentList.cshtml", lstcomments),
                totalPages = Math.Ceiling(((double)totalComment / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddComment(int idnews, int idcomment, string fullname, string email, string content, string imgcode)
        {
            var thongbao = "";
            var thongbaocapcha = "true";
            try
            {
                if (imgcode == Session["CaptchaImageText"].ToString())
                {
                    var objcmt = new tbl_NewsComment();
                    objcmt.CommentID = idcomment;
                    objcmt.NewsID = idnews;
                    objcmt.FullName = fullname;
                    objcmt.Email = email;
                    objcmt.NoiDung = content;
                    objcmt.CreateDate = DateTime.Now;
                    objcmt.Status = false;
                    objcmt.IsView = false;
                    objcmt.IsNews = true;
                    _newsRepository.AddCMT(objcmt);
                    thongbao = "success";
                }
                else
                {
                    thongbaocapcha = "false";
                }
            }
            catch (Exception e)
            {
                thongbao = "error";
            }

            return Json(new
            {
                thongbao,
                thongbaocapcha
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eMagazine()
        {
            return View();
        }

    }
}
