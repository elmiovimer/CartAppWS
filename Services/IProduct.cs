using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IProduct
    {
        List<Product> Get();
        List<Product> GetByCategory(int id);
        List<Product> GetBySubCategory(int id);
        List<Product> GetProductsBySubCategory(int id);
        Product GetById(int id);
        List<Image> GetImages(int[] IDs);
        List<Image> GetImagesBySubCategory(int id, DateTime date);
        int Save(Product product);
        int Delete(Product product);
    }
}
