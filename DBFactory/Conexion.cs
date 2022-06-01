using CartAppWS.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.DBFactory
{
    public class Conexion
    {
        private readonly SqlConnection conexion;
        private readonly IConfiguration _configuration;
        /*private const string SERVER = "192.168.1.253";
        private const string DATABASE = "DBPedidos";
        private const string USER = "jperez";
        private const string PWD = "Admin01";
        */

        
        
        /*private const string SERVER = "sql5103.site4now.net";
        private const string DATABASE = "DB_A71FC9_Pedidos";
        private const string USER = "DB_A71FC9_Pedidos_admin";
        private const string PWD = "8Oui2xLAUEgG7d3C1";*/
        
        private SqlTransaction transaccion;
        private int lastInsertID;

        public Conexion()
        {
            if (conexion == null)
            {
                _configuration = Constants.configuracion;
                //string strcon = $"Data Source={SERVER};Initial Catalog={DATABASE};User ID={USER};Password={PWD}";
                string strcon = $"Data Source={_configuration["DB_SVR"]};Initial Catalog={_configuration["DB_NAME"]};User ID={_configuration["DB_USR"]};Password={_configuration["DB_PWD"]}";
                conexion = new SqlConnection(strcon);
                transaccion = null;
                lastInsertID = 0;
            }
        }

        private void GetLastInsertID()
        {
            SqlCommand cmd = new SqlCommand("SELECT ISNULL(@@identity,0) as ID", conexion);
            if (transaccion != null)
                cmd.Transaction = transaccion;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
                lastInsertID = Convert.ToInt32(dt.Rows[0]["ID"]);
            else
                lastInsertID = 0;
        }

        public void BeginTrassancion()
        {
            conexion.Open();
            transaccion = conexion.BeginTransaction();
        }

        public void Rollback()
        {
            transaccion.Rollback();
            transaccion = null;
            conexion.Close();
        }

        public void Commit()
        {
            transaccion.Commit();
            transaccion = null;
            conexion.Close();
        }

        public void Open()
        {
            if (conexion.State == System.Data.ConnectionState.Open)
                conexion.Close();
            conexion.Open();
        }

        public void Close()
        {
            conexion.Close();
        }

        public int Execute(string sql)
        {
            if (transaccion == null)
                conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            if (transaccion != null)
                cmd.Transaction = transaccion;
            int i = cmd.ExecuteNonQuery();
            GetLastInsertID();
            if (transaccion == null)
                conexion.Close();
            return i;
        }

        public int Execute(string sql, string[] parametros, Object[] valores)
        {
            if (transaccion == null)
                conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            for (int p = 0; p < parametros.Length; p++)
                cmd.Parameters.AddWithValue(parametros[p], (valores[p] == null ? DBNull.Value : valores[p]));
            if (transaccion != null)
                cmd.Transaction = transaccion;
            int i = cmd.ExecuteNonQuery();
            GetLastInsertID();
            if (transaccion == null)
                conexion.Close();
            return i;
        }
        public DataTable Query(string sql)
        {
            if(transaccion == null)
                conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            if (transaccion != null)
                cmd.Transaction = transaccion;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (transaccion == null)
                conexion.Close();
            return dt;
        }

        public DataTable Query(string sql, string[] parametros, Object[] valores)
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            for (int p = 0; p < parametros.Length; p++)
                cmd.Parameters.AddWithValue(parametros[p], valores[p]);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conexion.Close();
            return dt;
        }

        public int LastInsertID()
        {
            return lastInsertID;
        }
    }
}
