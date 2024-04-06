using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Azure.Identity;
using FluentValidation;
using Mapster;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Courses;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Courses;
using me_academy.core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace me_academy.core.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, bool isProduction)
    {
        // set up database
        string connectionString = configuration.GetConnectionString("MeAcademy");
        services.AddDbContext<MeAcademyContext>(
            (sp, options) => options
                .UseSqlServer(connectionString, b => b.MigrationsAssembly("me-academy.api"))
                .AddInterceptors(
                    sp.GetRequiredService<SoftDeleteInterceptor>())
                .LogTo(Console.WriteLine, LogLevel.Information));

        // Add fluent validation.
        services.AddValidatorsFromAssembly(Assembly.Load("me-academy.core"));
        services.AddFluentValidationAutoValidation(configuration =>
        {
            // Disable the built-in .NET model (data annotations) validation.
            configuration.DisableBuiltInModelValidation = true;

            // Enable validation for parameters bound from `BindingSource.Form` binding sources.
            configuration.EnableFormBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Path` binding sources.
            configuration.EnablePathBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
            configuration.EnableCustomBindingSourceAutomaticValidation = true;

            // Replace the default result factory with a custom implementation.
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });

        services.AddHttpContextAccessor();

        services.AddLazyCache();

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtConfig:Issuer"],
                ValidAudience = configuration["JwtConfig:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Secret"])),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        //Mapster global Setting. This can also be overwritten per transform
        TypeAdapterConfig.GlobalSettings.Default
                        .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
                        .IgnoreNullValues(true)
                        .AddDestinationTransform((string x) => x.Trim())
                        .AddDestinationTransform((string x) => x ?? "")
                        .AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);

        // map courses models
        TypeAdapterConfig<CourseModel, Course>
            .NewConfig()
            .Map(dest => dest.Tags, src => string.Join(",", src.Tags));

        TypeAdapterConfig<Course, CourseDetailView>
            .NewConfig()
            .Map(dest => dest.Tags, src => src.Tags.Split(",", System.StringSplitOptions.None).ToList())
            .AfterMapping((src, dest) =>
            {
                // Extract DocumentView objects from CourseDocument and add them to CourseDetailView
                dest.Resources = src.Resources.Select(cd => new DocumentView
                {
                    Id = cd.DocumentId,
                    Name = cd.Document!.Name,
                    Type = cd.Document.Type,
                    Url = cd.Document.Url,
                    ThumbnailUrl = cd.Document.ThumbnailUrl
                }).ToList();
            });

        services.AddSingleton<ICacheService, CacheService>();

        services.TryAddScoped<SoftDeleteInterceptor>();
        services.TryAddScoped<UserSession>();
        services.TryAddScoped<ITokenGenerator, TokenGenerator>();
        services.TryAddScoped<IFileService, FileService>();
        services.TryAddScoped<IEmailService, EmailService>();

        services.TryAddTransient<IAuthService, AuthService>();
        services.TryAddTransient<ICourseService, CourseService>();

        return services;
    }
}