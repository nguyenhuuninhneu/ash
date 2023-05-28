using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface INewsCategory_Repository
    {
        List<news_category> GetAll();
        news_category Find(int id);

        void Add(news_category obj);
        void Edit(news_category obj);
        void Delete(int id);
    }
}
