using AutoMonitoring.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();