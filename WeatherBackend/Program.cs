using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using WeatherBackend.Common.Models;
using WeatherBackend.Middleware;

var builder = WebApplication.CreateBuilder(args);

// set up configs from env vars
var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables(); 
var configuration = configBuilder.Build();

var logLevel = configuration["LOG_LEVEL"] ?? "Information"; 
var logEventLevel = Enum.Parse<LogEventLevel>(logLevel, true);
var logName = configuration["LOG_NAME"] ?? "WeatherBackend.gRPCService.log";

// create Serilog logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(logEventLevel)
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, logName), 
        rollingInterval: RollingInterval.Month,
        buffered: false)
    .WriteTo.Console()
    .CreateLogger();

// register Serilog
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});

// configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Register MongoDB database
builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// register other services
builder.Services.AddControllers();
//builder.Services.AddTransient<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// Register the custom error middleware
app.UseMiddleware<ErrorMiddleware>();
app.MapControllers();
app.Run();