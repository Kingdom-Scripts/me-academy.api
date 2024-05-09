using me_academy.core.Interfaces;
using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.App;
using me_academy.core.Models.Input.Series;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Series;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers
{
    [ApiController]
    [Route("api/v1/series")]
    public class SeriesController : BaseController
    {
        private readonly ISeriesService _seriesService;

        public SeriesController(ISeriesService seriesService)
            => _seriesService = seriesService ?? throw new ArgumentNullException(nameof(seriesService));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult<SeriesView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> CreateSeries(SeriesModel model)
        {
            var res = await _seriesService.CreateSeries(model);
            return ProcessResponse(res);
        }

        [HttpPatch("{seriesUid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<SeriesView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> UpdateSeries(string seriesUid, SeriesModel model)
        {
            var res = await _seriesService.UpdateSeries(seriesUid, model);
            return ProcessResponse(res);
        }

        [HttpDelete("{seriesUid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> DeleteSeries(string seriesUid)
        {
            var res = await _seriesService.DeleteSeries(seriesUid);
            return ProcessResponse(res);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<SeriesView>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ListSeries([FromQuery] SeriesSearchModel model)
        {
            var res = await _seriesService.ListSeries(model);
            return ProcessResponse(res);
        }

        [HttpGet("{seriesUid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<SeriesDetailView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetSeries(string seriesUid)
        {
            var res = await _seriesService.GetSeries(seriesUid);
            return ProcessResponse(res);
        }

        [HttpPatch("{seriesUid}/view")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ViewSeries(string seriesUid)
        {
            var res = await _seriesService.AddSeriesView(seriesUid);
            return ProcessResponse(res);
        }

        [HttpPatch("{seriesUid}/publish")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> PublishSeries(string seriesUid)
        {
            var res = await _seriesService.PublishSeries(seriesUid);
            return ProcessResponse(res);
        }

        [HttpPatch("{seriesUid}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ActivateSeries(string seriesUid)
        {
            var res = await _seriesService.ActivateSeries(seriesUid);
            return ProcessResponse(res);
        }

        [HttpPatch("{seriesUid}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> DeactivateSeries(string seriesUid)
        {
            var res = await _seriesService.DeactivateSeries(seriesUid);
            return ProcessResponse(res);
        }

        [HttpPatch("{seriesUid}/preview-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<ApiVideoToken>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetPreviewToken(string seriesUid)
        {
            var res = await _seriesService.GetUploadToken(seriesUid);
            return ProcessResponse(res);
        }

        [HttpPost("{seriesUid}/preview")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> SetPreview(string seriesUid, VideoDetailModel model)
        {
            var res = await _seriesService.SetPreviewDetails(seriesUid, model);
            return ProcessResponse(res);
        }

        [HttpPost("{seriesUid}/courses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<SeriesCourseView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> AddNewCourseToSeries(string seriesUid, SeriesNewCourseModel model)
        {
            var res = await _seriesService.AddNewCourseToSeries(seriesUid, model);
            return ProcessResponse(res);
        }

        [HttpPost("{seriesUid}/courses/{courseUid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<SeriesCourseView>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> AddCourseToSeries(string seriesUid, string courseUid, SeriesCourseModel model)
        {
            var res = await _seriesService.AddExistingCourseToSeries(seriesUid, courseUid, model);
            return ProcessResponse(res);
        }

        [HttpDelete("{seriesUid}/courses/{seriesCourseId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        public async Task<IActionResult> RemoveCourseFromSeries(string seriesUid, int seriesCourseId)
        {
            var res = await _seriesService.RemoveCourseFromSeries(seriesUid, seriesCourseId);
            return ProcessResponse(res);
        }
    }
}
