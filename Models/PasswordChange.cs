using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class PasswordChange
    {
        public int IDClient { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
