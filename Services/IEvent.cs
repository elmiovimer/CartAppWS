using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IEvent
    {
        List<Event> Get();
        List<Event> GetAvailable();
        Event GetById(int id);
        int Save(Event Event);
        int Delete(Event Event);
    }
}
