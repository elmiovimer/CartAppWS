using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class User
    {
        public int IDUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Rol { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }

        public User()
        {
            IDUser = 0;
        }

        public User(int iDUser, string userName, string password, int rol, int createdUser, DateTime createdDate, int modifiedUser, DateTime modifiedDate, bool status)
        {
            IDUser = iDUser;
            UserName = userName;
            Password = password;
            Rol = rol;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }
    }
}
