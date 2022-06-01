using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class SubCategory
    {
        public int IDSubCategory { get; set; }
        public string Name { get; set; }
        public int IDCategory { get; set; }
        public string CategoryName { get; }
        public string Image { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

        public SubCategory()
        {
            IDCategory = 0;
            Status = 1;
        }

        public SubCategory(int iDSubCategory, string name, int iDCategory, string categoryName, 
            string image, int createdUser, DateTime createdDate, int modifiedUser, DateTime modifiedDate, 
            int status)
        {
            IDSubCategory = iDSubCategory;
            Name = name;
            IDCategory = iDCategory;
            CategoryName = categoryName;
            Image = image;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }
    }
}
