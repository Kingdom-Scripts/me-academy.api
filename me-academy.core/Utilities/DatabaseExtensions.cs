using me_academy.core.Models.App;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace me_academy.core.Utilities;

internal static class DatabaseExtensions
{
    public static async Task<int> GetNextCourseNumber(this MeAcademyContext context)
        => await GetNextNumber(context, "CourseNumber");

    public static async Task<int> GetNextSeriesNumber(this MeAcademyContext context)
        => await GetNextNumber(context, "SeriesNumber");


    private static async Task<int> GetNextNumber(MeAcademyContext context, string type)
    {
        var sqlParameter = new SqlParameter("@result", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        await context.Database
            .ExecuteSqlRawAsync($"set @result = next value for {type}", sqlParameter);
        int nextVal = (int)sqlParameter.Value;
        return nextVal;
    }
}
