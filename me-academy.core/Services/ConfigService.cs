using Mapster;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Config;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Services;

public class ConfigService : IConfigService
{
    private readonly MeAcademyContext _context;

    public ConfigService(MeAcademyContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Result> ListDurations()
    {
        var durations = await _context.Durations
            .ProjectToType<DurationView>()
            .ToListAsync();

        return new SuccessResult(durations);
    }
}