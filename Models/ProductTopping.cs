using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class ProductTopping
    {
        public int IDProductTopping { get; set; }
        public int IDProduct { get; set; }
        public int IDTopping { get; set; }
        public string ToppingName { get; }

        public int IDToppingType { get; set; }
        public string ToppingTypeName { get; set; }
        public int Combine { get; set; }
        public int Required { get; set; }
        public bool ByDefault { get; set; }

        public ProductTopping()
        {
            IDProductTopping = 0;
            ByDefault = true;
        }

        public ProductTopping(int iDProductTopping, int iDProduct, int iDTopping, string toppingName, int iDToppingType, string toppingTypeName, int combine, int requiered, bool byDefault)
        {
            IDProductTopping = iDProductTopping;
            IDProduct = iDProduct;
            IDTopping = iDTopping;
            ToppingName = toppingName;
            IDToppingType = iDToppingType;
            ToppingTypeName = toppingTypeName;
            Combine = combine;
            Required = requiered;
            ByDefault = byDefault;
        }
    }
}
