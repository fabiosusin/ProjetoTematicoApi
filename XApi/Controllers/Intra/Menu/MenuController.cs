using Business.API.Intra.Menu;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.Menu
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Menu - Intra"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/Menu")]
    public class MenuController : BaseController<MenuController>
    {
        protected BlIntraMenu Bl;
        public MenuController(ILogger<MenuController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new(settings);
        }

        [HttpGet, Route("get-menu")]
        public IActionResult GetHubMenu() => Ok(BlIntraMenu.GetIntraMenu());
    }
}
