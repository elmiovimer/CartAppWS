using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Company
    {
        public int IDCompany {get;set;}
        public string Name{get;set;}
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string SMSPhone { get; set; }
        public string SMSUser { get; set; }
        public string SMSPassword { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string SMTPServer { get; set; }
        public string StartTLS { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPAuthentication { get; set; }
        public string WSLink { get; set; }
        public string AuthorizeNETLoginID { get; set; }
        public string AuthorizeNETTransKey { get; set; }
        public string Logo { get; set; }
    }
}
