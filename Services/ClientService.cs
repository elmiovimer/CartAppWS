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
    public class ClientService : IClient
    {
        private Conexion conexion;

        public ClientService()
        {
            conexion = new Conexion();
        }

        public int Delete(Client client)
        {
            string sql = "DELETE FROM clients WHERE IDClient = " + client.IDClient;
            return conexion.Execute(sql);
        }

        public List<Client> Get()
        {
            List<Client> list = new List<Client>();
            string sql = "SELECT IDClient, FirstName, LastName, Phone, Email, " +
                "Password, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, " +
                "Status, MacAddress FROM Clients " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetClient(row));
            return list;
        }

        private Client GetClient(DataRow row)
        {
            int idClient = Convert.ToInt32(row["IDClient"]);
            string firstName = row["FirstName"].ToString();
            string lastName = row["LastName"].ToString();
            string phone = row["Phone"].ToString();
            string email = row["Email"].ToString();
            string password = ""; //row["Password"].ToString();
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            string macAddress = row["MacAddress"].ToString();            
            return new Client(idClient, firstName, lastName, phone, email, password, createdUser, createdDate, modifiedUser, modifiedDate, macAddress, status);
        }

        public Client GetById(int id)
        {
            string sql = "SELECT IDClient, FirstName, LastName, Phone, Email, " +
                "Password, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, " +
                "Status, MacAddress FROM Clients " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND IDClient = " + id;
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetClient(dt.Rows[0]);
            return null;
        }

        public Client GetByEmail(string email)
        {
            string sql = "SELECT IDClient, FirstName, LastName, Phone, Email, " +
                "Password, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, " +
                "Status, MacAddress FROM Clients " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "AND Email = @Email";
            string[] parametros = { "@Email" };
            object[] valores = { email };
            DataTable dt = conexion.Query(sql, parametros, valores);
            if (dt.Rows.Count > 0)
                return GetClient(dt.Rows[0]);
            return null;
        }

        public Client Login(string user, string password)
        {
            string sql = "SELECT IDClient, FirstName, LastName, Phone, Email, " +
                "Password, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, " +
                "Status, MacAddress FROM Clients " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "AND Email = @User AND Password = @Password";
            string[] parametros = { "@User", "@Password" };
            object[] valores = { user, password };
            DataTable dt = conexion.Query(sql, parametros, valores);
            if (dt.Rows.Count > 0)
                return GetClient(dt.Rows[0]);
            return null;
        }

        public int Save(Client client)
        {
            if (client.IDClient == 0)
                return Insert(client);
            else
                return Update(client);
        }

        private int Insert(Client client)
        {
            string sql = "INSERT INTO Clients (FirstName, LastName, Phone, Email, " +
                "Password, CreatedUser, CreatedDate, ModifiedUser, ModifiedDate, " +
                "Status, MacAddress)" +
                " VALUES (@FirstName, @LastName, @Phone, @Email, " +
                "@Password, @CreatedUser, @CreatedDate, @ModifiedUser, @ModifiedDate, " +
                "@Status, @MacAddress)";
            
            string[] parametros = {"@FirstName", "@LastName", "@Phone", "@Email",
                "@Password", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate",
                "@Status", "@MacAddress"};

            object[] valores = {client.FirstName, client.LastName, client.Phone, client.Email,
                client.Password, client.CreatedUser, client.CreatedDate, client.ModifiedUser, client.ModifiedDate, 
                client.Status, client.MacAddress};
            
            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(Client client)
        {
            string sql = "UPDATE Clients SET FirstName = @FirstName, LastName = @LastName, Phone = @Phone, Email = @Email, " +
                "CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, " +
                "Status = @Status, MacAddress = @MacAddress WHERE IDClient = @IDClient";
            
            string[] parametros = {"@FirstName", "@LastName", "@Phone", "@Email",
                "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate",
                "@Status", "@MacAddress", "@IDClient"};

            object[] valores = {client.FirstName, client.LastName, client.Phone, client.Email,
                client.CreatedUser, client.CreatedDate, client.ModifiedUser, client.ModifiedDate, 
                client.Status, client.MacAddress, client.IDClient};
            
            return conexion.Execute(sql, parametros, valores);
        }

        public int ChangePassword(int IDClient, string oldPassword, string newPassword)
        {
            string sql = "UPDATE Clients SET Password = @NewPassword, Status = @Status WHERE IDClient = @IDClient AND Password = @OldPassword";

            string[] parametros = { "@NewPassword", "@Status", "@IDClient", "OldPassword" };

            object[] valores = {newPassword, (int)Constants.Status.ACTIVO, IDClient, oldPassword};

            return conexion.Execute(sql, parametros, valores);
        }

        public int ChangePassword(int IDClient, string newPassword)
        {
            string sql = "UPDATE Clients SET Password = @NewPassword, Status = @Status WHERE IDClient = @IDClient ";

            string[] parametros = { "@NewPassword", "@Status", "@IDClient" };

            object[] valores = { newPassword, (int)Constants.Status.PASSWORD_CHANGE_REQUIRED, IDClient};

            return conexion.Execute(sql, parametros, valores);
        }

        public ClientValidation GenerateValidation(int idClient)
        {
            ClientValidation v = new ClientValidation()
            {
                IDClient = idClient,
                UUID = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Status = (int)Constants.Status.ACTIVO,
            };
            string sql = "INSERT INTO ClientsValidations (IDClient, UUID, Date, Status) VALUES (@IDClient, @UUID, @Date, @Status)";
            string[] parametros = { "@IDClient", "@UUID", "@Date", "@Status" };
            object[] valores = {v.IDClient, v.UUID, v.Date, v.Status };
            int i = conexion.Execute(sql, parametros, valores);
            if(i > 0)
            {
                v.IDValidation = conexion.LastInsertID();
                return v;
            }
            else
            {
                return null;
            }
        }

        public Client Validate(string uuid)
        {
            string sql = "SELECT IDClientValidation, IDClient, UUID, Date, Status " +
                "FROM ClientsValidations " +
                "WHERE UUID = @UUID AND Date BETWEEN @FromDate AND @ToDate AND Status = @Status";
            string[] parametros = { "@UUID", "@FromDate", "@ToDate", "@Status" };
            object[] valores = { uuid, DateTime.Now.AddMinutes(-10), DateTime.Now, (int)Constants.Status.ACTIVO};
            DataTable dt = conexion.Query(sql, parametros, valores);
            if(dt.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dt.Rows[0]["IDClient"]);
                Client client = GetById(id);
                client.Status = (int)Constants.Status.ACTIVO;
                Save(client);
                sql = "UPDATE ClientsValidations SET Status  = @Status WHERE UUID = @UUID ";
                parametros = new string[] { "@UUID", "@Status" };
                valores = new object[] { uuid, (int)Constants.Status.ELIMINADO };
                conexion.Execute(sql, parametros, valores);
                return client;
            }
            return null;
        }

        public List<ValidationType> GetValidationTypes()
        {
            List<ValidationType> list = new();
            list.Add(new ValidationType()
            {
                IDValidationType = 1,
                Name = "EMail",
            });
            list.Add(new ValidationType()
            {
                IDValidationType = 2,
                Name = "SMS",
            });
            return list;
        }

        public string GeneratePassword()
        {
            String pool = "ABCDEFGHIJKLMNOPQRSTVUWXYZabcdefghijklmnopqrstvuwxyz0123456789!_#";
            String pwd = "";
            Random r = new Random();
            for (int x = 0; x < 10; x++)
            {
                int i = r.Next(pool.Length);
                pwd += pool[i];
            }
            return pwd;
        }
    }
}
