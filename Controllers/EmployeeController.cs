using CartAppWS.Models;
using CartAppWS.Services;
using CartAppWS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;


namespace CartAppWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : ControllerBase
    {
        private readonly IDataService _dataService;

        public EmployeeController()
        {
            _dataService = new DataService();
        }

        #region Get
        //GET : Employee/GetEmpleados
        [HttpGet("getempleados")]
        public List<Employee> GetEmpleados()
        {
            try
            {
                return _dataService.Get();
            }
            catch (Exception ex)
            {
                return new List<Employee>() {
                    new Employee(){FirstName=ex.Message }
                };
            }
        }

        [HttpGet("getempleadosbyid")]
        public Employee GetEmpleadosByID(int id)
        {

            return _dataService.GetById(id);
            

        }
        #endregion

        #region Post

        [HttpPost("createemployee")]
        public Estado CreateEmployee(Employee employee)
        {
            try
            {
                int i = _dataService.Save(employee);
                if (i > 0)
                    return new Estado(1, "Creacion satisfactria", "");
                else
                    return new Estado(0, "Ninguna fila afectada", "");
            }
            catch (Exception e)
            {
                return new Estado(500, "INTERNAL SERVER ERROR", e.Message);
            }

        }

        #endregion

    }
}
