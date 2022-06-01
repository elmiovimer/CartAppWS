using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Product
    {
        public int IDProduct { get; set; }
        public int IDCategory { get; set; }
        public string CategoryName { get; }
        public int IDSubCategory { get; set; }
        public string SubCategoryName { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ImageModifiedDate { get; set; }
        public List<ProductTopping> ProductTopping { get; set; }

        public Product()
        {
            this.IDProduct = 0;
            this.ProductTopping = new List<ProductTopping>();
        }

        public Product(int idProduct, int idCategory, string categoryName, int subCategory, string subCategoryName, 
            string name, decimal price, decimal taxes, string image,
            DateTime createDate, DateTime modifiedDate, DateTime imageModifiedDate, int createdUser, int status,
            List<ProductTopping> listProductTopping)
        {
            IDProduct = idProduct;
            IDCategory = idCategory;
            CategoryName = categoryName;
            IDSubCategory = subCategory;
            SubCategoryName = subCategoryName;
            Name = name;
            Price = price;
            Taxes = taxes;
            Image = image;
            Status = status;
            CreatedUser = createdUser;
            CreateDate = createDate;
            ModifiedDate = modifiedDate;
            ImageModifiedDate = imageModifiedDate;
            ProductTopping = listProductTopping;
        }
    }
}
