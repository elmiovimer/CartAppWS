using CartAppWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public class DataService : IDataService
    {
        private List<Employee> lstEmployee = new List<Employee>();

        public DataService()
        {
            Employee employee = new Employee();
            employee.ID = 1;
            employee.FirstName = "Joydip";
            employee.lastname = "Kanjilal";
            employee.Address = "India";
            lstEmployee.Add(employee);
            employee = new Employee();
            employee.ID = 2;
            employee.FirstName = "Arelvis";
            employee.lastname = "Mendoza";
            employee.Address = "RD";
            lstEmployee.Add(employee);
            employee = new Employee();
            employee.ID = 3;
            employee.FirstName = "Gregory";
            employee.lastname = "Suarez";
            employee.Address = "RD";
            lstEmployee.Add(employee);
            employee = new Employee();
            employee.ID = 4;
            employee.FirstName = "Wilberth";
            employee.lastname = "Rodriguez";
            employee.Address = "RD";
            lstEmployee.Add(employee);
            employee = new Employee();
            employee.ID = 4;
            employee.FirstName = "Vimer";
            employee.lastname = "Fabian";
            employee.Address = "US";
            lstEmployee.Add(employee);
            employee = new Employee();
            employee.ID = 5;
            employee.FirstName = "Victor Johan";
            employee.lastname = "Palma";
            employee.Address = "Venezuela";
            lstEmployee.Add(employee);
        }

        public List<Employee> Get()
        {
            if (lstEmployee.Count > 0)
                return lstEmployee;
            return null;
        }

        public Employee GetById(int id)
        {
            foreach (var emp in lstEmployee)
            {
                if (emp.ID == id)
                    return emp;
            }
            return null;
        }

        public int Save(Employee employee)
        {
            return 1;
        }
    }
}
