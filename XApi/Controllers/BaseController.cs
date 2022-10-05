using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class BaseController<T> : ControllerBase
    {
        private readonly ILogger<T> _logger;
        
        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}
