using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITopicRepository
    {
        IEnumerable<tbl_Topic> GetAll();
        tbl_Topic Find(int id);
        void Add(tbl_Topic obj);
        bool Edit(tbl_Topic obj);
        void Edit(List<tbl_Topic> lstobj);
        void Delete(int id);

        IEnumerable<tbl_TopicComment> GetAllCMT();
        tbl_TopicComment FindCMT(int id);
        void AddCMT(tbl_TopicComment obj);
        void EditCMT(tbl_TopicComment obj);
        void DeleteCMT(int id);
    }
}
