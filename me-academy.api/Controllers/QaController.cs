using me_academy.core.Interfaces;
using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Questions;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Questions;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/qa")]
public class QaController : BaseController
{
    private readonly IQaService _qaService;

    public QaController(IQaService qaService)
        => _qaService = qaService ?? throw new ArgumentNullException(nameof(qaService));

    /// <summary>
    /// Create a new question for a course
    /// </summary>
    /// <param name="courseUid"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("{courseUid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> CreateQuestion(string courseUid, List<QuestionAndAnswerModel> model)
    {
        var res = await _qaService.CreateQuestion(courseUid, model);
        return ProcessResponse(res);
    }

    /// <summary>
    /// List questions for a course
    /// </summary>
    /// <param name="courseUid"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("{courseUid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<QuestionView>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ListQuestions(string courseUid, [FromQuery]PagingOptionModel request)
    {
        var res = await _qaService.ListQuestions(courseUid, request);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Submit answers to a question
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("answers")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> AddAnswers(List<QaResponseModel> model)
    {
        var res = await _qaService.AddAnswers(model);
        return ProcessResponse(res);
    }
}