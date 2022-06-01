using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface ISubCategory
    {
        List<SubCategory> Get();
        List<SubCategory> GetByCategory(int id);
        SubCategory GetById(int id);
        List<SubCategoryList> GetSubCategoryLists(int category);
        int Save(SubCategory subCategory);
        int Delete(SubCategory subCategory);
    }
}
