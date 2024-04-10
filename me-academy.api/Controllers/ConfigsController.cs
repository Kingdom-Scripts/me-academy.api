using me_academy.core.Interfaces;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Config;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/configs")]
public class ConfigsController : BaseController
{
    private readonly IConfigService _configService;

    public ConfigsController(IConfigService configService) =>
        _configService = configService ?? throw new ArgumentNullException(nameof(configService));

    /// <summary>
    /// List all pricing durations
    /// </summary>
    /// <returns></returns>
    [HttpGet("durations")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<DurationView>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ListDurations()
    {
        var res = await _configService.ListDurations();
        return ProcessResponse(res);
    }
}