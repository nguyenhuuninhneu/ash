using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model
{
    public partial class tbl_Message
    { 
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int IdUser { get; set; } 
        public string Time { get; set; }
    }
}
