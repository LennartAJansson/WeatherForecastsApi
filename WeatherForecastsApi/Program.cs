using MediatR;

using System.Reflection;

using WeatherForecastsApi.Controllers;
using WeatherForecastsApi.Extensions;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWeatherForecastsDb(builder.Configuration.GetConnectionString("WeatherForecastsDb"));

builder.Services.AddMediatR(Assembly.GetAssembly(typeof(WeatherForecastController))
    ?? throw new ArgumentNullException("Couldn't find assembly"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string myCors = "MyCors";
string[] origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>();

builder.Services.AddCors(options => options.AddPolicy(name: myCors,
                            conf => conf.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));

WebApplication? app = builder.Build();
app.UpdateDatabase(@".\seed.json");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(myCors);

app.MapControllers();

app.Run();
