using CartAppWS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Category
    {
        public int IDCategory { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public List<SubCategory> SubCategories { get; }

        public Category()
        {
            IDCategory = 0;
            Status = (int) Constants.Status.ACTIVO;
        }

        public Category(int iDCategory, string name, int position, DateTime startTime, 
            DateTime endTime, int createdUser, DateTime createdDate, int modifiedUser, 
            DateTime modifiedDate, int status, List<SubCategory> subCategories)
        {
            IDCategory = iDCategory;
            Name = name;
            Position = position;
            StartTime = startTime;
            EndTime = endTime;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
            SubCategories = subCategories;
        }
    }
}
