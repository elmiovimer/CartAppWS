using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Claim
    {
        public int IDClaim { get; set; }
        public int IDOrder { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
    }
}
