using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model.CustomModel
{
    public class tbl_ConfigTextField
    {
        public Guid ID { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        public string ActionBy { get; set; }
        public DateTime ActionTime { get; set; }
        public string LangCode { get; set; }
        public string NameLangCode { get; set; }
        public string  NgonNgu { get; set; }
    }
}
