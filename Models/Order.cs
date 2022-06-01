using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Order
    {
        public int IDOrder { get; set; }
        public String ClientName { get; set; }
        public string Phone { get; set; }
        public int IDClient { get; set; }
        public int IDAddress { get; set; }
        public Address Address { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Taxes { get; set; }
        public int IDPaymentMethod { get; set; }
        public int IDOrderType { get; set; }
        public int Status { get; set; }
        public int TookUserOrder { get; set; }
        public string MacAddress { get; set; }
        public int Tracking{ get; set; }
        public List<OrderItem> Items { get; set; }
        public List<OrderOffers> Offers { get; set; }
    }
}
