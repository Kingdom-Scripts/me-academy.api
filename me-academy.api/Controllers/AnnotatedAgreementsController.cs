using me_academy.core.Interfaces;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.AnnotatedAgreement;
using me_academy.core.Models.View.AnnotatedAgreement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using me_academy.core.Models.Input.AnnotatedAgreements;

namespace me_academy.api.Controllers
{
    [ApiController]
    [Route("api/v1/annotated-agreements")]
    public class AnnotatedAgreementsController : BaseController
    {
        private readonly IAnnotatedAgreementService _smeHubService;
        public AnnotatedAgreementsController(IAnnotatedAgreementService smeHubService)
            => _smeHubService = smeHubService ?? throw new ArgumentNullException(nameof(smeHubService));

        /// <summary>
        /// Save a new Annotated Agreement file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult<AnnotatedAgreementDetailView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> CreateAnnotatedAgreement([FromForm] AnnotatedAgreementModel model)
        {
            var res = await _smeHubService.CreateAnnotatedAgreement(model);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Update an Annotated Agreement
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{uid}")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<AnnotatedAgreementDetailView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> UpdateAnnotatedAgreement(string uid, [FromForm] AnnotatedAgreementModel model)
        {
            var res = await _smeHubService.UpdateAnnotatedAgreement(uid, model);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Delete and Annotated Agreement
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpDelete("{uid}")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> RemoveAnnotatedAgreement(string uid)
        {
            var res = await _smeHubService.RemoveAnnotatedAgreement(uid);
            return ProcessResponse(res);
        }

        /// <summary>
        /// List all the Annotated Agreements
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<AnnotatedAgreementView>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ListAnnotatedAgreements([FromQuery] AnnotatedAgreementSearchModel request)
        {
            var res = await _smeHubService.ListAnnotatedAgreements(request);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Get the details of and Annotated Agreement
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("{uid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<AnnotatedAgreementDetailView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAnnotatedAgreements(string uid)
        {
            var res = await _smeHubService.GetAnnotatedAgreement(uid);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Activate an Annotated Agreement, making it visible. Only Admins, SuperAdmins, and ManageCourse users can do this
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPatch("{uid}/activate")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ActivateAnnotatedAgreement(string uid)
        {
            var res = await _smeHubService.ActivateAnnotatedAgreement(uid);
            return ProcessResponse(res);
        }

        /// <summary>
        /// Deactivate an Annotated Agreement, making it invisible. Only Admins, SuperAdmins, and ManageCourse users can do this
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPatch("{uid}/deactivate")]
        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)},{nameof(Roles.ManageCourse)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> DeactivateAnnotatedAgreement(string uid)
        {
            var res = await _smeHubService.DeactivateAnnotatedAgreement(uid);
            return ProcessResponse(res);
        }
    }
}
