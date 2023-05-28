using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IDocumentPrice_Repository
    {
        List<document_price> GetAll();
        document_price Find(int id);

        void Add(document_price obj);
        void Edit(document_price obj);
        void Delete(int id);
    }
}
