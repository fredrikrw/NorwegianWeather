using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Interfaces.Services;
using BusinessLogic.Services;
using HttpApi.Interfaces.Validators;
using HttpApi.Validators;
using Infrastructure.Models.Options;
using Infrastructure.SQL.Repositories;
using RepoDb;

var builder = WebApplication.CreateBuilder(args);

GlobalConfiguration.Setup().UseSqlServer();

// Add services to the container.
builder.Services.AddScoped<IWeatherReportControllerParameterValidator, WeatherReportControllerParameterValidator>();
builder.Services.AddScoped<IWeatherReportService, WeatherReportService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IDailyWeatherReportRepository, DailyWeatherReportRepository>();

builder.Services.Configure<SqlConnectionOptions>(builder.Configuration.GetSection("SqlConnectionOptions"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
