using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IReleaseLinkInstallRepository
    {
        List<ReleaseLinkInstall> GetAll();
        ReleaseLinkInstall Find(int id);
        void Add(ReleaseLinkInstall obj);
        void Edit(ReleaseLinkInstall obj);
        void Delete(int id);
    }
}
