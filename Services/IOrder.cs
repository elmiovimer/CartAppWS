using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IOrder
    {
        List<Order> Get();
        Order GetById(int id);
        dynamic GetAvailableOrders(int id);
        List<Order> GetByClient(int id);
        public List<Order> TrackOrder(int tracking);
        public List<dynamic> TrackOrders(int[] ids);
        int Save(Order order);
        int Delete(Order order);
    }
}
