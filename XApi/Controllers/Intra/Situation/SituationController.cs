using Business.API.Intra.BlSituation;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using XApi.Controllers;

namespace API.Controllers.Intra.Situation
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Situação"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Situation")]
    public class SituationController : BaseController<SituationController>
    {
        public SituationController(ILogger<SituationController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlSituation Bl;

        [HttpPost, Route("upsert-Situation")]
        public IActionResult UpsertSituation(DTO.Intra.Situation.Database.Situation input) => Ok(Bl.UpsertSituation(input));

        [HttpGet, Route("get-by-number/{number}")]
        public IActionResult GetSituation(int number) => Ok(Bl.GetSituation(number));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteSituation(Guid id) => Ok(Bl.DeleteSituation(id));

        [HttpPost, Route("list")]
        public IActionResult ListSituation() => Ok(Bl.List());
    }
}
