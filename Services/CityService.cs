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
    public class CityService : ICity
    {
        private Conexion conexion;

        public CityService()
        {
            conexion = new Conexion();
        }

        public int Delete(City city)
        {
            string sql = "DELETE FROM Cities WHERE IDCity = " + city.IDCity;
            return conexion.Execute(sql);
        }

        public List<City> Get()
        {
            List<City> list = new List<City>();
            string sql = "SELECT c.IDCity, c.IDState, s.Name as StateName, c.Name, " +
                "c.CreatedUser, c.CreatedDate, c.ModifiedUser, c.ModifiedDate, " +
                "c.Status " +
                "FROM Cities c " +
                "JOIN States s ON (c.IDState = s.IDState) " +
                "WHERE c.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "ORDER BY s.Name, c.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetCity(row));
            return list;
        }

        private City GetCity(DataRow row)
        {
            int idCity = Convert.ToInt32(row["IDCity"]);
            int idState = Convert.ToInt32(row["IDState"]);
            string stateName = row["StateName"].ToString();
            string name = row["Name"].ToString();
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            return new City(idCity, idState, stateName, name, createdUser, createdDate, modifiedUser, modifiedDate, status);
        }

        public City GetById(int id)
        {
            string sql = "SELECT c.IDCity, c.IDState, s.Name as StateName, c.Name, " +
                "c.CreatedUser, c.CreatedDate, c.ModifiedUser, c.ModifiedDate, " +
                "c.Status " +
                "FROM Cities c " +
                "JOIN States s ON (c.IDState = s.IDState) " +
                "WHERE c.Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND c.IDCity = " + id + " " +
                "ORDER BY s.Name, c.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                return GetCity(row);
            return null;
        }

        public List<City> GetByState(int id)
        {
            List<City> list = new List<City>();
            string sql = "SELECT c.IDCity, c.IDState, s.Name as StateName, c.Name, " +
                "c.CreatedUser, c.CreatedDate, c.ModifiedUser, c.ModifiedDate, " +
                "c.Status " +
                "FROM Cities c " +
                "JOIN States s ON (c.IDState = s.IDState) " +
                "WHERE c.IDState = " + id + " " +
                "ORDER BY s.Name, c.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetCity(row));
            return list;
        }

        public int Save(City city)
        {
            if (city.IDCity == 0)
                return Insert(city);
            else
                return Update(city);
        }

        private int Insert(City city)
        {
            string sql = "INSERT INTO Cities (IDState, Name, CreatedUser, CreatedDate, " +
                "ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@IDState, @Name, @CreatedUser, @CreatedDate, " +
                "@ModifiedUser, ModifiedDate, @Status)";

            string[] parametros = {"@IDState", "@Name", "@CreatedUser", "@CreatedDate", 
                "@ModifiedUser", "ModifiedDate", "@Status"};

            object[] valores = {city.IDState, city.Name, city.CreatedUser, city.CreatedDate, 
                city.ModifiedUser, city.ModifiedDate, city.Status};

            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(City city)
        {
            string sql = "UPDATE Cities SET IDState = @IDState, Name = @Name, CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, " +
                "ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status  = @Status WHERE IDCity = @IDCity";
           
            string[] parametros = {"@IDState", "@Name", "@CreatedUser", "@CreatedDate", 
                "@ModifiedUser", "ModifiedDate", "@Status", "@IDCity"};

            object[] valores = {city.IDState, city.Name, city.CreatedUser, city.CreatedDate, 
                city.ModifiedUser, city.ModifiedDate, city.Status, city.IDCity};

            return conexion.Execute(sql, parametros, valores);
        }
    }
}
