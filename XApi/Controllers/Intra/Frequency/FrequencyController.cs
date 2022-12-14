using Business.API.Intra.BlFrequency;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.General.Api.Input;
using DTO.Intra.Frequency.Input;
using DTO.Intra.FrequencyDB.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Useful.Extensions.FilesExtension;
using XApi.Controllers;

namespace API.Controllers.Intra.FrequencyController
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Frequência"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Frequency")]
    public class FrequencyController : BaseController<FrequencyController>
    {
        public FrequencyController(ILogger<FrequencyController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlFrequency Bl;

        [HttpPost, Route("upsert-Frequency")]
        public IActionResult UpsertFrequency(Frequency input) => Ok(Bl.UpsertFrequency(input));

        [HttpGet, Route("generate-doc/{id}")]
        public IActionResult GenerateDoc(int id)
        {
            var doc = Bl.GenerateDoc(id);
            if (doc == null)
                return BadRequest();

            var fileInfo = new FileInfo(doc.Path);
            return string.IsNullOrEmpty(fileInfo.Extension) ? BadRequest() : Ok(File(FilesExtension.GetByteFromFile(doc.Path), FilesExtension.GetContentType(fileInfo.Extension), doc.Name + fileInfo.Extension));
        }

        [HttpGet, Route("get-by-id/{id}")]
        public IActionResult GetFrequency(int id) => Ok(Bl.GetFrequency(id));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteFrequency(int id) => Ok(Bl.DeleteFrequency(id));

        [HttpPost, Route("list")]
        public IActionResult ListFrequency(FrequencyListInput input) => Ok(Bl.List(input));

        [HttpPost, Route("report")]
        public IActionResult Report(FrequencyListInput input)
        {
            var doc = Bl.GetExcel(input);
            if (doc == null)
                return BadRequest();

            var fileInfo = new FileInfo(doc.Path);
            return string.IsNullOrEmpty(fileInfo.Extension) ? BadRequest() : Ok(File(FilesExtension.GetByteFromFile(doc.Path), FilesExtension.GetContentType(fileInfo.Extension), doc.Name + fileInfo.Extension));
        }

        [HttpPost, Route("export")]
        public IActionResult Export(FrequencyListInput input)
        {
            var doc = Bl.Export(input);
            if (doc == null)
                return BadRequest();

            var fileInfo = new FileInfo(doc.Path);
            return string.IsNullOrEmpty(fileInfo.Extension) ? BadRequest() : Ok(File(FilesExtension.GetByteFromFile(doc.Path), FilesExtension.GetContentType(fileInfo.Extension), doc.Name + fileInfo.Extension));
        }

        [HttpPost, Route("import")]
        public IActionResult Import(ImportFileInput input) => Ok(Bl.Import(input));
    }
}
