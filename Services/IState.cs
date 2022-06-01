using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IState
    {
        List<State> Get();
        State GetById(int id);
        int Save(State state);
        int Delete(State state);
    }
}
