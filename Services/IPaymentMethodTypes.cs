using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IPaymentMethodTypes
    {
        List<PaymentMethodType> Get();
        PaymentMethodType GetById(int id);
        int Save(PaymentMethodType paymentPethodType);
        int Delete(PaymentMethodType paymentPethodType);
    }
}
