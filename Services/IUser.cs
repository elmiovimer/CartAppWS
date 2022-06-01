using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IUser
    {
        List<User> Get();
        User GetById(int id);
        User Login(string usr, string pwd);
        int Save(User user);
        int Delete(User user);
    }
}
