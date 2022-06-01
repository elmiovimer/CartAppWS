using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public class OrderTypeService : IOrderType
    {
        private readonly Conexion conexion;

        public OrderTypeService()
        {
            conexion = new Conexion();
        }



        public int Detele(OrderType type)
        {
            string sql = $"DELETE FROM OrdersTypes WHERE IDOrderType = {type.IDOrderType}";
            return conexion.Execute(sql);
        }

        public List<OrderType> Get()
        {
            List<OrderType> list = new List<OrderType>();
            string sql = "SELECT IDOrderType, Name, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM OrdersTypes " +
                "WHERE Status <> "  + (int) Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetOrderType(row));
            return list;
        }

        public List<OrderType> GetAvailable()
        {
            List<OrderType> list = new List<OrderType>();
            string sql = "SELECT IDOrderType, Name, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Available, Status " +
                "FROM OrdersTypes " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO + " AND Available = 'true'";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetOrderType(row));
            return list;
        }
        private OrderType GetOrderType(DataRow row)
        {
            return new OrderType()
            {
                IDOrderType = Convert.ToInt32(row["IDOrderType"]), 
                Name = row["Name"].ToString(),
                CreatedUser = Convert.ToInt32(row["CreatedUser"]),
                CreatedDate = Fecha.toDateTimeUTC(row["CreatedDate"]),
                ModifiedUser = Convert.ToInt32(row["ModifiedUser"]),
                ModifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]),
                Status = Convert.ToInt32(row["Status"]),
            };
        }

        public OrderType GetByID(int id)
        {
            string sql = "SELECT IDOrderType, Name, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM OrdersTypes " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO + " " +
                $"AND IDOrderType = {id}";
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetOrderType(dt.Rows[0]);
            return null;
        }

        public int Save(OrderType type)
        {
            if (type.IDOrderType == 0)
                return Insert(type);
            else
                return Update(type);
        }

        private int Insert(OrderType type)
        {
            string sql = "INSERT INTO OrdersTypes (Name, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@Name, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";
            string[] parametros = { "@Name", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status" };
            object[] valores = { type.Name, type.CreatedUser, type.CreatedDate, type.ModifiedUser, type.ModifiedDate, type.Status };
            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(OrderType type)
        {
            string sql = "UPDATE OrdersTypes SET Name = @Name, CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status  " +
                "WHERE IDOrderType = @IDOrderType";
            string[] parametros = { "@Name", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@IDOrderType" };
            object[] valores = { type.Name, type.CreatedUser, type.CreatedDate, type.ModifiedUser, type.ModifiedDate, type.Status, type.IDOrderType };
            return conexion.Execute(sql, parametros, valores);
        }
    }
}
