using Asp.Versioning.ApiExplorer;
using BaseApp.API.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.ConfigureBaseServices();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureLocalization();
builder.Services.ConfigureHangfire(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureApplicationServices(builder.Configuration);

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.ConfigureMiddlewarePipeline(apiVersionDescriptionProvider);
app.ConfigureEndpoints();

app.Run();
