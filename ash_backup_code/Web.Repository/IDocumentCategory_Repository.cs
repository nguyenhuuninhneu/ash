using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IDocumentCategory_Repository
    {
        List<document_category> GetAll();
        IEnumerable<document_category> GetAllByTableName(string tableName);
        document_category Find(int id);

        void Add(document_category obj);
        void Edit(document_category obj);
        void Delete(int id);
    }
}
