using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace me_academy.core.Utilities;

public static class PrepDatabase
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProd)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<MeAcademyContext>(), isProd);
        }
    }

    private static void SeedData(MeAcademyContext context, bool isProd)
    {
        // run migration when in prod
        if (isProd)
        {
            Log.Information("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "--> Could not run migrations.");
            }
        }

        //create default role data
        if (!context.Roles.Any())
        {
            Log.Information("--> Seeding Role Data...");

            // reset the identity count
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Roles', RESEED, 0)");

            context.Roles.AddRange(
                new Role { Name = nameof(Roles.SvpAdmin) },
                new Role { Name = nameof(Roles.SvpManager) },
                new Role { Name = nameof(Roles.BusinessAdmin) },
                new Role { Name = nameof(Roles.BusinessManager) },
                new Role { Name = nameof(Roles.BusinessClient) },
                new Role { Name = nameof(Roles.Client) },
                new Role { Name = nameof(Roles.SuperAdmin) }
                );
        }

        context.SaveChanges();
    }
}