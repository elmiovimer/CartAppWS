using System.Collections.Generic;

namespace CartAppWS.Models
{
    public class OrderItem
    {
        public int IDOrderItem { get; set; }
        public int IDOrder { get; set; }
        public int IDProduct { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public string Comment { get; set; }
        public List<OrderItemTopping> toppings { get; set; }        
    }
}