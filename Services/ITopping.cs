using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface ITopping
    {
        List<Topping> Get();

        List<Topping> GetByToppingType(int id);
        Topping GetById(int id);
        int Save(Topping topping);
        int Delete(Topping topping);

    }
}
