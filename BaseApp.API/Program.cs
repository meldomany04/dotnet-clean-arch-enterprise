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

app.ConfigureMiddlewarePipeline();
app.ConfigureEndpoints();

app.Run();
