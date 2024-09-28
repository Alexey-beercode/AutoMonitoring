using System.Text;
using AutoMonitoring.BLL.Factories.Implementations;
using AutoMonitoring.BLL.Factories.Interfaces;
using AutoMonitoring.BLL.Infrastructure.Mapper;
using AutoMonitoring.BLL.Infrastructure.Validators;
using AutoMonitoring.BLL.Services.Implementations;
using AutoMonitoring.BLL.Services.Interfaces;
using AutoMonitoring.DAL.Infrastructure;
using AutoMonitoring.DAL.Infrastructure.Database;
using AutoMonitoring.DAL.Repositories.Implementations;
using AutoMonitoring.DAL.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AutoMonitoring.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddSwaggerDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description = @"Enter JWT Token please.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }
            );
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new List<string>()
                    }
                }
            );
        });
    }
    
    public static void AddMapping(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(
            typeof(UserProfile).Assembly,
            typeof(RoleProfile).Assembly
        );
    }

    public static void AddAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");

        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };
        });
        builder.Services.AddAuthorization(options => options.DefaultPolicy =
            new AuthorizationPolicyBuilder
                    (JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminArea", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Admin");
            });
        });
    }
    
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
        
        builder.Services.AddScoped<IUserRepository,UserRepository>();
        builder.Services.AddScoped<IUserSessionRepository,UserSessionRepository>();
        builder.Services.AddScoped<IRoleRepository,RoleRepository>();

        builder.Services.AddScoped<IUserSessionFactory, UserSessionFactory>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IUserSessionService, UserSessionService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddControllers(); 
        
    }

    public static void AddValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<BlockUserDTOValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<RefreshTokenDTOValidator>();
        builder
            .Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
      
    }
}