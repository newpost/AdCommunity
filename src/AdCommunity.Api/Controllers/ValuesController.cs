using AdCommunity.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly LocalizationService _localizationService;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
            _localizationService = LocalizationServiceFactory.GetLocalizationServiceInstance();
        }
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return _localizationService.Translate("AlreadyExistsErrorMessage", "ValuesController");
        }
    }
}
