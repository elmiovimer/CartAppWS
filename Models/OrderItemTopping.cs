namespace CartAppWS.Models
{
    public class OrderItemTopping
    {
        public int IDOrdersItemTopping { get; set; } 
        public int IDOrderItem { get; set; }
        public int IDTopping { get; set; }
        public string ToppingName { get; set; }
        public int IDToppingType { get; set; }
        public string ToppingTypeName { get; set; }
        public bool Selected { get; set; }
    }
}