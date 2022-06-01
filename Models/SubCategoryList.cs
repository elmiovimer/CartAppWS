using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class SubCategoryList
    {
        public SubCategory SubCategory { get; set; }
        public DateTime LastImageModified { get; set; }
        public List<Product> Products { get; set; }
    }
}
