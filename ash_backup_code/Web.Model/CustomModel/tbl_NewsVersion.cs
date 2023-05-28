using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model.CustomModel
{
    public class tbl_NewsVersion: tbl_News
    {
        public int Version { get; set; }
        public DateTime LastModifieDate { get; set; }
        public string LastModifieBy { get; set; }
    }
}
