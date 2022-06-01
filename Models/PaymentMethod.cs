using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class PaymentMethod
    {
        public int IDPaymentMethod { get; set; }
        public int IDPaymentMethodType { get; set; }
        public int IDCardType { get; set; }
        public string PaymentMethodTypeName { get; set; }
        public int IDClient { get; set; }
        public string CardNumber { get; set; }
        public string ExpDate { get; set; }
        public string CVC { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime PaymentDate { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public PaymentMethod()
        {
            IDPaymentMethod = 0;
        }

        public PaymentMethod(int iDPaymentMethod, int iDPaymentMethodType,
            string paymentMethodTypeName, int iDClient, string cardNumber,
            string expDate, string cVC, string userName, string password,
            DateTime paymentDate, int createdUser, DateTime createdDate,
            int modifiedUser, DateTime modifiedDate, int status)
        {
            IDPaymentMethod = iDPaymentMethod;
            IDPaymentMethodType = iDPaymentMethodType;
            PaymentMethodTypeName = paymentMethodTypeName;
            IDClient = iDClient;
            CardNumber = cardNumber;
            ExpDate = expDate;
            CVC = cVC;
            UserName = userName;
            Password = password;
            PaymentDate = paymentDate;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }
    }
}
