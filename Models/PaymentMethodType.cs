using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class PaymentMethodType
    {
        public int IDPaymentPethodType { get; set; }
        public string Name { get; set; }
        public bool Card { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public PaymentMethodType()
        {
            IDPaymentPethodType = 0;
        }

        public PaymentMethodType(int iDPaymentPethodType, string name, bool card, 
            int createdUser, DateTime createdDate, int modifiedUser, 
            DateTime modifiedDate, int status)
        {
            IDPaymentPethodType = iDPaymentPethodType;
            Name = name;
            Card = card;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }
    }
}
