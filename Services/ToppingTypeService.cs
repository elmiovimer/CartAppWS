using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public class ToppingTypeService: IToppingType
    {
        private Conexion conexion;

        public ToppingTypeService()
        {
            conexion = new Conexion();
        }

        int IToppingType.Delete(ToppingType toppingType)
        {
            String sql = "DELETE FROM ToppingTypes WHERE IDToppingType = " + toppingType.IDToppingType;
            int i = conexion.Execute(sql);
            return i;
        }

        List<ToppingType> IToppingType.Get()
        {
            return GetToppingTypes();
        }

        private ToppingType GetToppingType(DataRow row)
        {
            int id = int.Parse(row["IDToppingType"].ToString());
            string name = row["Name"].ToString();
            int combine = int.Parse(row["Combine"].ToString());
            int required = int.Parse(row["Required"].ToString());
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            return new ToppingType(id, name, combine, required, createdUser, createdDate, modifiedUser, modifiedDate, status);
        }

        ToppingType IToppingType.GetById(int id)
        {
            return GetByID(id);
        }

        int IToppingType.Save(ToppingType toppingType)
        {
            if (toppingType.IDToppingType == 0)
                return insert(toppingType);
            else
                return update(toppingType);
        }

        private int insert(ToppingType toppingType)
        {
            String sql = "INSERT INTO ToppingTypes (Name, Combine, Required, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@Name, @Combine, @Required, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";

            string[] parametros = {"@Name", "@Combine", "@Required", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status"};

            object[] valores = {toppingType.Name, toppingType.Combine, toppingType.Required, toppingType.CreatedUser, toppingType.CreatedDate, toppingType.ModifiedUser, 
                toppingType.ModifiedDate, toppingType.Status};


            int i= conexion.Execute(sql, parametros, valores);
            return i;
        }

        private int update(ToppingType toppingType)
        {
            String sql = "UPDATE ToppingTypes SET Name = @Name, Combine = @Combine, Required = @Required, CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, " + 
                "ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status " +
                "WHERE IDToppingType = @IDToppingType";

            string[] parametros = {"@Name", "@Combine", "@Required", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@IDToppingType"};

            object[] valores = {toppingType.Name, toppingType.Combine, toppingType.Required, toppingType.CreatedUser, toppingType.CreatedDate, toppingType.ModifiedUser, 
                toppingType.ModifiedDate, toppingType.Status, toppingType.IDToppingType};


            int i= conexion.Execute(sql, parametros, valores);
            return i;
        }

        ToppingGroup IToppingType.GetGroup(int id)
        {
            ToppingGroup tg = new ToppingGroup
            {
                ToppingType = GetByID(id),
                Toppings = new ToppingService().GetByToppingType(id)
            };
            return tg;
        }

        List<ToppingGroup> IToppingType.GetGroups()
        {
            List<ToppingType> toppingTypes = GetToppingTypes();
            List<ToppingGroup> list = new List<ToppingGroup>();
            ToppingService ts = new ToppingService();
            foreach(ToppingType tt in toppingTypes)
            {
                var Listado = ts.GetByToppingType(tt.IDToppingType);
                ToppingGroup tg = new ToppingGroup
                {
                    ToppingType = tt,
                    Toppings = Listado != null ? Listado : new List<Topping>()
                };
                list.Add(tg);
            }
            
            return list;
        }

        private ToppingType GetByID(int id)
        {
            String sql = "SELECT IDToppingType, Name, Combine, Required, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM ToppingTypes " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " + 
                "AND IDToppingType = " + id;
            DataTable data = conexion.Query(sql);
            if (data.Rows.Count > 0)
            {
                return GetToppingType(data.Rows[0]);
            }
            return null;
        }

        private List<ToppingType> GetToppingTypes()
        {
            List<ToppingType> list = new List<ToppingType>();
            String sql = "SELECT IDToppingType, Name, Combine, Required, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM ToppingTypes " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(GetToppingType(row));
            }
            return list;
        }
    }
}
