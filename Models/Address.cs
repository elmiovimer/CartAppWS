using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Models
{
    public class Address
    {
        public int IDAddress { get; set; }
        public int IDClient { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int IDCity { get; set; }
        public string CityName { get; }
        public int IDState { get; set; }
        public string StateName { get; }
        public string ZipCode { get; set; }
        public bool ByDefault { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }


        public Address()
        {
            IDAddress = 0;
        }

        public Address(int iDAddress, int iDClient, string name, string address1, 
            string address2, int iDCity, string cityName, int iDState, string stateName, 
            string zipCode, bool byDefault, int createdUser, DateTime createdDate, int modifiedUser,
            DateTime modifiedDate, int status)
        {
            IDAddress = iDAddress;
            IDClient = iDClient;
            Name = name;
            Address1 = address1;
            Address2 = address2;
            IDCity = iDCity;
            CityName = cityName;
            IDState = iDState;
            StateName = stateName;
            ZipCode = zipCode;
            ByDefault = byDefault;
            CreatedUser = createdUser;
            CreatedDate = createdDate;
            ModifiedUser = modifiedUser;
            ModifiedDate = modifiedDate;
            Status = status;
        }
    }
}
