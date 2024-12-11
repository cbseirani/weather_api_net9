using WeatherBackend.gRPCService.Services;
using WeatherBackend.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// set up configs from env vars
var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables(); 
var configuration = configBuilder.Build();

// register Serilog
builder.Services.AddSerilogLogging(configuration);

// configure Redis caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "WeatherBackend_";
});

// register MongoDB
builder.Services.AddMongoDb(configuration);

// add services to the container.
builder.Services.AddHostedService<WeatherFetchService>();
builder.Services.AddGrpc();

var app = builder.Build();

// configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();