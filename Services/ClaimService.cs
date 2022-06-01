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
    public class ClaimService : IClaim
    {
        private readonly Conexion conexion;

        public ClaimService()
        {
            conexion = new Conexion();
        }

        public int Delete(Claim claim)
        {
            string sql = $"DELETE FROM Claims WHERE IDClaim = {claim.IDClaim}";
            return conexion.Execute(sql);
        }

        public List<Claim> Get()
        {
            List<Claim> list = new List<Claim>();
            string sql = "SELECT IDClaim, IDOrder, Date, Comment, Status  " +
                "FROM Claims " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetClaim(row));
            return list;
        }

        private Claim GetClaim(DataRow row)
        {
            return new Claim()
            {
                IDClaim = Convert.ToInt32(row["IDClaim"]),
                IDOrder = Convert.ToInt32(row["IDOrder"]),
                Date = Fecha.toDateTimeUTC(row["Date"]),
                Comment = row["Comment"].ToString(), 
                Status = Convert.ToInt32(row["Status"]),
            };
        }

        public Claim GetById(int id)
        {
            string sql = "SELECT IDClaim, IDOrder, Date, Comment, Status  " +
                "FROM Claims " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO + " " +
                $"AND IDClaim = {id}";
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetClaim(dt.Rows[0]);
            return null;
        }

        public List<Claim> GetByOrder(int id)
        {
            List<Claim> list = new List<Claim>();
            string sql = "SELECT IDClaim, IDOrder, Date, Comment, Status  " +
                "FROM Claims " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO + " " +
                $"AND IDOrder = {id}";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetClaim(row));
            return list;
        }

        public int Save(Claim claim)
        {
            if (claim.IDClaim == 0)
                return Insert(claim);
            else
                return Update(claim);
        }

        private int Insert(Claim claim)
        {
            string sql = "INSERT INTO claims (IDOrder, Date, Comment, Status) " +
                "VALUES (@IDOrder, @Date, @Comment, @Status)";
            string[] parametros = { "@IDOrder", "@Date", "@Comment", "@Status"};
            object[] valores = {claim.IDOrder, claim.Date, claim.Comment, claim.Status };
            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(Claim claim)
        {
            string sql = "UPDATE claims SET IDOrder = @IDOrder, Date = @Date, Comment = @Comment, Status = @Status " +
                "WHERE IDClaim = @IDClaim";
            string[] parametros = { "@IDOrder", "@Date", "@Comment", "@Status", "@IDClaim" };
            object[] valores = { claim.IDOrder, claim.Date, claim.Comment, claim.Status, claim.IDClaim };
            return conexion.Execute(sql, parametros, valores);
        }
    }
}
