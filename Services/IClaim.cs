using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IClaim 
    {
        List<Claim> Get();
        List<Claim> GetByOrder(int id);
        Claim GetById(int id);
        int Save(Claim claim);
        int Delete(Claim claim);
    }
}
