using me_academy.core.Interfaces;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Input.SmeHub;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.SmeHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers
{
    [ApiController]
    [Route("api/v1/sme-hubs")]
    [Authorize]
    public class SmeHubsController : BaseController
    {
        private readonly ISmeHubService _smeHubService;
        public SmeHubsController(ISmeHubService smeHubService)
            => _smeHubService = smeHubService ?? throw new ArgumentNullException(nameof(smeHubService));

        /// <summary>
        /// Save a new SME Hub file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult<SmeHubDetailView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> CreateSmeHub([FromForm] SmeHubModel model)
        {
            var res = await _smeHubService.CreateSmeHub(model);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Update an SME Hub
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{uid}")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<SmeHubDetailView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> UpdateSmeHub(string uid, [FromForm] SmeHubModel model)
        {
            var res = await _smeHubService.UpdateSmeHub(uid, model);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Delete and SME Hub
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("{uid}")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> RemoveSmeHub(string uid)
        {
            var res = await _smeHubService.RemoveSmeHub(uid);
            return ProcessResponse(res);
        }

        /// <summary>
        /// List all the SME Hubs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        //[AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<SmeHubView>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ListSmeHubs([FromQuery] SmeHubSearchModel request)
        {
            var res = await _smeHubService.ListSmeHubs(request);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Get the details of and SME Hub
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("{uid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<SmeHubDetailView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetSmeHubs(string uid)
        {
            var res = await _smeHubService.GetSmeHub(uid);
            return ProcessResponse(res);
        }

        /// <summary>
        /// List the types of SME Hubs
        /// </summary>
        /// <returns></returns>
        [HttpGet("types")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<SmeHubTypeView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ListTypes()
        {
            var res = await _smeHubService.ListTypes();
            return ProcessResponse(res);
        }

        /// <summary>
        /// Activate an SME Hub, making it visible. Only Admins, SuperAdmins, and ManageCourse users can do this
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost("{uid}/activate")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ActivateSmeHub(string uid)
        {
            var res = await _smeHubService.ActivateSmeHub(uid);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Deactivate an SME Hub, making it invisible. Only Admins, SuperAdmins, and ManageCourse users can do this
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost("{uid}/deactivate")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> DeactivateSmeHub(string uid)
        {
            var res = await _smeHubService.DeactivateSmeHub(uid);
            return ProcessResponse(res);
        }
    }
}
