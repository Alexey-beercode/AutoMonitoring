using AutoMonitoring.Extensions;

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

app.Run();