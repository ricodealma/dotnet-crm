using Dotnet.Crm.App.Extensions;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Crm - Receivable",
        Description = "Microservice responsible for generating receivable proposals",
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Crm - Receivable");
    config.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

await app.FillEnvironmentVariables(app.Configuration);
app.AddEndpoints();
app.Run();