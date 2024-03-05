using GladLogs.Server.Auth;
using GladLogs.Server.Consts;
using GladLogs.Server.Middleware;
using GladLogs.Server.Models;
using GladLogs.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var corsName = "CorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsName, policy => 
    {
        var origins = builder.Configuration.GetValue<List<string>>("Orgins");
        if (origins is null) return;
        foreach (var origin in origins)
        {
            policy.WithOrigins(origin);

        };

    });
});

builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<LogsContext>();

builder.Services.Configure<TwitchChatOptions>(builder.Configuration.GetSection("Twitch"));
builder.Services.AddHostedService<TwitchChatService>();

var apiKey = builder.Configuration[ApiKeyConsts.ApiKey] ?? throw new InvalidOperationException("Invalid Api Key");

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseCors(corsName);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
