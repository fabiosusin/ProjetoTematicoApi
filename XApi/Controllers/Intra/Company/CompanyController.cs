using Business.API.Intra.BlCompany;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.Company.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.Company
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Empresa"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Company")]
    public class CompanyController : BaseController<CompanyController>
    {
        public CompanyController(ILogger<CompanyController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlCompany Bl;

        [HttpPost, Route("upsert-Company")]
        public IActionResult UpsertCompany(DTO.Intra.Company.Database.Company input) => Ok(Bl.UpsertCompany(input));

        [HttpGet, Route("get-by-document/{cpfCnpj}")]
        public IActionResult GetCompany(string cpfCnpj) => Ok(Bl.GetCompany(cpfCnpj));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteCompany(string id) => Ok(Bl.DeleteCompany(id));

        [HttpPost, Route("list")]
        public IActionResult ListCompany(CompanyListInput input) => Ok(Bl.List(input));
    }
}
