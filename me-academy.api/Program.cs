using me_academy.core.Extensions;
using me_academy.core.Middlewares;
using me_academy.core.Models.Configurations;
using me_academy.core.Utilities;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Application Starting up...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // set up serilog.
    builder.Host.UseSerilog((hostContext, services, config) =>
    {
        config.ReadFrom.Configuration(hostContext.Configuration);
        config.ReadFrom.Services(services);
        config.Enrich.FromLogContext();
        config.WriteTo.Console();
    });

    // Add services to the container.
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(swagger =>
    {
        //This is to generate the Default UI of Swagger Documentation
        swagger.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "ME Academy API",
            Description = "ME Academy Web API. An e-learning platform for lawyers and legal practitioners.",
            Contact = new OpenApiContact
            {
                Name = "Kingdom Scripts Tech Solutions",
                Email = "info@kingdomscripts.com"
            }
        });

        string xmlFilePath = Path.Combine(AppContext.BaseDirectory, "me-academy.api.xml");
        swagger.IncludeXmlComments(xmlFilePath, true);

        // include the XML of me-academy.core
        string coreXmlFilePath = Path.Combine(AppContext.BaseDirectory, "me-academy.core.xml");
        swagger.IncludeXmlComments(coreXmlFilePath, true);

        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
        });

        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        swagger.IgnoreObsoleteActions();
        swagger.IgnoreObsoleteProperties();
    });

    builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
    builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
    builder.Services.Configure<PasystackConfig>(builder.Configuration.GetSection("Paystack"));

    // Set up CORS
    string allowedOrigins = "_meAllowedDomains";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: allowedOrigins,
            policy =>
            {
                string[] hosts = builder.Configuration.GetSection("AppConfig:AllowedHosts").Get<string[]>()!;
                policy.WithOrigins(hosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    builder.Services.ConfigureServices(builder.Configuration, builder.Environment.IsProduction());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseCors(allowedOrigins);

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<JWTMiddleware>();
    app.UseMiddleware<UserSessionMiddleware>();

    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();

    await InitializeApiVideoToken.InitializeToken(app);
    PrepDatabase.PrepPopulation(app, app.Environment.IsProduction());

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException" &&
                           ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}