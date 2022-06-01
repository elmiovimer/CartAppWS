using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class State
    {
        public int IDState { get; set; }
        public string Name { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public State()
        {
            IDState = 0;
        }

        public State(int iDState, string name, int createdUser, DateTime createdDate, int modifiedUser, DateTime modifiedDate, int status)
        {
            IDState = iDState;
            Name = name;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }
    }
}
