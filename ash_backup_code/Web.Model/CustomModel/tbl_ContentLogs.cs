using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model.CustomModel
{
    public class tbl_ContentLogs
    {
        public Guid ID { get; set; }
        public int ParentID { get; set; }
        public string Contents { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionTime { get; set; }
        public bool isLogin { get; set; }
        public string BrowserType { get; set; }
        public string DeviceType { get; set; }
    }
}
