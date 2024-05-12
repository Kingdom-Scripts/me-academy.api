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
                new Role { Name = nameof(Roles.SuperAdmin) },
                new Role { Name = nameof(Roles.Admin) },
                new Role { Name = nameof(Roles.ManageCourse) },
                new Role { Name = nameof(Roles.ManageUser) },
                new Role { Name = nameof(Roles.Customer) }
            );

            // TODO: remove this
            context.AddRange(
                new UserRole { RoleId = 1, UserId = 6 },
                new UserRole { RoleId = 2, UserId = 6 },
                new UserRole { RoleId = 5, UserId = 6 },
                new UserRole { RoleId = 1, UserId = 1 },
                new UserRole { RoleId = 2, UserId = 1 },
                new UserRole { RoleId = 3, UserId = 1 },
                new UserRole { RoleId = 4, UserId = 1 },
                new UserRole { RoleId = 5, UserId = 1 },

                new UserRole { RoleId = 1, UserId = 2 },
                new UserRole { RoleId = 2, UserId = 2 },
                new UserRole { RoleId = 3, UserId = 2 },
                new UserRole { RoleId = 4, UserId = 2 },
                new UserRole { RoleId = 5, UserId = 2 }
                );
        }

        context.SaveChanges();
    }
}