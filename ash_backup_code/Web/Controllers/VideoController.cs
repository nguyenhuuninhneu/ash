using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class VideoController : BaseController
    {
        //
        // GET: /Video/
        readonly IVideoRepository _videoRepository = new VideoRepository();
        readonly IFieldFileRep _fieldsFile = new FieldFileRep();
        readonly IVideoCategoryRepository _videoCategoryRepository = new VideoCategoryRepository();
        public ActionResult Index()
        {
            var langCode = "vn";
            if (Session["langCodeHome"] != null)
            {
                langCode = Session["langCodeHome"].ToString();
            }

            var lstvideo = _videoRepository.GetAll().Where(g => g.Status && g.LangCode == langCode).OrderBy(g => g.Ordering)
                .ThenByDescending(g => g.CreatedDate).ToList();
            foreach (var item in lstvideo.Where(p=>p.Type == 1))
            {
                //get file
                var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == item.ID && g.Code == "video");
                if (lstFile != null)
                {
                    item.Url = lstFile.LinkFile;
                }
            }
            TempData["lstvideo"] = lstvideo;

            var lstvideoHot = lstvideo.Where(g => g.IsHot).ToList();
            TempData["lstvideoHot"] = lstvideoHot;

            var lstcate = _videoCategoryRepository.GetAll().Where(g=>g.Status && g.LangCode == langCode).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            TempData["lstcate"] = lstcate; 

            return View();
        }
        public ActionResult Detail(int id = 0, int catid = 0)
        {
            var langCode = "vn";
            if (Session["langCodeHome"] != null)
            {
                langCode = Session["langCodeHome"].ToString();
            }

            var objvideo = new tbl_Video();
            var idshow = 0;
            if (catid > 0)
            {
                objvideo = _videoRepository.GetAll().Where(g => g.Status && g.VideoCategoryId == catid.ToString() && g.LangCode == langCode).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).FirstOrDefault();
              
                if (objvideo != null)
                {
                    idshow = objvideo.ID;
                    if (objvideo.Type == 1)
                    {
                        var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == objvideo.ID && g.Code == "video");
                        if (lstFile != null)
                        {
                            objvideo.Url = lstFile.LinkFile;
                        }
                    }
                }
            }
            else
            {
                idshow = id;
            }
            TempData["idshow"] = idshow;

            var objshow = _videoRepository.Find(idshow);
           

            var viewnum = objshow.ViewNumber;
            objshow.ViewNumber = viewnum + 1; 
            _videoRepository.Edit(objshow);
            if (objshow != null)
            {
                if (objshow.Type == 1)
                {
                    var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == objshow.ID && g.Code == "video");
                    if (lstFile != null)
                    {
                        objshow.Url = lstFile.LinkFile;
                    }
                }
            }
            TempData["objvideo"] = objshow;

            var otherVideo = _videoRepository.GetAll().Where(g => g.Status && g.ID != idshow && objshow.VideoCategoryId.Contains(g.VideoCategoryId) && g.LangCode == langCode).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate);
            foreach (var item in otherVideo.Where(p => p.Type == 1))
            {
                //get file
                var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == item.ID && g.Code == "video");
                if (lstFile != null)
                {
                    item.Url = lstFile.LinkFile;
                }
            }

            TempData["otherVideo"] = otherVideo.ToList(); 
             
            return View();
        }
        public ActionResult DetailAjax(string lstid,int idshow, int page = 1)
        { 
            var objshow = _videoRepository.Find(idshow);
            if (objshow != null)
            {
                if (objshow.Type == 1)
                {
                    var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == objshow.ID && g.Code == "video");
                    if (lstFile != null)
                    {
                        objshow.Url = lstFile.LinkFile;
                    }
                }
            }

            var langCode = "vn";
            if (Session["langCodeHome"] != null)
            {
                langCode = Session["langCodeHome"].ToString();
            }

            var otherVideo = _videoRepository.GetAll().Where(g => g.Status && g.ID != idshow && !lstid.Split(',').Contains(g.ID.ToString()) &&objshow.VideoCategoryId.Contains(g.VideoCategoryId) && g.LangCode == langCode).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate);
            foreach (var item in otherVideo.Where(p => p.Type == 1))
            {
                //get file
                var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == item.ID && g.Code == "video");
                if (lstFile != null)
                {
                    item.Url = lstFile.LinkFile;
                }
            }
            var totalVideo = otherVideo.ToList().Count;
            var rowLimit = 10; 
            var otherVideoz = otherVideo.Skip((page - 1) * rowLimit).Take(rowLimit); 
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/Video/DetailAjax.cshtml", otherVideoz),
                totalPages = Math.Ceiling(((double)totalVideo / rowLimit)), 
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Right(int right = 0)
        { 
            var file = Common.GetPath("~/Content/Json/Video.json");
            var data = System.IO.File.ReadAllText(file);
            var lstCate = JsonConvert.DeserializeObject<List<tbl_Video>>(data);

            var langCode = "vn";
            if(Session["langCodeHome"]!=null)
            {
                langCode = Session["langCodeHome"].ToString();
            }
            var lstVideo = lstCate.Where(g => g.Status && g.IsHome&&g.LangCode==langCode).OrderBy(g => g.Ordering).ThenByDescending(g=>g.CreatedDate).ToList();
            foreach (var item in lstVideo.Where(p => p.Type == 1))
            {
                //get file
                var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == item.ID && g.Code == "video");
                if (lstFile != null)
                {
                    item.Url = lstFile.LinkFile;
                }
            }
            if (lstVideo.Count == 0)
            {
                lstVideo = _videoRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            }
            lstVideo = lstVideo.Take(2).ToList();
            if(right == 1)
                return View("Right2",lstVideo);
            return View(lstVideo);
        }
    }
}
