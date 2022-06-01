using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IOffer
    {
        List<Offer> Get();
        List<Offer> GetAvailable();
        Offer GetById(int id);
        int Save(Offer offer);
        int Delete(Offer offer);
    }
}
