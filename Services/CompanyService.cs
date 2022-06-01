using CartAppWS.DBFactory;
using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public class CompanyService : ICompany
    {
        public readonly Conexion conexion;

        public CompanyService()
        {
            conexion = new Conexion();
        }
        public Company Get()
        {
            string sql = "SELECT TOP 1 IDCompany, Name, Address, City, State, ZipCode, Phone, SMSPhone, SMSUser, SMSPassword, Email, " +
                "EmailPassword, SMTPServer, StartTLS, SMTPPort, SMTPAuthentication, WSLink, AuthorizeNETLoginID, AuthorizeNETTransKey, Logo " +
                "FROM Company";
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetCompany(dt.Rows[0]);
            return null;
        }

        public void Update(string foto)
        {
            String sql = "UPDATE company SET Logo = @foto";
            string[] parametros = { "@foto" };
            object[] valores = { foto };
            conexion.Execute(sql, parametros, valores);
        }

        private Company GetCompany(DataRow row)
        {
            return new Company()
            {
                IDCompany = Convert.ToInt32(row["IDCompany"]),
                Name = row["Name"].ToString(),
                Address = row["Address"].ToString(),
                City = row["City"].ToString(),
                State = row["State"].ToString(),
                ZipCode = row["Zipcode"].ToString(),
                Phone = row["Phone"].ToString(),
                SMSPhone = row["SMSPhone"].ToString(),
                SMSUser = row["SMSUser"].ToString(),
                SMSPassword = row["SMSPassword"].ToString(),
                Email = row["Email"].ToString(),
                EmailPassword = row["EmailPassword"].ToString(),
                SMTPServer = row["SMTPServer"].ToString(),
                StartTLS = row["StartTLS"].ToString(),
                SMTPPort = Convert.ToInt32(row["SMTPPort"]),
                SMTPAuthentication = row["SMTPAuthentication"].ToString(),
                WSLink = row["WSLink"].ToString(),
                AuthorizeNETLoginID = row["AuthorizeNETLoginID"].ToString(),
                AuthorizeNETTransKey = row["AuthorizeNETTransKey"].ToString(),
                Logo = row["Logo"].ToString(),
            };
        }
    }
}
