using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class OffersProducts
    {
        public int IDOfferProduct { get; set; }
        public int IDOffer { get; set; }
        public int IDProduct { get; set; }
        
        public decimal Quantity { get; set; }
        public string Name { get; }
        public string Image { get; }

        public OffersProducts(int iDOfferProduct, int iDOffer, int iDProduct, decimal quantity, string name, string image)
        {
            IDOfferProduct = iDOfferProduct;
            IDOffer = iDOffer;
            IDProduct = iDProduct;
            Quantity = quantity;
            Name = name;
            Image = image;
        }

        public OffersProducts()
        {
            IDOfferProduct = 0;
        }
    }
}
