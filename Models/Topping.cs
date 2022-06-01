using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Topping
    {
        public int IDTopping { get; set; }
        public string Name { get; set; }
        public int IDToppingType { get; set; }
        public string ToppingTypeName { get; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }
        public double Price { get; set; }

        public Topping(int iDTopping, string name, int iDToppingType, string toppingTypeName, 
            int createdUser, DateTime createdDate, int modifiedUser, DateTime modifiedDate, int status, double price)
        {
            IDTopping = iDTopping;
            Name = name;
            IDToppingType = iDToppingType;
            ToppingTypeName = toppingTypeName;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
            Price = price;

        }

        public Topping()
        {
            IDTopping = 0;
            Price = 0;
        }
    }
}
