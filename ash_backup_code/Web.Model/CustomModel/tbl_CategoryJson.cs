using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model
{
    public partial class tbl_CategoryJson
    {
        public tbl_Category cate { get; set; }
        public List<tbl_CategoryJson> listChildren { get; set; }       
    }
}
