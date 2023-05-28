using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using Web.BaseSecurity;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Index")]
    public class BackupDatabaseController : BaseController
    {
        //
        // GET: /Admin/BackupDatabase/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Backup()
        {
            var filePathBackupSql = ConfigurationManager.AppSettings["FilePathBackupSQL"];
            if (!Directory.Exists(filePathBackupSql))
            {
                Directory.CreateDirectory(filePathBackupSql);
            }
            
            var sqlConnectionString = ConfigurationManager.AppSettings["ConnectionStringBackupSQL"];
            var conn = new SqlConnection(sqlConnectionString);
            try
            {
                conn.Open();
                var dbName = conn.Database;
                
                var command = new SqlCommand(@"backup database " + string.Format("[{0}]", dbName) + " to disk ='" + filePathBackupSql + "\\" + string.Format("{0}.bak", dbName) + "' with init,stats=10", conn)
                {
                    CommandTimeout = 300
                };
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Backup thất bại",
                }, JsonRequestBehavior.AllowGet);
            }

            LogController.AddLog(string.Format("Backup database"), User.Username);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Backup thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Restore()
        {
            var filePathBackupSql = ConfigurationManager.AppSettings["FilePathBackupSQL"];
            var sqlConnectionString = ConfigurationManager.AppSettings["ConnectionStringBackupSQL"];
            var conn = new SqlConnection(sqlConnectionString);
            try
            {
                conn.Open();
                var dbName = conn.Database;
                filePathBackupSql = filePathBackupSql + dbName + ".bak";
                if (!System.IO.File.Exists(filePathBackupSql))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Chưa có file backup",
                    }, JsonRequestBehavior.AllowGet);
                }
                var command = new SqlCommand(@"USE MASTER; ALTER DATABASE "+dbName+" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;"+
"RESTORE DATABASE "+dbName+" FROM DISK = '"+filePathBackupSql+"' WITH REPLACE ALTER DATABASE "+dbName+" SET MULTI_USER", conn);
                command.ExecuteReader();
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Restore thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
            LogController.AddLog(string.Format("Restore database"), User.Username);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Restore thành công",
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
