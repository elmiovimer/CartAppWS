using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class CategoryList
    {
        public Category Category { get; set; }
        public List<SubCategoryList> SubCategories { get; set; }
    }
}
