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
    public class AddressService : IAddress
    {
        private readonly Conexion conexion;

        public AddressService()
        {
            conexion = new Conexion();
        }
        public int Delete(Address address)
        {
            string sql = "DELETE FROM Addresses WHERE IDAddress = " + address.IDAddress;
            return conexion.Execute(sql);
        }

        public List<Address> Get()
        {
            List<Address> list = new List<Address>();
            string sql = "SELECT a.IDAddress, a.IDClient, a.Name, " +
                "a.Address1, a.Address2, a.IDCity, c.Name as CityName, " +
                "a.IDState, s.Name as StateName, a.ZipCode, a.ByDefault, " +
                "a.CreatedUser, a.CreatedDate, a.ModifiedUser, a.ModifiedDate, " +
                "a.Status FROM Addresses a " +
                "JOIN Cities c ON a.IDCity = c.IDCity " +
                "JOIN States s ON a.IDState = s.IDState " +
                "ORDER BY a.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetAddress(row));
            return list;
        }

        private Address GetAddress(DataRow row)
        {
            int idAddress = Convert.ToInt32(row["IDAddress"]);
            int idClient = Convert.ToInt32(row["IDClient"]);
            string name = row["Name"].ToString();
            string address1 = row["Address1"].ToString();
            string address2 = row["Address2"].ToString(); //change
            int idCity = Convert.ToInt32(row["IDCity"]);
            string cityName = row["CityName"].ToString();
            int idState = Convert.ToInt32(row["IDState"]);
            string stateName = row["StateName"].ToString();
            bool byDefault = Convert.ToBoolean(row["ByDefault"]);
            string zipCode = row["ZipCode"].ToString();
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            return new Address(idAddress, idClient, name, address1, address2, idCity,cityName,idState,stateName,
                zipCode, byDefault, createdUser, createdDate, modifiedUser, modifiedDate, status);
        }

        public List<Address> GetByClient(int id)
        {
            List<Address> list = new List<Address>();
            string sql = "SELECT a.IDAddress, a.IDClient, a.Name, " +
                "a.Address1, a.Address2, a.IDCity, c.Name as CityName, " +
                "a.IDState, s.Name as StateName, a.ZipCode, a.ByDefault, " +
                "a.CreatedUser, a.CreatedDate, a.ModifiedUser, a.ModifiedDate, " +
                "a.Status FROM Addresses a " +
                "JOIN Cities c ON a.IDCity = c.IDCity " +
                "JOIN States s ON a.IDState = s.IDState " +
                "WHERE IDClient = " + id + " " +
                "ORDER BY a.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetAddress(row));
            return list;
        }

        public Address GetById(int id)
        {
            string sql = "SELECT a.IDAddress, a.IDClient, a.Name, " +
                "a.Address1, a.Address2, a.IDCity, c.Name as CityName, " +
                "a.IDState, s.Name as StateName, a.ZipCode, a.ByDefault, " +
                "a.CreatedUser, a.CreatedDate, a.ModifiedUser, a.ModifiedDate, " +
                "a.Status FROM Addresses a " +
                "JOIN Cities c ON a.IDCity = c.IDCity " +
                "JOIN States s ON a.IDState = s.IDState " +
                "WHERE IDAddress = " + id + " " +
                "ORDER BY a.Name";
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetAddress(dt.Rows[0]);
            return null;
        }

        public int Save(Address address)
        {
            if (address.IDAddress == 0)
                return Insert(address);
            else
                return Update(address);
        }

        private int Insert(Address address)
        {
            string sql = "INSERT INTO Addresses (IDClient, Name, " +
                "Address1, Address2, IDCity, IDState, ZipCode, ByDefault, " +
                "CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@IDClient, @Name, @Address1, @Address2, @IDCity, @IDState, " +
                "@ZipCode, @ByDefault, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";
            string[] parametros = { "@IDClient", "@Name", "@Address1", "@Address2", "@IDCity", "@IDState", 
                "@ZipCode", "@ByDefault","@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status" };
            object[] valores = {address.IDClient, address.Name, address.Address1, address.Address2, address.IDCity, address.IDState,
            address.ZipCode, address.ByDefault, address.CreatedUser, address.CreatedDate, address.ModifiedUser, address.ModifiedDate, address.Status
            };
            UnsetDefault(address);
            
            return  conexion.Execute(sql, parametros, valores);     
        }

        private int Update(Address address)
        {
            string sql = "UPDATE Addresses SET IDClient = @IDClient, Name = @Name, " +
                "Address1 = @Address1, Address2 = @Address2, IDCity = @IDCity, IDState = @IDState, ZipCode = @ZipCode, ByDefault = @ByDefault, " +
                "CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status " +
                "WHERE IDAddress = @IDAddress";

            string[] parametros = { "@IDClient", "@Name", "@Address1", "@Address2", "@IDCity", "@IDState",
                "@ZipCode", "@ByDefault", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@IDAddress" };
            object[] valores = {address.IDClient, address.Name, address.Address1, address.Address2, address.IDCity, address.IDState,
            address.ZipCode, address.ByDefault, address.CreatedUser, address.CreatedDate, address.ModifiedUser, address.ModifiedDate, address.Status, address.IDAddress
            };
            UnsetDefault(address);
            return conexion.Execute(sql, parametros, valores); 
        }

        private void UnsetDefault(Address address)
        {
            if (address.ByDefault)
                conexion.Execute($"UPDATE Addresses SET ByDefault = 'False' WHERE IDClient = {address.IDClient}");
        }
    }
}
