using Microsoft.EntityFrameworkCore;
using Serilog;
using Repository.Data;
using App.Mappings;
using App.Middlewares;
using MassTransit;
using App.DTOs.Consumers;
using GreenPipes;
using App.DTOs.Categories;



var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Register services
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddHealthChecks();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UpdateCategoryConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

        // Only sender for MassTransit (RabbitMQ)
        cfg.Message<UpdateCategoryMessage>(e =>
        {
            e.SetEntityName("update-category-queue");
        });

    });

    x.AddRequestClient<UpdateCategoryMessage>();
});
// MassTransit Service Register
builder.Services.AddMassTransitHostedService();

// Add core services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
