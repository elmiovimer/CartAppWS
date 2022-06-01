using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;

namespace CartAppWS.Services
{
    public class ToppingService : ITopping
    {
        private Conexion conexion;

        public ToppingService()
        {
            conexion = new Conexion();
        }

        public int Delete(Topping topping)
        {
            string sql = "DELETE FROM Toppings WHERE IDTopping = " + topping.IDTopping;
            int i = conexion.Execute(sql);
            return i;
        }

        public List<Topping> Get()
        {
            List<Topping> list = new List<Topping>();
            string sql = "SELECT t.IDTopping, t.Name, t.IDToppingType, tt.Name as ToppingTypeName, " +
                "t.CreatedUser, t.CreatedDate, t.ModifiedUser, t.ModifiedDate, t.Status, t.Price " +
                "FROM toppings t " +
                "JOIN ToppingTypes tt " +
                "ON (t.IDToppingType = tt.IDToppingType) " +
                "WHERE t.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "ORDER BY tt.Name";
            DataTable dt = conexion.Query(sql);
            foreach(DataRow row in dt.Rows)
            {
                list.Add(GetTopping(row));
            }
            return list;
        }

        public List<Topping> GetByToppingType(int id)
        {
            List<Topping> list = new List<Topping>();
            string sql = "SELECT t.IDTopping, t.Name, t.IDToppingType, tt.Name as ToppingTypeName, " +
                "t.CreatedUser, t.CreatedDate, t.ModifiedUser, t.ModifiedDate, t.Status, t.Price " +
                "FROM toppings t JOIN ToppingTypes tt ON (t.IDToppingType = tt.IDToppingType) " +
                "WHERE t.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND t.IDToppingType = " + id + " " +
                "ORDER BY tt.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(GetTopping(row));
            }
            return list;
        }

        private Topping GetTopping(DataRow row)
        {
            int idTopping = Convert.ToInt32(row["IDTopping"]);
            string name = row["Name"].ToString();
            int idToppingType = Convert.ToInt32(row["IDToppingType"]);
            string toppingTypeName = row["ToppingTypeName"].ToString();
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            double price = Convert.ToDouble(row["Price"]);
            return new Topping(idTopping, name, idToppingType, toppingTypeName, createdUser, createdDate, modifiedUser, modifiedDate, status, price);
        }

        public Topping GetById(int id)
        {
            Topping topping = null;
            string sql = "SELECT t.IDTopping, t.Name, t.IDToppingType, tt.Name as ToppingTypeName, " +
                "t.CreatedUser, t.CreatedDate, t.ModifiedUser, t.ModifiedDate, t.Status, t.Price " +
                "FROM toppings t " +
                "JOIN ToppingTypes tt ON (t.IDToppingType = tt.IDToppingType) " +
                "WHERE t.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND IDTopping = " + id;
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
            {
                topping = GetTopping(dt.Rows[0]);
            }
            conexion.Close();
            return topping;
        }

        public int Save(Topping topping)
        {
            if (topping.IDTopping == 0)
                return insert(topping);
            else
                return update(topping);
        }
        private int insert(Topping topping)
        {
            string sql = "INSERT INTO Toppings (Name, IDToppingType, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status, Price) " +
                "VALUES (@Name, @IDToppingType, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status, @Price) ";

            string[] parametros = {"@Name", "@IDToppingType", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@Price"};

            object[] valores = {topping.Name, topping.IDToppingType, topping.CreatedUser, topping.CreatedDate, topping.ModifiedUser, topping.ModifiedDate, topping.Status, topping.Price};

            int i = conexion.Execute(sql, parametros, valores);


            return i;
        }

        private int update(Topping topping)
        {
            string sql = "UPDATE Toppings SET Name = @Name, IDToppingType = @IDToppingType, CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, " +
                "ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status, Price = @Price  WHERE IDTopping = @IDTopping ";
            
            string[] parametros = {"@Name", "@IDToppingType", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status",  "@Price", "@IDTopping"};

            object[] valores = {topping.Name, topping.IDToppingType, topping.CreatedUser, topping.CreatedDate, topping.ModifiedUser, topping.ModifiedDate, topping.Status, topping.Price, topping.IDTopping};

            int i = conexion.Execute(sql, parametros, valores);

            return i;
        }
    }
}
