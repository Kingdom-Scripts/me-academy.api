using me_academy.core.Interfaces;
using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Questions;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Questions;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/question")]
public class QuestionController : BaseController
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
        => _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));

    /// <summary>
    /// Create a new question for a course
    /// </summary>
    /// <param name="courseUid"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("courses/{courseUid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> CreateQuestion(string courseUid, List<QuestionAndAnswerModel> model)
    {
        var res = await _questionService.CreateCourseQuestion(courseUid, model);
        return ProcessResponse(res);
    }

    /// <summary>
    /// List questions for a course
    /// </summary>
    /// <param name="courseUid"></param>
    /// <returns></returns>
    [HttpGet("courses/{courseUid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<CourseQuestionView>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ListQuestions(string courseUid)
    {
        var res = await _questionService.ListCourseQuestions(courseUid);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Submit answers to a question
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("courses/{courseUid}/answers")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> AddAnswers(string courseUid, List<QuestionResponseModel> model)
    {
        var res = await _questionService.AddAnswersForCourse(courseUid, model);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Add a question to a series
    /// </summary>
    /// <param name="seriesUid"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("series/{seriesUid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> AddQuestionToSeries(string seriesUid, QuestionAndAnswerModel model)
    {
        var res = await _questionService.AddQuestionToSeries(seriesUid, model);
        return ProcessResponse(res);
    }

/// <summary>
/// Update the question of a series
/// </summary>
/// <param name="seriesUid"></param>
/// <param name="model"></param>
/// <returns></returns>
    [HttpPatch("series/{seriesUid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> UpdateSeriesQuestion(string seriesUid, QuestionAndAnswerModel model) {
        var res = await _questionService.UpdateSeriesQuestion(seriesUid, model);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Delete a question from series
    /// </summary>
    /// <param name="seriesUid"></param>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [HttpDelete("series/{seriesUid}/{questionId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> DeleteSeriesQuestion(string seriesUid, int questionId) {
        var res = await _questionService.DeleteSeriesQuestion(seriesUid, questionId);
        return ProcessResponse(res);
    }

    
    /// <summary>
    /// List questions for a series
    /// </summary>
    /// <param name="seriesUid"></param>
    /// <returns></returns>
    [HttpGet("series/{seriesUid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<SeriesQuestionView>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ListSeriesQuestions(string seriesUid)
    {
        var res = await _questionService.ListSeriesQuestions(seriesUid);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Submit answers to a question for series
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("series/{seriesUid}/answers")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> AddSeriesAnswers(string seriesUid, List<QuestionResponseModel> model)
    {
        var res = await _questionService.AddAnswersForSeries(seriesUid, model);
        return ProcessResponse(res);
    }
}