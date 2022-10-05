using Business.API.Intra.Employee;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.Employee.Database;
using DTO.Intra.Employee.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.Employee
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Funcionario"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Employee")]
    public class EmployeeController : BaseController<EmployeeController>
    {
        public EmployeeController(ILogger<EmployeeController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlEmployee Bl;

        [HttpPost, Route("upsert-employee")]
        public IActionResult UpsertCustomer(DTO.Intra.Employee.Database.Employee input) => Ok(Bl.UpsertEmployee(input));

        [HttpGet, Route("get-by-document/{cpfCnpj}")]
        public IActionResult GetCustomer(string cpfCnpj) => Ok(Bl.GetEmployee(cpfCnpj));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteCustomer(string id) => Ok(Bl.DeleteEmployee(id));

        [HttpPost, Route("list")]
        public IActionResult ListCustomer(EmployeeListInput input) => Ok(Bl.List(input));
    }
}
