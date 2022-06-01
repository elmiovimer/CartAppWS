using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IOrderType
    {
        List<OrderType> Get();
        public List<OrderType> GetAvailable();
        OrderType GetByID(int id);

        int Detele(OrderType type);
        int Save(OrderType type);
    }
}
