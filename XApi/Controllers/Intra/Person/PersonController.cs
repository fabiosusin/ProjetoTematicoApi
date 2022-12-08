using Business.API.Intra.BlPerson;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.Person.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Useful.Extensions.FilesExtension;

namespace XApi.Controllers.Intra.Person
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Pessoa"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Person")]
    public class CompanyController : BaseController<CompanyController>
    {
        public CompanyController(ILogger<CompanyController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlPerson Bl;

        [HttpPost, Route("upsert-Person")]
        public IActionResult UpsertCustomer(DTO.Intra.Person.Database.Person input) => Ok(Bl.UpsertPerson(input));

        [HttpGet, Route("get-by-document/{cpfCnpj}")]
        public IActionResult GetCustomer(string cpfCnpj) => Ok(Bl.GetPerson(cpfCnpj));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteCustomer(int id) => Ok(Bl.DeletePerson(id));

        [HttpPost, Route("list")]
        public IActionResult ListCustomer(PersonListInput input) => Ok(Bl.List(input));

        [HttpGet, Route("export")]
        public IActionResult Export()
        {
            var doc = Bl.Export();
            if (doc == null)
                return BadRequest();

            var fileInfo = new FileInfo(doc.Path);
            return string.IsNullOrEmpty(fileInfo.Extension) ? BadRequest() : Ok(File(FilesExtension.GetByteFromFile(doc.Path), FilesExtension.GetContentType(fileInfo.Extension), doc.Name + fileInfo.Extension));
        }
    }
}
