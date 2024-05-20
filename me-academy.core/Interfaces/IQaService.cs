using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Questions;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IQuestionService
{
    Task<Result> CreateCourseQuestion(string courseUid, List<QuestionAndAnswerModel> model);
    Task<Result> ListCourseQuestions(string courseUid);
    Task<Result> AddAnswersForCourse(string courseUid, List<QuestionResponseModel> model);

    Task<Result> AddQuestionToSeries(string seriesUid, QuestionAndAnswerModel model);
    Task<Result> UpdateSeriesQuestion(string seriesUid, QuestionAndAnswerModel model);
    Task<Result> DeleteSeriesQuestion(string seriesUid, int questionId);
    Task<Result> ListSeriesQuestions(string seriesUid);
    Task<Result> AddAnswersForSeries(string seriesUid, List<QuestionResponseModel> model);
}