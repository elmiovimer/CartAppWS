using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Client
    {
        public int IDClient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string MacAddress { get; set; }

        public int Status { get; set; }


        public Client()
        {
            IDClient = 0;
            this.CreatedDate = DateTime.Now;
        }

        public Client(int iDClient, string firstName, string lastName, string phone, string email, string password, int createdUser, DateTime createdDate, int modifiedUser, DateTime modifiedDate, string macAddress, int status)
        {
            IDClient = iDClient;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Password = password;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            MacAddress = macAddress;
            Status = status;
        }

    }
}
