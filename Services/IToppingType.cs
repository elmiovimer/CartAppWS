using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IToppingType
    {
        List<ToppingType> Get();
        ToppingType GetById(int id);
        ToppingGroup GetGroup(int id);
        List<ToppingGroup> GetGroups();
        int Save(ToppingType toppingType);
        int Delete(ToppingType toppingType);
    }
}
