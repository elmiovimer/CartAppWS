using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IAddress
    {
        List<Address> Get();
        List<Address> GetByClient(int id);
        Address GetById(int id);
        int Save(Address address);
        int Delete(Address address);
    }
}
