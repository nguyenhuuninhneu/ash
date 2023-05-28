using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IAttachmentRepository
    {
        List<Attachment> GetAll();
        Attachment Find(int id);
        void Add(Attachment obj);
        void Edit(Attachment obj);
        void Delete(int id);
    }
}
