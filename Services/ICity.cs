using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface ICity
    {
        List<City> Get();
        List<City> GetByState(int id);
        City GetById(int id);
        int Save(City city);
        int Delete(City city);
    }
}
