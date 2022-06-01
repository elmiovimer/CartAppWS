using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface ICategory
    {
        List<Category> Get();
        Category GetById(int id);
        List<CategoryList> GetCategoryList();
        CategoryList GetCategoryListByID(int id);
        int Save(Category category);
        int Delete(Category category);
    }
}
