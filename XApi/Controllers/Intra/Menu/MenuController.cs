using Business.API.Intra.Menu;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.Menu.Enum;
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

        [HttpGet, Route("get-menu/{type}")]
        public IActionResult GetHubMenu(MenuSystemTypeEnum type) => Ok(BlIntraMenu.GetIntraMenu(type));
    }
}
