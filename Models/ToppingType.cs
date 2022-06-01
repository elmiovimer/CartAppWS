using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class ToppingType
    {
        public int IDToppingType { get; set; }
        public string Name { get; set; }
        public int Combine { get; set; }

        public int Required { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public ToppingType(int iDToppingType, string name, int combine, int required, int createdUser, 
            DateTime createdDate, int modifiedUser, DateTime modifiedDate, int status)
        {
            IDToppingType = iDToppingType;
            Name = name;
            Combine = combine;
            Required = required;

            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }

        public ToppingType()
        {
            IDToppingType = 0;
        }
    }
}
