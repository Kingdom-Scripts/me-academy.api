using LazyCache;
using me_academy.core.Constants.CacheKeys;
using me_academy.core.Models.App;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Stats;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Services;
public class StatsService
{
    private readonly IAppCache _cache;
    private readonly MeAcademyContext _context;

    public StatsService(IAppCache cache, MeAcademyContext context)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<Result> ListTopRegistrants()
    {
        var result = await _cache.GetOrAddAsync(StatsCacheKeys.TopRegistrants(), async () =>
        {
            var topRegistrants = await _context.Orders
                .Where(o => o.IsPaid)
                .GroupBy(o => o.UserId)
                .Select(g => new TopRegistrantView
                {
                    FullName = g.First().User.FirstName + " " + g.First().User.LastName,
                    DateJoined = g.FirstOrDefault().User.CreatedAtUtc,
                    MaterialsPurchased = string.Join(", ", g.GroupBy(gx => gx.ItemType)
                        .Select(gg => $"{gg.Count()} {gg.Key}{(gg.Count() > 1 ? 's' : "")}")),
                    TotalSpent = g.Sum(o => o.TotalAmount)
                })
                .Take(10)
                .ToListAsync();

            return topRegistrants;
        }, new TimeSpan(0, 45, 0));

        return new SuccessResult(result);
    }
}
