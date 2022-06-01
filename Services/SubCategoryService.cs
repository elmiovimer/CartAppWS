using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;

namespace CartAppWS.Services
{
    public class SubCategoryService : ISubCategory
    {
        Conexion conexion;

        public SubCategoryService()
        {
            conexion = new Conexion();
        }

        public int Delete(SubCategory subCategory)
        {
            string sql = "DELETE FROM SubCategory WHERE IDSubCategory = " + subCategory.IDSubCategory;
            return conexion.Execute(sql);
        }

        public List<SubCategory> Get()
        {
            List<SubCategory> list = new List<SubCategory>();
            string sql = "SELECT s.IDSubCategory, s.Name, s.IDCategory, c.Name as CategoryName, " +
                "s.Image, s.CreatedUser, s.CreatedDate, s.ModifiedUser, s.ModifiedDate, s.Status " +
                "FROM SubCategories s " +
                "JOIN Categories c ON (s.IDCategory = c.IDCategory) " +
                "WHERE s.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "ORDER BY c.Name, s.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetSubCategory(row));
            return list;
        }

        private SubCategory GetSubCategory(DataRow row)
        {
            int idSubCategory = Convert.ToInt32(row["IDSubCategory"]);
            string name = row["Name"].ToString();
            int idCaregory = Convert.ToInt32(row["IDCategory"]);
            string image = row["Image"].ToString();
            string categoryName = row["CategoryName"].ToString();
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            return new SubCategory(idSubCategory, name, idCaregory, categoryName, image, createdUser, createdDate, modifiedUser, modifiedDate, status);
        }

        public List<SubCategory> GetByCategory(int id)
        {
            List<SubCategory> list = new List<SubCategory>();
            string sql = "SELECT s.IDSubCategory, s.Name, s.IDCategory, c.Name as CategoryName, s.CreatedUser, s.CreatedDate, " +
                "s.Image, s.CreatedUser, s.CreatedDate, s.ModifiedUser, s.ModifiedDate, s.Status " +
                "FROM SubCategories s " +
                "JOIN Categories c ON (s.IDCategory = c.IDCategory) " +
                "WHERE s.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND s.IDCategory = " + id + " "  + 
                "ORDER BY s.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetSubCategory(row));
            return list;
        }

        public SubCategory GetById(int id)
        {
            string sql = "SELECT s.IDSubCategory, s.Name, s.IDCategory, c.Name as CategoryName, s.CreatedUser, s.CreatedDate, " +
                "s.Image, s.CreatedUser, s.CreatedDate, s.ModifiedUser, s.ModifiedDate, s.Status " +
                "FROM SubCategories s " +
                "JOIN Categories c ON (s.IDCategory = c.IDCategory) " +
                "WHERE s.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND s.IDSubCategory = " + id + " " +
                "ORDER BY c.Name, s.Name";
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetSubCategory(dt.Rows[0]);
            return null;
        }

        public int Save(SubCategory subCategory)
        {
            if (subCategory.IDSubCategory == 0)
                return insert(subCategory);
            else
                return update(subCategory);
        }

        private int insert(SubCategory subCategory)
        {
            string sql = "INSERT INTO SubCategories (Name, IDCategory, Image, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@Name, @IDCategory, @Image, @CreatedUser, " +
                "@CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";

            string[] parametros = {"@Name", "@IDCategory", "@Image", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status"};

            object[] valores = {subCategory.Name, subCategory.IDCategory, subCategory.Image, subCategory.CreatedUser, subCategory.CreatedDate, 
                subCategory.ModifiedUser, subCategory.ModifiedDate, subCategory.Status};


            return conexion.Execute(sql, parametros, valores);
        }

        private int update(SubCategory subCategory)
        {
            string sql = "UPDATE SubCategories SET Name = @Name, IDCategory = @IDCategory, Image = @Image, CreatedUser = @CreatedUser, " +
                "CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status " +
                "WHERE IDSubCategory = @IDSubCategory"; 

            string[] parametros = {"@Name", "@IDCategory", "@Image", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", 
                "@Status", "@IDSubCategory"};

            object[] valores = {subCategory.Name, subCategory.IDCategory, subCategory.Image, subCategory.CreatedUser, subCategory.CreatedDate,
                subCategory.ModifiedUser, subCategory.ModifiedDate, subCategory.Status, subCategory.IDSubCategory};


            return conexion.Execute(sql, parametros, valores);
        }

        public List<SubCategoryList> GetSubCategoryLists(int category)
        {
            List<SubCategoryList> subCategoryLists = new List<SubCategoryList>();
            List<SubCategory> subCategories = GetByCategory(category);
            ProductService ps = new ProductService();
            foreach(SubCategory subCategory in subCategories)
            {
                List<Product> products = ps.GetBySubCategory(subCategory.IDSubCategory);
                string sql = $"SELECT isnull(Max(ImageModifiedDate), GETDATE()) as ImageModifiedDate FROM Products WHERE IDSubCategory = {subCategory.IDSubCategory}";
                DataTable dt = conexion.Query(sql);
                DateTime date = Fecha.toDateTimeUTC(DateTime.Now);
                if(dt.Rows.Count > 0)
                    date = Fecha.toDateTimeUTC(dt.Rows[0]["ImageModifiedDate"]);
                subCategoryLists.Add(new SubCategoryList()
                {
                    SubCategory = subCategory,
                    LastImageModified = date,
                    Products = products
                });
            }
            return subCategoryLists;
        }
    }
}
