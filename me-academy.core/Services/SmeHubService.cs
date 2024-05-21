using Mapster;
using me_academy.core.Extensions;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Courses;
using me_academy.core.Models.Input.SmeHub;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.SmeHub;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace me_academy.core.Services;

public class SmeHubService : ISmeHubService
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
        var documentResult = await _fileService.UploadFileInternal("sme-hubs", input.File);
        if (!documentResult.Success)
            return new ErrorResult(documentResult.Title, documentResult.Message);

        smeHub.DocumentId = documentResult.Content.Id;

        await _context.AddAsync(smeHub);
        await _context.SaveChangesAsync();

        return new SuccessResult(StatusCodes.Status201Created, smeHub.Adapt<SmeHubDetailView>());
    }

    public async Task<Result> UpdateSmeHub(string uid, SmeHubModel model)
    {
        var smeHub = await _context.SmeHubs
            .Where(sh => sh.Uid == uid)
            .Include(sh => sh.Document)
            .FirstOrDefaultAsync();

        if (smeHub == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Item not found");

        smeHub = model.Adapt(smeHub);

        if (model.File != null)
        {
            var deletedRes = await _fileService.DeleteFileInternal(smeHub.DocumentId);
            if (!deletedRes.Success)
                return new ErrorResult("Unable to delete existing file, kindly try again later");

            var documentResult = await _fileService.UploadFile("sme-hubs", model.File);
            if (!documentResult.Success)
            {
                return new ErrorResult(documentResult.Title, documentResult.Message);
            }

            smeHub.DocumentId = documentResult.Content.Id;
        }

        await _context.SaveChangesAsync();

        return new SuccessResult(smeHub.Adapt<SmeHubDetailView>());
    }

    public async Task<Result> RemoveSmeHub(string uid)
    {
        var smeHub = await _context.SmeHubs
            .Where(sh => sh.Uid == uid)
            .FirstOrDefaultAsync();

        if (smeHub is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "The resource is not found.");

        _context.Remove(smeHub);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult("Resource is deleted successfully.")
            : new ErrorResult("Unable to save changes, kindly try again later.");
    }

    public async Task<Result> ListSmeHubs(SmeHubSearchModel request)
    {
        if (((request.IsActive.HasValue && request.IsActive.Value) || request.WithDeleted) && !_userSession.IsAnyAdmin)
            return new ForbiddenResult();

        request.SearchQuery = !string.IsNullOrWhiteSpace(request.SearchQuery)
            ? request.SearchQuery.ToLower().Trim()
            : null;

        var smeHubs = _context.SmeHubs.AsQueryable();

        // allow filters only for admin users or users who can manage courses
        smeHubs = _userSession.IsAnyAdmin || _userSession.InRole(RolesConstants.ManageCourse)
            ? smeHubs.Where(sh => !request.IsActive.HasValue || sh.IsActive == request.IsActive.Value)
                .Where(sh => request.WithDeleted || !sh.IsDeleted)
            : smeHubs.Where(sh => sh.IsActive && !sh.IsDeleted);

        var result = await smeHubs
            .Where(sh => string.IsNullOrEmpty(request.SearchQuery)
                || sh.Title.ToLower().Contains(request.SearchQuery))
            .ProjectToType<SmeHubView>()
            .ToPaginatedListAsync(request.PageIndex, request.PageSize);

        return new SuccessResult(result);
    }

    public async Task<Result> GetSmeHub(string uid)
    {
        var smeHub = _context.SmeHubs
            .Where(sh => sh.Uid == uid).AsQueryable();

        if (!_userSession.IsAnyAdmin)
            smeHub = smeHub.Where(sh => !sh.IsDeleted && sh.IsActive);

        var result = await smeHub
            .ProjectToType<SmeHubDetailView>()
            .FirstOrDefaultAsync();

        if (result is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "The Sme Hub you requested cannot be found");

        return new SuccessResult(result);
    }

    public async Task<Result> ListTypes()
    {
        var result = await _context.SmeHubTypes
            .ProjectToType<SmeHubTypeView>()
            .ToListAsync();

        return new SuccessResult(result);
    }
}
