using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public interface IClient
    {
        List<Client> Get();
        Client GetById(int id);
        Client GetByEmail(string email);
        Client Login(string user, string password);
        int Save(Client client);
        int Delete(Client client);
        List<ValidationType> GetValidationTypes();
        ClientValidation GenerateValidation(int id);
        int ChangePassword(int id, string oldPassword, string newPassword);
        int ChangePassword(int id, string newPassword);
        Client Validate(string uuid);
        String GeneratePassword();
    }
}
