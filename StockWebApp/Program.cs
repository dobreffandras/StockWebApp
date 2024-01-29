using StockWebApp;
using StockWebApp.Model;
using StockWebApp.Services;
using System;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

// Dependency Injection
builder.Services.AddSingleton(_ => ReadDataFromConfigurationFile());
builder.Services.AddSingleton<StocksService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(p => p.AllowAnyOrigin());
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();

app.Run();
return;

Data ReadDataFromConfigurationFile()
{
    using StreamReader r = new("data.json");
    var json = r.ReadToEnd();
    var parsed = JsonSerializer.Deserialize<Data>(
        json, 
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    return parsed ?? new Data(Enumerable.Empty<Stock>());
}