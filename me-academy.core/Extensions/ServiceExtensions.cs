using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Azure.Identity;
using FluentValidation;
using Mapster;
using me_academy.core.Constants;
using me_academy.core.Interfaces;
using me_academy.core.Middlewares;
using me_academy.core.Models.App;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Courses;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View;
using me_academy.core.Models.View.Courses;
using me_academy.core.Models.View.Series;
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
        string connectionString = configuration.GetConnectionString("MeAcademy")!;
        services.AddDbContext<MeAcademyContext>(
            (sp, options) => options
                .UseSqlServer(connectionString, b => b.MigrationsAssembly("me-academy.api"))
                .AddInterceptors(
                    sp.GetRequiredService<SoftDeleteInterceptor>())
                .LogTo(Console.WriteLine, LogLevel.Information));

        // Add fluent validation.
        services.AddValidatorsFromAssembly(Assembly.Load("me-academy.core"));
        services.AddFluentValidationAutoValidation(fluentConfig =>
        {
            // Disable the built-in .NET model (data annotations) validation.
            fluentConfig.DisableBuiltInModelValidation = true;

            // Enable validation for parameters bound from `BindingSource.Form` binding sources.
            fluentConfig.EnableFormBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Path` binding sources.
            fluentConfig.EnablePathBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
            fluentConfig.EnableCustomBindingSourceAutomaticValidation = true;

            // Replace the default result factory with a custom implementation.
            fluentConfig.OverrideDefaultResultFactoryWith<CustomResultFactory>();
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Secret"]!)),
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

        // Add HTTP clients
        services.AddHttpClient(HttpClientKeys.ApiVideo, async client =>
        {
            string baseAddress = configuration["AppConfig:ApiVideo:BaseUrl"]!;

            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddHttpMessageHandler<ApiVideoHttpHandler>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseDefaultCredentials = true
            });

        // Mapster global Setting. This can also be overwritten per transform
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
            .Map(dest => dest.Tags, src => src.Tags.Split(",", System.StringSplitOptions.None).ToList());

        TypeAdapterConfig<CoursePrice, PriceView>
            .NewConfig()
            .Map(dest => dest.Name, src => src.Duration!.Name);

        TypeAdapterConfig<CourseDocument, DocumentView>
            .NewConfig()
            .Map(dest => dest.Name, src => src.Document!.Name)
            .Map(dest => dest.Type, src => src.Document!.Type)
            .Map(dest => dest.Url, src => src.Document!.Url)
            .Map(dest => dest.ThumbnailUrl, src => src.Document!.ThumbnailUrl);

        services.AddSingleton<ICacheService, CacheService>();

        services.TryAddScoped<SoftDeleteInterceptor>();
        services.TryAddScoped<UserSession>();
        services.TryAddScoped<ITokenGenerator, TokenGenerator>();
        services.TryAddScoped<IFileService, FileService>();
        services.TryAddScoped<IEmailService, EmailService>();

        services.TryAddTransient<IAuthService, AuthService>();
        services.TryAddTransient<ICourseService, CourseService>();
        services.TryAddTransient<IConfigService, ConfigService>();
        services.TryAddTransient<ApiVideoHttpHandler>();
        services.TryAddTransient<IVideoService, VideoService>();
        services.TryAddTransient<IQaService, QaService>();

        return services;
    }
}