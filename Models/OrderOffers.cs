using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class OrderOffers
    {
        public int IDOrderOffer { get; set; }
        public int IDOrder { get; set; } 
        public int IDOffer { get; set; }
        public string OfferName { get; set; }
        public decimal Quantity { get; set; }
    }
}
