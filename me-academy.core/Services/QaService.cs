using Mapster;
using me_academy.core.Extensions;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Questions;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Questions;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Services;

public class QaService : IQaService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;

    public QaService(MeAcademyContext context, UserSession userSession)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
    }

    public async Task<Result> CreateQuestion(string courseUid, List<QuestionAndAnswerModel> model)
    {
        int courseId = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .Select(c => c.Id)
            .FirstOrDefaultAsync();

        if (courseId == 0)
            return new ErrorResult("Course not found");

        var newQuestion = model.Adapt<List<QuestionAndAnswer>>();
        newQuestion.ForEach(q =>
        {
            q.CourseId = courseId;
            q.CreatedById = _userSession.UserId;
        });

        await _context.QuestionAndAnswers.AddRangeAsync(newQuestion);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult("Question created successfully")
            : new ErrorResult("Failed to create question");
    }

    public async Task<Result> ListQuestions(string courseUid, PagingOptionModel request)
    {
        var questions = await _context.QuestionAndAnswers
            .Where(q => q.Course!.Uid == courseUid)
            .ProjectToType<QuestionView>()
            .ToPaginatedListAsync(request.PageIndex, request.PageSize);

        return new SuccessResult(questions);
    }

    public async Task<Result> AddAnswers(List<QaResponseModel> model)
    {
        var answers = model.Select(m => new QaResponse
        {
            QaId = m.QaId,
            Answer = m.Answer,
            OptionId = m.OptionId,
            CreatedById = _userSession.UserId
        });

        await _context.AddRangeAsync(answers);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult("Answers added successfully")
            : new ErrorResult("Failed to add answers");
    }
}