using System.Text;
using LoanSystem.Application.Abstractions.Identity;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Infrastructure.Database;
using LoanSystem.Infrastructure.Database.Repositories;
using LoanSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LoanSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Database
        services.AddDbContext<AppDbContext>(options =>
        {
            // Dump environment variable keys for troubleshooting
            Console.WriteLine("=== Railway Environment Variable Keys ===");
            foreach (System.Collections.DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                Console.WriteLine($"- {entry.Key}");
            }
            Console.WriteLine("=========================================");

            var connectionString = GetEnvVarIgnoreCase("ConnectionStrings__DefaultConnection")
                                   ?? GetEnvVarIgnoreCase("ConnectionStrings:DefaultConnection")
                                   ?? GetEnvVarIgnoreCase("DATABASE_URL")
                                   ?? GetEnvVarIgnoreCase("MYSQL_URL")
                                   ?? GetEnvVarIgnoreCase("MYSQL_PRIVATE_URL")
                                   ?? GetEnvVarIgnoreCase("MYSQLPRIVATE_URL")
                                   ?? GetEnvVarIgnoreCase("CONNECTION_STRING")
                                   ?? configuration.GetConnectionString("DefaultConnection");
                                   
            Console.WriteLine($"Resolved connection string contains localdb: {connectionString?.Contains("(localdb)") == true}");
                                   
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));
            options.UseMySql(connectionString, serverVersion);
        });

        // 2. Repositories & Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IInteractionRepository, InteractionRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ILoanProductRepository, LoanProductRepository>();

        // 3. Identity Core
        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // 4. JWT Options & Provider
        var jwtSection = configuration.GetSection(JwtOptions.SectionName);
        services.Configure<JwtOptions>(jwtSection);
        
        services.PostConfigure<JwtOptions>(options =>
        {
            if (string.IsNullOrEmpty(options.SecretKey))
            {
                options.SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
                                    ?? "super_secret_key_that_is_long_enough_for_hmac_sha256_32_chars";
            }
            if (string.IsNullOrEmpty(options.Issuer))
            {
                options.Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "LoanSystem";
            }
            if (string.IsNullOrEmpty(options.Audience))
            {
                options.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "LoanSystem";
            }
        });

        services.AddScoped<IJwtProvider, JwtProvider>();

        // 5. Authentication & JWT Configuration
        var secretKey = configuration["Jwt:SecretKey"] 
                        ?? Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
                        ?? "super_secret_key_that_is_long_enough_for_hmac_sha256_32_chars"; // Fallback for default local dev testing if not set

        var issuer = configuration["Jwt:Issuer"] 
                     ?? Environment.GetEnvironmentVariable("JWT_ISSUER") 
                     ?? "LoanSystem";

        var audience = configuration["Jwt:Audience"] 
                       ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                       ?? "LoanSystem";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        return services;
    }

    private static string? GetEnvVarIgnoreCase(string key)
    {
        var variables = Environment.GetEnvironmentVariables();
        foreach (System.Collections.DictionaryEntry entry in variables)
        {
            if (string.Equals(entry.Key?.ToString(), key, StringComparison.OrdinalIgnoreCase))
            {
                return entry.Value?.ToString();
            }
        }
        return null;
    }
}
