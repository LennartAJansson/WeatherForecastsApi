using MediatR;

using System.Reflection;

using WeatherForecastsApi;
using WeatherForecastsApi.Controllers;
using WeatherForecastsApi.Extensions;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWeatherForecastsDb(builder.Configuration.GetConnectionString(Constants.ConnectionStringName));

builder.Services.AddMediatR(Assembly.GetAssembly(typeof(WeatherForecastController))
    ?? throw new ArgumentNullException("Couldn't find assembly"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors not included in documentation, see more in appsettings.json
string[] origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>();
string[] methods = builder.Configuration.GetSection("Cors:Methods").Get<string[]>();
string[] headers = builder.Configuration.GetSection("Cors:Headers").Get<string[]>();

builder.Services.AddCors(options => options.AddPolicy(name: Constants.CorsPolicyName,
                            conf => conf.WithOrigins(origins)
                                .WithHeaders(headers)
                                .WithMethods(methods)));

WebApplication? app = builder.Build();

app.UpdateDatabase(Constants.SeedFileName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(Constants.CorsPolicyName);

app.MapControllers();

app.Run();
