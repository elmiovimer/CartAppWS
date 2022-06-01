using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class City
    {
        public int IDCity { get; set; }
        public int IDState { get; set; }
        public string StateName { get; }
        public string Name { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public City()
        {
            IDCity = 0;
        }

        public City(int iDCity, int iDState, string stateName, string name, int createdUser, DateTime createdDate, int modifiedUser, DateTime modifiedDate, int status)
        {
            IDCity = iDCity;
            IDState = iDState;
            StateName = stateName;
            Name = name;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }
    }
}
