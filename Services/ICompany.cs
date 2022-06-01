using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface ICompany
    {
        Company Get();
        void Update(string foto);
    }
}
