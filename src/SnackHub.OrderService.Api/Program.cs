using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SnackHub.OrderService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Snack Hub Order Service", Version = "v1" });
        options.AddAuthorizationOptions();
        
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
        
    })
    .AddHttpClient();


builder
    .Services
    .AddMongoDb(builder.Configuration)
    .AddMassTransit(builder.Configuration)
    .AddRepositories()
    .AddServices()
    .AddUseCases()
    .AddValidators();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (bool.TryParse(builder.Configuration.GetSection("https").Value, out var result) && result)
    app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseMongoDbConventions();
app.MapControllers();
app.Run();