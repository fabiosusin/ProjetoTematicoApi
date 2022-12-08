using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DAO.DBConnection;
using DTO.Intra.User.Input;
using System.Threading.Tasks;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Business.API.Hub.Account;
using System;

namespace XApi.Controllers.Intra.Account
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Conta - Intra"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/User")]
    public class UserController : BaseController<UserController>
    {
        public UserController(ILogger<UserController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlIntraAuth Bl;

        [HttpPost, Route("upsert-user")]
        public IActionResult UpsertAccount(AddUserInput input) => Ok(Bl.UpsertUser(input));

        [HttpGet, Route("get-user-by-email")]
        public IActionResult GetAccountByEmail(string email) => Ok(Bl.FindAccountByEmail(email));

        [HttpPost, Route("send-temp-password/{userEmail}"), AllowAnonymous]
        public async Task<IActionResult> SendTempPassword(string userEmail) => Ok(await Bl.SendTempPassword(userEmail).ConfigureAwait(false));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteUser(int id) => Ok(Bl.DeleteUser(id));

        [HttpPost, Route("list")]
        public IActionResult ListUser(UserListInput input) => Ok(Bl.List(input));
    }
}
