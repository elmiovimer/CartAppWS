using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class EventOffer
    {
        public int IDEventOffer { get; set; }
        public int IDEvent { get; set; }
        public int IDOffer { get; set; }
        public string OfferName { get; set; }
    }
}
