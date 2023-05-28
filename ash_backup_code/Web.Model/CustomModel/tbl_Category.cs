using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model
{
    public partial class tbl_Category
    {
        public string ParentName { get; set; }
        public int Level { get; set; }
        public string NgonNgu { get; set; }
        public List<tbl_News_Custom> LstNews { get; set; }
    }
}
