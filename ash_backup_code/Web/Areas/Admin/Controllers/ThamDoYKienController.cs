using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class ThamDoYKienController : BaseController
    {
        IThamDoYKienRepository _thamDoYKien = new ThamDoYKienRepository();
        IThamDoYKienIpRepository _thamDoYKienIpRepository = new ThamDoYKienIpRepository();
        // GET: /Admin/ThamDoYKien/

        public ActionResult Index()
        {

            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData()
        {
            var lstThamDoYKien = _thamDoYKien.GetAll().OrderBy(x => x.Ordering).ThenByDescending(x => x.ID);
            foreach (var item in lstThamDoYKien)
            {
                var obj = _thamDoYKien.GetAll().FirstOrDefault(x => x.ID == item.ParentID);
                if (obj != null)
                {
                    item.ParentName = obj.Name;
                }
            }

            var lstLevel = Common.CreateLevel(lstThamDoYKien.ToList());
            return Json(new
            {
                contentView = RenderViewToString("~/Areas/Admin/Views/ThamDoYKien/ListData.cshtml", lstLevel)
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstThamDoYKien = _thamDoYKien.GetAll().Where(x => x.Status).ToList();
            var lstLevel = Common.CreateLevel(lstThamDoYKien.ToList());
            TempData["lstLevel"] = lstLevel;
            return Json(RenderRazorViewToString("~/Areas/Admin/Views/ThamDoYKien/Add.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_ThamDoYKien obj)
        {
            try
            {
                var Ordering = Convert.ToInt32(Request["Ordering"]);
                if (Ordering < 0)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("thứ tự không được âm ")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _thamDoYKien.Add(obj);
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = string.Format("Thêm mới thành công")
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới không  thành công")
                }, JsonRequestBehavior.AllowGet);

            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var obj = _thamDoYKien.Find(id);
            var lstThamDoYKien = _thamDoYKien.GetAll().Where(x => x.Status).ToList();
            var lstLevel = Common.CreateLevel(lstThamDoYKien.Where(x => x.ID != id).ToList());
            TempData["lstLevel"] = lstLevel;
            return Json(RenderViewToString("~/Areas/Admin/Views/ThamDoYKien/Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_ThamDoYKien obj)
        {
            try
            {
                var Ordering = Convert.ToInt32(Request["Ordering"]);
                if (Ordering < 0)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("thứ tự không được âm ")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _thamDoYKien.UpDate(obj);
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = string.Format("Cập nhật thành công")
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật không thành công")
                }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var lstThamDoYKien = _thamDoYKien.GetAll().ToList();
                //lấy ra tất cả các thằng con
                var lstChild = Common.GetChild(lstThamDoYKien, id).Distinct();

                if (lstChild != null)
                {
                    foreach (var item in lstChild)
                    {
                        var lstPoll = _thamDoYKien.GetAll().Where(x => x.ID == item).ToList();
                        foreach (var item2 in lstPoll)
                        {
                            var lstPollParent = _thamDoYKien.GetAll().Where(x => x.ID == item2.ParentID).ToList();
                            foreach (var item3 in lstPollParent)
                            {
                                item3.Poll = item3.Poll - item2.Poll;
                            }
                        }
                        _thamDoYKien.Delete(item);
                    }
                }
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = string.Format("Xóa thành công")

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa thất bại")

                }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteAll(string lstID)
        {
            var arrID = lstID.Split(',');
            int count = 0;


            foreach (var item in arrID)
            {
                try
                {
                    var lstThamDoYKien = _thamDoYKien.GetAll().ToList();
                    var lstChild = Common.GetChild(lstThamDoYKien, Convert.ToInt32(item)).Distinct();
                    foreach (var item2 in lstChild)
                    {
                        var lstPoll = _thamDoYKien.GetAll().Where(x => x.ID == item2).ToList();
                        foreach (var item3 in lstPoll)
                        {
                            var lstPollParent = _thamDoYKien.GetAll().Where(x => x.ID == item3.ParentID).ToList();
                            foreach (var item4 in lstPollParent)
                            {
                                item4.Poll = item4.Poll - item3.Poll;
                            }
                        }

                        _thamDoYKien.Delete(item2);
                        count++;
                    }
                    _thamDoYKien.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {

                    continue;
                }

            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục", count)
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UpdatePosition(string value)
        {
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {

                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = _thamDoYKien.Find(Convert.ToInt32(id));
                try
                {
                    if (Convert.ToInt32(pos) < 0)
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            Messenger = string.Format("vị trí không được âm")
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        obj.Ordering = Convert.ToInt32(pos);
                        _thamDoYKien.UpDate(obj);
                    }


                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật vị trí thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HomeThamDoYKien()
        {
            var abc = HttpContext.Session.SessionID;
            var lstThamDoYKien = _thamDoYKien.GetAll().Where(x => x.Status).OrderBy(x=>x.Ordering).ThenByDescending(x=>x.ID).ToList();
            TempData["lstThamDoYKien"] = lstThamDoYKien;
            Session["BinhChon"] = HttpContext.Session.SessionID;
            var lstIp = _thamDoYKienIpRepository.GetAll().ToList();
            var lstPollId = _thamDoYKien.GetAll().Where(x => x.ParentID == 0).ToList();
            string KhongBinhChon = "";
            foreach (var item1 in lstPollId)
            {
                var tt = 0;
                foreach (var item2 in lstIp)
                {
                    if(item1.ID == item2.PollId && item2.SessionID == Session["BinhChon"].ToString())
                    {
                        tt++;
                        if(tt != 1)
                            KhongBinhChon += ",";
                        KhongBinhChon += item1.ID;
                    }
                }
            }
            ViewBag.KhongBinhChon = KhongBinhChon;
            //kiểm tra tất cả các có Ip cha vừa có Ipcha =pollID , và 
            //foreach (var item in lstIp)
            //{

            //    if(item.IpLocal== ipLocal && item.IpPublic== ipPublic)
            //    {
            //        ViewBag.hideButton = 1;
            //    }
            //}
            //string KhongBinhChon = "";
            //var lstPollID = _thamDoYKien.GetAll().Where(x => x.ParentID == 0).ToList();
            //foreach (var item in lstPollID)
            //{

            //    if (item.IpMay!=null && item.IpMay.ToString().Contains(Ip))
            //    {
            //        KhongBinhChon += item.ID;
            //    }

            //}
            //ViewBag.KhongBinhChon = KhongBinhChon;

            //if (Session["BinhChon"] != null)
            //{
            //    var sessionid = HttpContext.Session.SessionID;
            //    var lstbinhchon = Session["BinhChon"].ToString().Split(',');
            //    if (lstbinhchon.Contains(sessionid))
            //        ViewBag.AnNUT = 1;
            //}
            return View("~/Areas/Admin/Views/ThamDoYKien/HomeThamDoYKien.cshtml");

        }
        public ActionResult BinhChon(int id, int pollid)
        {
            try
            {
                Session["BinhChon"] = HttpContext.Session.SessionID;
                var objIP = new tbl_ThamDoYKien_Ip();
                objIP.SessionID = Session["BinhChon"].ToString();
                objIP.PollId = pollid;
                _thamDoYKienIpRepository.Add(objIP);

                //kiểm tra binh chọn bằng IP của máy
               // var objPollId = _thamDoYKien.Find(pollid);
                //var IP = GetLocalIPv4();
                //if(objPollId.IpMay == null)
                //{
                //    objPollId.IpMay = IP;
                //}
                //else
                //{
                //    var lstIpMay = objPollId.IpMay.ToString().Split(',');
                //    if (!lstIpMay.Contains(IP))
                //        objPollId.IpMay += "," + IP;
                //}

                //_thamDoYKien.UpDate(objPollId);

                //if (Session["BinhChon_"+ pollid] == null)
                //{
                //    Session["BinhChon_" + pollid] = HttpContext.Session.SessionID;
                //}
                //else
                //{
                //    var sessionid = HttpContext.Session.SessionID;
                //    var lstBinhChon = Session["BinhChon_"+ pollid].ToString().Split(',');
                //    if (!lstBinhChon.Contains(sessionid))
                //        Session["BinhChon_"+ pollid] += "," + sessionid;
                //}

                var obj = _thamDoYKien.Find(id);
                if (obj != null)
                {
                    obj.Poll++;
                    _thamDoYKien.UpDate(obj);
                }
                var objPoll = _thamDoYKien.Find(pollid);
                if (objPoll != null)
                {
                    objPoll.Poll++;
                    _thamDoYKien.UpDate(objPoll);
                }

                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Bình chọn thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Bình chọn thất bại ",
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult KetQuaBinhChon(int id)
        {
            var obj = _thamDoYKien.Find(id);
            var lstThamDoYKien = _thamDoYKien.GetAll().Where(g => g.ParentID == id).ToList();
            foreach (var item in lstThamDoYKien)
            {
                var pct = ((float)item.Poll / obj.Poll) * 100;
                item.Percent = pct.ToString();
            }
            TempData["lstKetQua"] = lstThamDoYKien;
            return Json(new
            {
                lstKetQua = lstThamDoYKien,
                ParentName = obj.Name
            }, JsonRequestBehavior.AllowGet);
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        //hàm lấy địa chỉ ip public 
        public static string PublicIPAddress()
        {
            string uri = "http://checkip.dyndns.org/";
            string ip = String.Empty;

            using (var client = new HttpClient())
            {
                var result = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;

                ip = result.Split(':')[1].Split('<')[0];
            }

            return ip;
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

    }
}
