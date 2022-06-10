using MediatR;

using System.Reflection;

using WeatherForecastsApi.Controllers;
using WeatherForecastsApi.Extensions;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(WeatherForecastController))
    ?? throw new ArgumentNullException("Couldn't find assembly"));

builder.Services.AddWeatherForecastsDb(builder.Configuration.GetConnectionString("WeatherForecastsDb"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication? app = builder.Build();

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
