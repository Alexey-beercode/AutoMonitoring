using AutoMonitoring.Extensions;
using Microsoft.AspNetCore.Builder;

namespace AutoMonitoring.Tests.IntegrationTests.Config;
using AutoMonitoring.Extensions;
public class TestStartup
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddDatabase();
        builder.AddAuthentication();
        builder.AddMapping();
        builder.AddSwaggerDocumentation();
        builder.AddValidation();
        builder.AddServices();

        var app = builder.Build();
        app.AddApplicationMiddleware();
        app.AddSwagger();
        app.MapGet("/", () => "Hello World!");

        app.Run();
    }

}