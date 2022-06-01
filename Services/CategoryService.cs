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
    public class CategoryService : ICategory
    {
        private Conexion conexion;

        public CategoryService()
        {
            conexion = new Conexion();
        }

        public int Delete(Category category)
        {
            string sql = "DELETE FROM Categories WHERE IDCategory = " + category.IDCategory;
            return conexion.Execute(sql);
        }

        public List<Category> Get()
        {
            List<Category> list = new List<Category>();
            string sql = "SELECT IDCategory, Name, Position, StartTime, EndTime, CreatedUser, CreatedDate, " +
                "ModifiedUser, ModifiedDate, Status " +
                "FROM Categories " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " + 
                "ORDER BY Position, Name";
            DataTable dt = conexion.Query(sql);
            foreach(DataRow row in dt.Rows)
            {
                list.Add(GetCategory(row));
            }
            return list;
        }

        private Category GetCategory(DataRow row)
        {
            int id = Convert.ToInt32(row["IDCategory"]);
            string name = row["Name"].ToString();
            int position = Convert.ToInt32(row["Position"]);
            DateTime startTime = Fecha.toDateTimeUTC(row["StartTime"]);
            DateTime endTime = Fecha.toDateTimeUTC(row["EndTime"]);
            int createdUser = Convert.ToInt32(row["CreatedUser"]); 
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            SubCategoryService scs = new SubCategoryService();
            return new Category(id, name, position, startTime, endTime, createdUser, createdDate, modifiedUser, modifiedDate, status, scs.GetByCategory(id));
        }

        public Category GetById(int id)
        {
            string sql = "SELECT IDCategory, Name, Position, StartTime, EndTime, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM Categories " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND IDCategory = " + id;
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
            {
                return GetCategory(dt.Rows[0]);
            }
            return null;
        }

        public int Save(Category category)
        {
            if (category.IDCategory == 0)
                return insert(category);
            else
                return update(category);
        }

        private int insert(Category category)
        {
            string sql = "INSERT INTO Categories (Name, Position, StartTime, EndTime, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@Name, @Position, @StartTime, @EndTime, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";

            string[] parametros = {"@Name", "@Position", "@StartTime", "@EndTime", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate",
                    "@Status"};
            Object[] valores = {category.Name, category.Position, category.StartTime, category.EndTime, category.CreatedUser, category.CreatedDate, 
                    category.ModifiedUser, category.ModifiedDate, category.Status};

            return conexion.Execute(sql, parametros, valores);
        }



        private int update(Category category)
        {
            string sql = "UPDATE Categories SET Name = @Name, Position = @Position, StartTime = @StartTime, " +
                "EndTime = @EndTime, CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, " +
                "Status = @Status " +
                "WHERE  IDCategory = @IDCategory";

            string[] parametros = {"@Name", "@Position", "@StartTime", "@EndTime", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", 
                    "@Status", "@IDCategory"};

            Object[] valores = {category.Name, category.Position, category.StartTime, category.EndTime, category.CreatedUser, category.CreatedDate, 
                    category.ModifiedUser, category.ModifiedDate, category.Status, category.IDCategory};


            return conexion.Execute(sql, parametros, valores);
        }

        public List<CategoryList> GetCategoryList()
        {
            List<CategoryList> categoryLists = new List<CategoryList>();
            List<Category> categories = Get();
            SubCategoryService scs = new SubCategoryService();
            foreach(Category category in categories)
            {
                categoryLists.Add(new CategoryList()
                {
                    Category = category,
                    SubCategories = scs.GetSubCategoryLists(category.IDCategory)
                });
            }
            return categoryLists;
        }

        public CategoryList GetCategoryListByID(int id)
        {
           
            Category category = GetById(id);
            SubCategoryService scs = new SubCategoryService();
            if(category != null)
            {
                return new CategoryList()
                {
                    Category = category,
                    SubCategories = scs.GetSubCategoryLists(category.IDCategory)
            };
            }
            return null;
        }
    }
}
