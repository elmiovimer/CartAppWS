using CartAppWS.Models;
using CartAppWS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IDataService
    {
        List<Employee> Get();
        Employee GetById(int id);

        int Save(Employee employee);
    }
}
