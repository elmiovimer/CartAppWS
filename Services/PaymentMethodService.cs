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
    public class PaymentMethodService : IPaymentMethod
    {
        private readonly Conexion conexion;

        public PaymentMethodService()
        {
            conexion = new Conexion();
        }

        public int Delete(PaymentMethod method)
        {
            string sql = $"DELETE FROM PaymentMethod WHERE IDPaymentMethod = {method.IDPaymentMethod}";
            return conexion.Execute(sql);
        }

        public List<PaymentMethod> Get()
        {
            List<PaymentMethod> list = new List<PaymentMethod>();
            string sql = "SELECT pm.IDPaymentMethod, pm.IDPaymentMethodType, t.Name as PaymentMethodTypeName, pm.IDCardType, " +
                "pm.IDClient, pm.CardNumber, pm.ExpDate, pm.CVC, pm.UserName, pm.Password, " +
                "pm.PaymentDate, pm.CreatedUser, pm.CreatedDate, pm.ModifiedUser, " +
                "pm.ModifiedDate, pm.Status " +
                "FROM PaymentMethod pm " +
                "JOIN PaymentPethodTypes t ON(pm.IDPaymentMethodType = t.IDPaymentMethodType) " +
                "WHERE pm.Status <> " + (int) Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetMethod(row));
            return list;
        }

        private PaymentMethod GetMethod(DataRow row)
        {
            return new PaymentMethod()
            {
                IDPaymentMethod = Convert.ToInt32(row["IDPaymentMethod"]),
                IDPaymentMethodType = Convert.ToInt32(row["IDPaymentMethodType"]),
                PaymentMethodTypeName = row["PaymentMethodTypeName"].ToString(),
                IDCardType = Convert.ToInt32(row["IDCardType"]),
                IDClient = Convert.ToInt32(row["IDClient"]),
                CardNumber = row["CardNumber"].ToString(),
                ExpDate = row["ExpDate"].ToString(),
                CVC = row["CVC"].ToString(),
                UserName = row["UserName"].ToString(),
                Password = row["Password"].ToString(),
                PaymentDate = Fecha.toDateTimeUTC(row["PaymentDate"]),
                CreatedUser = Convert.ToInt32(row["CreatedUser"]),
                CreatedDate = Fecha.toDateTimeUTC(row["CreatedDate"]),
                ModifiedUser = Convert.ToInt32(row["ModifiedUser"]),
                ModifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]),
                Status = Convert.ToInt32(row["Status"]),
            };
        }

        public List<PaymentMethod> GetByClient(int id)
        {
            List<PaymentMethod> list = new List<PaymentMethod>();
            string sql = "SELECT pm.IDPaymentMethod, pm.IDPaymentMethodType, t.Name as PaymentMethodTypeName, pm.IDCardType, " +
                "pm.IDClient, pm.CardNumber, pm.ExpDate, pm.CVC, pm.UserName, pm.Password, " +
                "pm.PaymentDate, pm.CreatedUser, pm.CreatedDate, pm.ModifiedUser, " +
                "pm.ModifiedDate, pm.Status " +
                "FROM PaymentMethod pm " +
                "JOIN PaymentPethodTypes t ON(pm.IDPaymentMethodType = t.IDPaymentMethodType) " +
                "WHERE pm.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                $"AND pm.IDClient = {id}";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetMethod(row));
            return list;
        }

        public PaymentMethod GetById(int id)
        {
            string sql = "SELECT pm.IDPaymentMethod, pm.IDPaymentMethodType, t.Name as PaymentMethodTypeName, pm.IDCardType, " +
                "pm.IDClient, pm.CardNumber, pm.ExpDate, pm.CVC, pm.UserName, pm.Password, " +
                "pm.PaymentDate, pm.CreatedUser, pm.CreatedDate, pm.ModifiedUser, " +
                "pm.ModifiedDate, pm.Status " +
                "FROM PaymentMethod pm " +
                "JOIN PaymentPethodTypes t ON(pm.IDPaymentMethodType = t.IDPaymentMethodType) " +
                "WHERE pm.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                $"AND pm.IDPaymentMethod = {id}";
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetMethod(dt.Rows[0]);
            return null;
        }

        public int Save(PaymentMethod method)
        {
            if (method.IDPaymentMethod == 0)
                return Insert(method);
            else
                return Update(method);
        }

        private int Insert(PaymentMethod method)
        {
            string sql = "INSERT INTO PaymentMethod (IDPaymentMethodType, IDCardType, IDClient, CardNumber, ExpDate, CVC, UserName, Password, PaymentDate, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@IDPaymentMethodType, @IDCardType, @IDClient, @CardNumber, @ExpDate, @CVC, @UserName, @Password, @PaymentDate, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status) ";
            string[] parametros = { "@IDPaymentMethodType", "@IDCardType", "@IDClient", "@CardNumber", "@ExpDate", "@CVC", "@UserName", "@Password", "@PaymentDate", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status" };
            object[] valores = { method.IDPaymentMethodType, method.IDCardType, method.IDClient, method.CardNumber, method.ExpDate, method.CVC, method.UserName, method.Password, method.PaymentDate, method.CreatedUser, method.CreatedDate, method.ModifiedUser, method.ModifiedDate, method.Status };
            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(PaymentMethod method)
        {
            string sql = "UPDATE PaymentMethod SET IDPaymentMethodType = @IDPaymentMethodType, IDCardType = @IDCardType, IDClient = @IDClient, CardNumber = @CardNumber, ExpDate = @ExpDate, CVC = @CVC, UserName = @UserName, Password = @Password, PaymentDate = @PaymentDate, CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status " +
                "WHERE IDPaymentMethod = @IDPaymentMethod";
            string[] parametros = { "@IDPaymentMethodType", "@IDCardType", "@IDClient", "@CardNumber", "@ExpDate", "@CVC", "@UserName", "@Password", "@PaymentDate", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@IDPaymentMethod" };
            object[] valores = { method.IDPaymentMethodType, method.IDCardType, method.IDClient, method.CardNumber, method.ExpDate, method.CVC, method.UserName, method.Password, method.PaymentDate, method.CreatedUser, method.CreatedDate, method.ModifiedUser, method.ModifiedDate, method.Status, method.IDPaymentMethod };
            return conexion.Execute(sql, parametros, valores);
        }
    }
}
