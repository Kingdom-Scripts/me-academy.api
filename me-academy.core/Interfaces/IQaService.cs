using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Questions;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IQaService
{
    Task<Result> CreateQuestion(string courseUid, List<QuestionAndAnswerModel> model);
    Task<Result> ListQuestions(string courseUid, PagingOptionModel request);
    Task<Result> AddAnswers(List<QaResponseModel> model);
}