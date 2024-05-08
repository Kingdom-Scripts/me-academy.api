using Mapster;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Series;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Series;
using me_academy.core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Services;

public class SeriesService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;

    public SeriesService(MeAcademyContext context, UserSession userSession)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
    }

    public async Task<Result> CreateSeries(SeriesModel model)
    {
        bool seriesExists = await _context.Series
            .AnyAsync(x => x.Title.ToLower().Trim() == model.Title.ToLower().Trim());

        if (seriesExists)
            return new ErrorResult("Series with the same title already exists.");

        // create new series object
        var series = model.Adapt<Series>();
        series.CreatedById = _userSession.UserId;
        series.Uid = await GetSeriesUid(model.Title);

        // add prices
        if (model.Prices.Any())
        {
            series.SeriesPrices = model.Prices.Select(p => new SeriesPrice
            {
                Price = p.Price,
                DurationId = p.DurationId
            }).ToList();
        }

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Created(series.Title, _userSession.Name));

        // save the data
        await _context.AddAsync(series);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status201Created, series.Adapt<SeriesView>())
            : new ErrorResult("Failed to create series.");
    }

    public async Task<Result> UpdateSeries(string seriesUid, SeriesModel model)
    {
        var series = await _context.Series
            .Include(x => x.SeriesPrices)
            .FirstOrDefaultAsync(x => x.Uid == seriesUid);

        if (series == null)
            return new ErrorResult("Series not found.");

        // update series object
        series.Title = model.Title;
        series.Uid = await GetSeriesUid(model.Title);
        series.UpdatedById = _userSession.UserId;
        series.UpdatedOnUtc = DateTime.UtcNow;
        series.UseFulLinks = new();

        // update prices
        if (model.Prices.Any())
        {
            // remove existing prices
            _context.SeriesPrices.RemoveRange(series.SeriesPrices);

            // add new prices
            series.SeriesPrices = model.Prices.Select(p => new SeriesPrice
            {
                Price = p.Price,
                DurationId = p.DurationId
            }).ToList();
        }

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Updated(series.Title, _userSession.Name));

        // save the data
        _context.Update(series);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, series.Adapt<SeriesView>())
            : new ErrorResult("Failed to update series.");
    }

    #region PRIVATE METHODS

    private async Task<string> GetSeriesUid(string title)
    {
        var trimmedTitle = title.Trim() // trim
            .ToLower().Replace("-", "", StringComparison.OrdinalIgnoreCase) // remove hyphens
            .Replace(" ", "-", StringComparison.OrdinalIgnoreCase); // replace spaces with hyphens

        // get the next series number from sequnce
        var nextSeriesNumber = await _context.GetNextSeriesNumber();
        return $"{trimmedTitle}-{nextSeriesNumber}";
    }

    private async void AddSeriesAuditLog(Series Series, string description)
    {
        var newLog = new SeriesAuditLog
        {
            Series = Series,
            Description = description,
            CreatedById = _userSession.UserId
        };
        await _context.AddAsync(newLog);
    }

    #endregion
}