using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class ClientValidation
    {
        public int IDValidation { get; set; }
        public int IDClient { get; set; }
        public string UUID { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }

        public string GetLink()
        {
            return "/api/client/validate?validation=" + UUID;
        }
    }
}
