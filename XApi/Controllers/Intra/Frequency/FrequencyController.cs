using Business.API.Intra.BlFrequency;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.FrequencyDB.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

        [HttpGet, Route("get-by-id/{id}")]
        public IActionResult GetFrequency(Guid id) => Ok(Bl.GetFrequency(id));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteFrequency(Guid id) => Ok(Bl.DeleteFrequency(id));

        [HttpPost, Route("list")]
        public IActionResult ListFrequency() => Ok(Bl.List());
    }
}
