using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Payment
    {
        public Order order { get; set; }
        public int IDPaymentMethod { get; set; }
    }
}
