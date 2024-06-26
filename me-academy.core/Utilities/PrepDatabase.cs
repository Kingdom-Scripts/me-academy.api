﻿using me_academy.core.Models.App;
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
                new Role { Name = nameof(Roles.SuperAdmin) },
                new Role { Name = nameof(Roles.Admin) },
                new Role { Name = nameof(Roles.ManageCourse) },
                new Role { Name = nameof(Roles.ManageUser) },
                new Role { Name = nameof(Roles.Customer) }
            );
        }

        // create default sme hub types
        if (!context.SmeHubTypes.Any())
        {
            Log.Information("--> Seeding Sme Hub Type Data...");

            context.SmeHubTypes.AddRange(
                new SmeHubType { Name = "Resolutions" },
                new SmeHubType { Name = "Employee and HR Documents" },
                new SmeHubType { Name = "Agreements" },
                new SmeHubType { Name = "Forms and Notices" },
                new SmeHubType { Name = "Power of Attorney" }
                                                                                       );
        }

        context.SaveChanges();
    }
}