using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;

namespace CartAppWS.Services
{
    public class ProductService : IProduct
    {
        private Conexion conexion;

        public ProductService()
        {
            conexion = new Conexion();
        }

        public int Delete(Product product)
        {
            String sql = "DELETE FROM products WHERE IDProduct = " + product.IDProduct;
            return conexion.Execute(sql);
        }

        public List<Product> Get()
        {
            List<Product> list = new List<Product>();
            string sql = "SELECT p.IDProduct, p.IDCategory, c.Name as CategoryName, " +
                "s.IDSubCategory, s.Name as SubCategoryName, p.Name, p.Price, p.Tax, " +
                "p.Image, p.CreatedDate, p.ModifiedDate, p.ImageModifiedDate, p.CreatedUser, p.Status " +
                "FROM products p " +
                "JOIN Categories c ON (p.IDCategory = c.IDCategory) " +
                "JOIN SubCategories s ON (p.IDSubCategory = s.IDSubCategory) " +
                "WHERE p.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "ORDER BY c.Position, c.Name, s.Name, p.Name ";
            DataTable dt = conexion.Query(sql);
            foreach(DataRow row in dt.Rows)
            {
                list.Add(GetProduct(row));
            }
            return list;
        }

        public List<Product> GetByCategory(int id)
        {
            List<Product> list = new List<Product>();
            string sql = "SELECT p.IDProduct, p.IDCategory, c.Name as CategoryName, " +
                "s.IDSubCategory, s.Name as SubCategoryName, p.Name, p.Price, p.Tax, " +
                "p.Image, p.CreatedDate, p.ModifiedDate, p.ImageModifiedDate, p.CreatedUser, p.Status " +
                "FROM products p " +
                "JOIN Categories c ON (p.IDCategory = c.IDCategory) " +
                "JOIN SubCategories s ON (p.IDSubCategory = s.IDSubCategory) " +
                "WHERE p.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND p.IDCategory = " + id + " " +
                "ORDER BY c.Position, c.Name, s.Name, p.Name ";            
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(GetProduct(row));
            }
            return list;
        }
        public List<Product> GetBySubCategory(int id)
        {
            List<Product> list = new List<Product>();
            string sql = "SELECT p.IDProduct, p.IDCategory, c.Name as CategoryName, " +
                "s.IDSubCategory, s.Name as SubCategoryName, p.Name, p.Price, p.Tax, " +
                "p.Image, p.CreatedDate, p.ModifiedDate, p.ImageModifiedDate, p.CreatedUser, p.Status " +
                "FROM products p " +
                "JOIN Categories c ON (p.IDCategory = c.IDCategory) " +
                "JOIN SubCategories s ON (p.IDSubCategory = s.IDSubCategory) " +
                "WHERE p.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND p.IDSubCategory = " + id + " " +
                "ORDER BY c.Position, c.Name, s.Name, p.Name ";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(GetProduct(row));
            }
            return list;
        }

        public List<Product> GetProductsBySubCategory(int id) //Retorna los productos por subcategoria sin imagen
        {
            List<Product> list = new List<Product>();
            string sql = "SELECT p.IDProduct, p.IDCategory, c.Name as CategoryName, " +
                "s.IDSubCategory, s.Name as SubCategoryName, p.Name, p.Price, p.Tax, " +
                "' ' as Image, p.CreatedDate, p.ModifiedDate, p.ImageModifiedDate,  p.CreatedUser, p.Status " +
                "FROM products p " +
                "JOIN Categories c ON (p.IDCategory = c.IDCategory) " +
                "JOIN SubCategories s ON (p.IDSubCategory = s.IDSubCategory) " +
                "WHERE p.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "AND p.IDSubCategory = " + id + " " +
                "ORDER BY c.Position, c.Name, s.Name, p.Name ";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(GetProduct(row));
            }
            return list;
        }

        private Product GetProduct(DataRow row)
        {
            int idProduct = Convert.ToInt32(row["IDProduct"]);
            int idCategory = Convert.ToInt32(row["IDCategory"]);
            int idSubCategory = Convert.ToInt32(row["IDSubCategory"]);
            string categoryName = row["CategoryName"].ToString();
            string subCategoryName = row["SubCategoryName"].ToString();
            string name = row["Name"].ToString() ;
            decimal price = Convert.ToDecimal(row["Price"]);
            decimal tax = Convert.ToDecimal(row["Tax"]);
            string image = row["image"].ToString();
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            int status = Convert.ToInt32(row["Status"]);
            DateTime imageModifiedDate = Fecha.toDateTimeUTC(row["ImageModifiedDate"]);
            return new Product(idProduct, idCategory, categoryName, idSubCategory, subCategoryName, name, price, tax, 
                image, createdDate, modifiedDate, imageModifiedDate, createdUser, status, GetToppings(idProduct));
        }

        private List<ProductTopping> GetToppings(int idproduct)
        {
            List<ProductTopping> list = new List<ProductTopping>();
            String sql = "SELECT pt.IDProductTopping, pt.IDProduct, pt.IDTopping, t.Name, pt.ByDefault, tt.IDToppingType, tt.Name as ToppingTypeName, " +
                "tt.Combine, tt.Required " +
                "FROM ProductsToppings pt " +
                "JOIN Toppings t ON(pt.IDTopping = t.IDTopping) " +
                "JOIN ToppingTypes tt ON(t.IDToppingType = tt.IDToppingType) " +
                "WHERE IDProduct = " + idproduct;

            DataTable dt = conexion.Query(sql);
            
            foreach(DataRow row in dt.Rows)
            {
                list.Add(GetTopping(row));
            }
            return list;
        }

        private ProductTopping GetTopping(DataRow row)
        {
            int idProductTopping = Convert.ToInt32(row["IDProductTopping"]);
            int idProduct = Convert.ToInt32(row["IDProduct"]);
            int idTopping = Convert.ToInt32(row["IDTopping"]);
            string name = row["Name"].ToString();
            int idToppingType = Convert.ToInt32(row["IDToppingType"].ToString());
            string toppingTypeName = row["ToppingTypeName"].ToString();
            int combine = Convert.ToInt32(row["Combine"]);
            int required = Convert.ToInt32(row["Required"]);
            bool byDefault = Convert.ToBoolean(row["ByDefault"]);
            return new ProductTopping(idProductTopping, idProduct, idTopping, name, idToppingType, toppingTypeName, combine, required, byDefault);
        }

        public Product GetById(int id)
        {
            Product product = null;
            string sql = "SELECT p.IDProduct, p.IDCategory, c.Name as CategoryName, " +
                "s.IDSubCategory, s.Name as SubCategoryName, p.Name, p.Price, p.Tax, " +
                "p.Image, p.CreatedDate, p.ModifiedDate, p.ImageModifiedDate, p.CreatedUser, p.Status " +
                "FROM products p " +
                "JOIN Categories c ON (p.IDCategory = c.IDCategory) " +
                "JOIN SubCategories s ON (p.IDSubCategory = s.IDSubCategory) " +
                "WHERE p.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND p.IDProduct = " + id;
                

            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
            {
                product = GetProduct(dt.Rows[0]);
            }
            return product;
        }

        public List<CartAppWS.Models.Image> GetImages(int[] IDs)
        {
            List<CartAppWS.Models.Image> list = new List<CartAppWS.Models.Image>();
            string filtro = "(";
            foreach (int i in IDs)
                filtro += $"{i},";
            filtro += filtro[0..^1] + ")";
            string sql = "SELECT p.IDProduct, p.Image " +
                "FROM products p " +
                "JOIN Categories c ON (p.IDCategory = c.IDCategory) " +
                "JOIN SubCategories s ON (p.IDSubCategory = s.IDSubCategory) " +
                "WHERE p.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "AND p.IDProduct in " + filtro + " " +
                "ORDER BY c.Position, c.Name, s.Name, p.Name ";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CartAppWS.Models.Image()
                {
                    ID = Convert.ToInt32(row["IDProduct"]),
                    Picture = row["Image"].ToString(),
                }); 
            }
            return list;
        }

        public List<CartAppWS.Models.Image> GetImagesBySubCategory(int id, DateTime date)
        {
            List<CartAppWS.Models.Image> list = new List<CartAppWS.Models.Image>();         
            string sql = "SELECT p.IDProduct, p.Image " +
                "FROM products p " +
                "JOIN Categories c ON (p.IDCategory = c.IDCategory) " +
                "JOIN SubCategories s ON (p.IDSubCategory = s.IDSubCategory) " +
                "WHERE p.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "AND p.IDSubCategory = @IDSubCategory AND p.ImageModifiedDate < @ImageModifiedDate " +
                "ORDER BY c.Position, c.Name, s.Name, p.Name ";
            string[] parametros = { "@IDSubCategory", "ImageModifiedDate" };
            object[] valores = { id, date};

            DataTable dt = conexion.Query(sql, parametros, valores);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CartAppWS.Models.Image()
                {
                    ID = Convert.ToInt32(row["IDProduct"]),
                    Picture = row["Image"].ToString(),
                });
            }
            return list;
        }

        public int Save(Product product)
        {
            if (product.IDProduct == 0)
                return Insert(product);
            else
                return Update(product);
        }

        private int Insert(Product product)
        {
            int i;
            product.ImageModifiedDate = DateTime.Now;
            string sql1 = "INSERT INTO products (IDCategory, IDSubCategory, Name, Price, Tax, Image, CreatedDate, " +
                "ModifiedDate, CreatedUser, Status, ImageModifiedDate) " +
                "VALUES (@IDCategory, @IDSubCategory, @Name, @Price, @Tax, @Image, @CreatedDate, " +
                "@ModifiedDate, @CreatedUser, @Status, @ImageModifiedDate)";

            string[] parametros = {"@IDCategory", "@IDSubCategory", "@Name", "@Price", "@Tax", "@Image", "@CreatedDate",
                "@ModifiedDate", "@CreatedUser", "@Status", "@ImageModifiedDate"};

            object[] valores = {product.IDCategory, product.IDSubCategory, product.Name, product.Price, product.Taxes, product.Image, product.CreateDate, 
                product.ModifiedDate, product.CreatedUser, product.Status, product.ImageModifiedDate};



            conexion.BeginTrassancion();
            try
            {                
                i = conexion.Execute(sql1, parametros, valores);
                int id = conexion.LastInsertID();
                foreach (ProductTopping producttopping in product.ProductTopping)
                {
                    producttopping.IDProduct = id;
                    i += InsertToppings(producttopping);
                }
                conexion.Commit();
            }catch(Exception ){
                conexion.Rollback();
                throw;
            }
            return i;
        }

        private int Update(Product product)
        {
            int i;
            string sql = "UPDATE products SET IDCategory = @IDCategory, IDSubCategory = @IDSubCategory, Name = @Name, Price = @Price, Tax = @Tax, " +
                "Image = @Image, CreatedDate = @CreatedDate, ModifiedDate = @ModifiedDate, CreatedUser = @CreatedUser, " +
                "Status = @Status WHERE IDProduct = @IDProduct";


            string[] parametros = {"@IDCategory", "@IDSubCategory", "@Name", "@Price", "@Tax", "@Image", "@CreatedDate",
                "@ModifiedDate", "@CreatedUser", "@Status", "@IDProduct"};

            object[] valores = {product.IDCategory, product.IDSubCategory, product.Name, product.Price, product.Taxes, product.Image, product.CreateDate, 
                product.ModifiedDate, product.CreatedUser, product.Status, product.IDProduct};


            conexion.BeginTrassancion();
            try
            {
               

                i = conexion.Execute(sql, parametros, valores);
                i += DeleteToppings(product);
                foreach (ProductTopping producttopping in product.ProductTopping)
                {
                    producttopping.IDProduct = product.IDProduct;
                    if (producttopping.IDProductTopping == 0)
                        i += InsertToppings(producttopping);
                    else
                        i += UpdatetToppings(producttopping);
                }
                conexion.Commit();
            }catch(Exception)
            {
                conexion.Rollback();
                throw;
            }
            return i;
        }

        private int DeleteToppings(Product product)
        {
            if(product.ProductTopping.Count > 0)
            {
                String sql = "DELETE FROM ProductsToppings WHERE IDProduct = " + product.IDProduct + " AND Not IDTopping in (";
                foreach (ProductTopping producttopping in product.ProductTopping)
                    sql += producttopping.IDTopping + ",";

                sql = sql.Substring(0, sql.Length - 1) + ")";
                return conexion.Execute(sql);
            }
            return 0;
        }

        private int InsertToppings(ProductTopping productTopping)
        {
            string sql2 = "INSERT INTO ProductsToppings (IDProduct, IDTopping, ByDefault) VALUES (@IDProduct, @IDTopping, @ByDefault)";

            string[] parametros = {"@IDProduct", "@IDTopping", "@ByDefault"};

            object[] valores = {productTopping.IDProduct, productTopping.IDTopping, productTopping.ByDefault};

            return conexion.Execute(sql2, parametros, valores);
        }

        private int UpdatetToppings(ProductTopping productTopping)
        {
            string sql2 = "UPDATE ProductsToppings SET IDProduct = @IDProduct , IDTopping = @IDTopping, ByDefault = @ByDefault WHERE IDProductTopping = @IDProductTopping";
            
            string[] parametros = {"@IDProduct", "@IDTopping", "@ByDefault", "@IDProductTopping"};

            object[] valores = {productTopping.IDProduct, productTopping.IDTopping, productTopping.ByDefault, productTopping.IDProductTopping};

            return conexion.Execute(sql2, parametros, valores);
        }

    }
}
