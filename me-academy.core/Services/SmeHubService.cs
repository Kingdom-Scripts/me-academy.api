using Mapster;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.SmeHub;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.SmeHub;
using Microsoft.AspNetCore.Http;

namespace me_academy.core.Services;

public class SmeHubService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly IFileService _fileService;

    public SmeHubService(MeAcademyContext context, UserSession userSession, IFileService fileService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
    }

    public async Task<Result> CreateSmeHub(SmeHubModel input)
    {
        var smeHub = input.Adapt<SmeHub>();
        smeHub.CreatedById = _userSession.UserId;

        // add document
        var documentResult = await _fileService.UploadFile("sme-hubs", input.File);
        if (!documentResult.Success)
            return new ErrorResult(documentResult.Title, documentResult.Message);

        smeHub.DocumentId = documentResult.Content.Id;

        await _context.AddAsync(smeHub);
        await _context.SaveChangesAsync();

        return new SuccessResult(StatusCodes.Status201Created, smeHub.Adapt<SmeHubView>());
    }
}
