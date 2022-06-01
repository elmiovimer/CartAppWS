using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IPaymentMethod
    {
        List<PaymentMethod> Get();
        List<PaymentMethod> GetByClient(int id);
        PaymentMethod GetById(int id);
        int Save(PaymentMethod method);
        int Delete(PaymentMethod method);
    }
}
