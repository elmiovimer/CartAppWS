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
    public class UserService : IUser
    {
        private Conexion conexion;

        public UserService()
        {
            conexion = new Conexion();
        }

        public int Delete(User user)
        {
            string sql = "DELETE FROM Users WHERE IDUser = " + user.IDUser;
            return conexion.Execute(sql);
        }

        public List<User> Get()
        {
            List<User> list = new List<User>();
            string sql = "SELECT IDUser, UserName, Password, Rol, Status, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate " +
                "FROM Users " + 
                "WHERE Status = 1"; 
            DataTable dt = conexion.Query(sql);
            foreach(DataRow row in dt.Rows)
            {
                list.Add(GetUser(row));
            }
            return list;
        }

        private User GetUser(DataRow row)
        {
            int idUser = Convert.ToInt32(row["IDUser"]);
            string userName = row["UserName"].ToString();
            string password = row["Password"].ToString();
            int rol = Convert.ToInt32(row["Rol"]);
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]); 
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            bool status = Convert.ToBoolean(row["Status"]);
            return new User(idUser, userName, password, rol, createdUser, createdDate, modifiedUser, modifiedDate, status);
        }

        public User GetById(int id)
        {
            string sql = "SELECT IDUser, UserName, Password, Rol, Status, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate " +
                "FROM Users " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND IDUser = " + id;
            DataTable dt = conexion.Query(sql);
            if(dt.Rows.Count > 0)
            {
                return GetUser(dt.Rows[0]);
            }
            return null;
        }

        public User Login(string usr, string pwd)
        {
            string sql = $"SELECT IDUser, UserName, Password, Rol, Status, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate FROM Users " +
                "WHERE Status = " + (int) Constants.Status.ACTIVO + " " +
                "AND UserName = @User AND Password = @Password" ;
            string[] parametros = { "@User", "@Password"};
            Object[] valores = {usr, pwd };
            DataTable dt = conexion.Query(sql, parametros, valores);
            if (dt.Rows.Count > 0)
            {
                return GetUser(dt.Rows[0]);
            }
            return null;
        }

        public int Save(User user)
        {
            if (user.IDUser == 0)
                return Insert(user);
            else
                return Update(user);
        }

        private int Insert(User user)
        {
            string sql = "INSERT INTO Users (UserName, Password, Rol, Status, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate) " +
                "VALUES (@UserName, @Password, @Rol, @Status, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate)";
            
            string[] parametros = { "@UserName", "@Password", "@Rol", "@CreatedUser", "@Status", "@CreatedDate", "@ModifiedUser", "@ModifiedDate"};
            
            Object[] valores = {user.UserName, user.Password, user.Rol, user.Status, user.CreatedUser, Fecha.ToSQL(user.CreatedDate), user.ModifiedUser, Fecha.ToSQL(user.ModifiedDate)};
            
            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(User user)
        {
            string sql = "UPDATE Users SET UserName = @UserName, Password = @Password, Rol = @Rol, Status = @Status, " +
                "CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate " +
                "WHERE IDUser = @IDUser";
            
            string[] parametros = { "@UserName", "@Password", "@Rol", "@CreatedUser", "@Status", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@IDUser" };
            
            Object[] valores = { user.UserName, user.Password, user.Rol, user.Status, user.CreatedUser, Fecha.ToSQL(user.CreatedDate), user.ModifiedUser, Fecha.ToSQL(user.ModifiedDate), user.IDUser };
            
            return conexion.Execute(sql, parametros, valores);
        }
    }
}
