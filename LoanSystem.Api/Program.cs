using FluentValidation;
using Scalar.AspNetCore;
using LoanSystem.Api.Middleware;
using LoanSystem.Application.Behaviors;
using LoanSystem.Infrastructure;
using LoanSystem.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Port binding for Railway (only override if PORT is specified in the environment)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Application Layer
var applicationAssembly = typeof(LoanSystem.Application.Abstractions.Messaging.ICommand).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(applicationAssembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(applicationAssembly);

// Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Apply database migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var resetDb = Environment.GetEnvironmentVariable("RESET_DATABASE");
        if (string.Equals(resetDb, "true", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("RESET_DATABASE=true detected. Deleting existing database tables...");
            await context.Database.EnsureDeletedAsync();
            Console.WriteLine("Database deleted successfully. Proceeding with migrations...");
        }

        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Failed to apply database migrations on startup. If this is design-time (e.g. dotnet ef), this is expected.");
    }
}

// Configure the HTTP request pipeline.
var forwardedOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedOptions.KnownNetworks.Clear();
forwardedOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedOptions);

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
