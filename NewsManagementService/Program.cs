using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NewsManagementService.Application;
using NewsManagementService.Infrastructure;
using NewsManagementService.Infrastructure.RabbitMQ.Configuration;
using NewsManagementService.Infrastructure.RabbitMQ.Consumers;
using NewsManagementService.Infrastructure.Repositories;
using NewsManagementService.Infrastructure.Seeders;
using NewsManagementService.Interfaces.RabbitMQ;
using NewsManagementService.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Services Configuration
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ Configuration
builder.Services.Configure<RabbitMqConfiguration>(
    builder.Configuration.GetSection("RabbitMQConfiguration"));

builder.Services.AddSingleton<IQueueConnection, RabbitMqConnection>();
builder.Services.AddHostedService<N8NEventsConsumer>();
builder.Services.AddHostedService<UserPreferencesConsumer>();

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsCategoriesRepository, NewsCategoriesRepository>();
builder.Services.AddScoped<IMacroNewsCategoriesRepository, MacroNewsCategoriesRepository>();
builder.Services.AddScoped<IUserPreferencesReplicaRepository, UserPreferencesReplicaRepository>();

// Main Application Service
builder.Services.AddScoped<NewsAppService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        
        DbInitializer.SeedAsync(dbContext).Wait();
        logger.LogInformation("Data seeding was successful.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during data migration or seeding.");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();