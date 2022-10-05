using Business.API.Intra.Home;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.Home
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Home - Intra"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/[controller]")]
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(ILogger<HomeController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlIntraHome Bl;

        [HttpGet, Route("get-data")]
        public IActionResult GetData() => Ok(Bl.GetHomeData());
    }
}
