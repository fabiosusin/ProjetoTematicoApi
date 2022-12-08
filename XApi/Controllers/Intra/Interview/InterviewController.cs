using Business.API.Intra.BlInterview;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using XApi.Controllers;

namespace API.Controllers.Intra.Interview
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Entrevista"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Interview")]
    public class InterviewController : BaseController<InterviewController>
    {
        public InterviewController(ILogger<InterviewController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlInterview Bl;

        [HttpPost, Route("upsert-Interview")]
        public IActionResult UpsertInterview(DTO.Intra.Interview.Database.Interview input) => Ok(Bl.UpsertInterview(input));

        [HttpGet, Route("get-by-id/{id}")]
        public IActionResult GetInterview(int id) => Ok(Bl.GetInterview(id));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteInterview(int id) => Ok(Bl.DeleteInterview(id));

        [HttpPost, Route("list")]
        public IActionResult ListInterview() => Ok(Bl.List());
    }
}
