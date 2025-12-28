using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NewsManagementService.Infrastructure;
using NewsManagementService.Infrastructure.RabbitMQ.Configuration;
using NewsManagementService.Infrastructure.RabbitMQ.Consumers;
using NewsManagementService.Infrastructure.Repositories;
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

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsCategoriesRepository, NewsCategoriesRepository>();
builder.Services.AddScoped<IMacroNewsCategoriesRepository, MacroNewsCategoriesRepository>();
builder.Services.AddScoped<IUserPreferencesReplicaRepository, UserPreferencesReplicaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
