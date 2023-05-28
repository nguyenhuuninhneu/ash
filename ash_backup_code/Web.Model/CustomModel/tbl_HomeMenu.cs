using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model
{
    public partial class tbl_HomeMenu
    {
        public string ParentName { get; set; }
        public int Level { get; set; }

        public List<view_NEWS_LISTDATA> lstNews {get; set;}
    }
}
