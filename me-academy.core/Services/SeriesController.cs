using me_academy.core.Models.App;

namespace me_academy.core.Services;

public class SeriesController
{
    private readonly MeAcademyContext _context;

    public SeriesController(MeAcademyContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


}