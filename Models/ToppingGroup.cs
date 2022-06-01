using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class ToppingGroup
    {
        public ToppingType ToppingType { get; set; }
        public List<Topping> Toppings { get; set; }

        public ToppingGroup()
        {
        }

        public ToppingGroup(ToppingType toppingType, List<Topping> toppings)
        {
            ToppingType = toppingType;
            Toppings = toppings;
        }
    }
}
