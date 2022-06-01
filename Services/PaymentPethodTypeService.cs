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
    public class PaymentPethodTypeService : IPaymentMethodTypes
    {
        private Conexion conexion;

        public PaymentPethodTypeService()
        {
            conexion = new Conexion();
        }
        public int Delete(PaymentMethodType paymentPethodType)
        {
            string sql = "DELETE FROM PaymentPethodTypes WHERE IDPaymentMethodType = " + paymentPethodType.IDPaymentPethodType;
            return conexion.Execute(sql);
        }

        public List<PaymentMethodType> Get()
        {
            List<PaymentMethodType> list = new List<PaymentMethodType>();
            string sql = "SELECT IDPaymentMethodType, Name, Card, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM PaymentPethodTypes " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "ORDER BY Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetPaymentPethodType(row));
            return list;
        }

        private PaymentMethodType GetPaymentPethodType(DataRow row)
        {
            int id = Convert.ToInt32(row["IDPaymentMethodType"]);
            string name = row["Name"].ToString();
            bool card = Convert.ToBoolean(row["Card"]);
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            return new PaymentMethodType(id, name, card, createdUser, createdDate, modifiedUser, modifiedDate, status);
        }

        public PaymentMethodType GetById(int id)
        {
            string sql = "SELECT IDPaymentMethodType, Name, Card, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM PaymentPethodTypes " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND IDPaymentMethodType = " + id;
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return(GetPaymentPethodType(dt.Rows[0]));
            return null;
        }

        public int Save(PaymentMethodType paymentPethodType)
        {
            PaymentMethodType pmt = paymentPethodType;
            if (pmt.IDPaymentPethodType == 0)
                return Insert(pmt);
            else
                return Update(pmt);
        }

        private int Insert(PaymentMethodType pmt)
        {
            string sql = "INSERT INTO PaymentPethodTypes (Name, Card, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUE (@Name, @Card, " +
                "@CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";

            string[] parametros = {"@Name", "@Card", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status"};

            object[] valores = {pmt.Name, pmt.Card, pmt.CreatedUser, pmt.CreatedDate, pmt.ModifiedUser, pmt.ModifiedDate, pmt.Status};


            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(PaymentMethodType pmt)
        {
            string sql = "UPDATE PaymentPethodTypes SET Name = @Name, Card = @Card, " +
                "CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, " +
                "Status = @Status WHERE IDPaymentMethodType = @IDPaymentMethodType";

            string[] parametros = {"@Name", "@Card", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@IDPaymentMethodType"};

            object[] valores = {pmt.Name, pmt.Card, pmt.CreatedUser, pmt.CreatedDate, pmt.ModifiedUser, pmt.ModifiedDate, pmt.Status, pmt.IDPaymentPethodType};


            return conexion.Execute(sql, parametros, valores);
        }
    }
}
