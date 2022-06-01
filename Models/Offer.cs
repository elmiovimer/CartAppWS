using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Offer
    {
        public int IDOffer { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public string Comment { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public List<OffersProducts> Products { get; set; }

        public Offer(int iDOffer, string name, string image, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, decimal price, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday, string comment, int createdUser, DateTime createDate, int modifiedUser, DateTime modifiedDate, int status, List<OffersProducts> products)
        {
            IDOffer = iDOffer;
            Name = name;
            Image = image;
            StartDate = startDate;
            EndDate = endDate;
            StartTime = startTime;
            EndTime = endTime;
            Price = price;
            Monday = monday;
            Tuesday = tuesday;
            Wednesday = wednesday;
            Thursday = thursday;
            Friday = friday;
            Saturday = saturday;
            Sunday = sunday;
            Comment = comment;
            CreatedUser = createdUser;
            CreatedDate = createDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
            Products = products;
        }

        public Offer()
        {
            IDOffer = 0;
            Price = 0;
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
            Sunday = false;
            Products = new List<OffersProducts>();
        }
    }
}
