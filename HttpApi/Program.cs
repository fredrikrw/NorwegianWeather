using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Interfaces.Services;
using BusinessLogic.Services;
using HttpApi.Interfaces.Validators;
using HttpApi.Validators;
using Infrastructure.Models.Options;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IWeatherReportControllerParameterValidator, WeatherReportControllerParameterValidator>();
builder.Services.AddScoped<IWeatherReportService, WeatherReportService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IDailyWeatherReportRepository, DailyWeatherReportRepository>();

builder.Services.Configure<SqlClientOptions>(builder.Configuration.GetSection("SqlClientOptions"));

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
