using Grpc.Net.Client;
using WeatherBackend.Infrastructure.Extensions;
using WeatherBackend.Infrastructure.Middleware;
using WeatherBackend.Services;
using WeatherBackend.Services.Implementations;
using WeatherBackend.WeatherGrpc;

var builder = WebApplication.CreateBuilder(args);

// set up configs from env vars
var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables(); 
var configuration = configBuilder.Build();

// register Serilog
builder.Services.AddSerilogLogging(configuration);

// register MongoDB
builder.Services.AddMongoDb(configuration);

// register other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// register gRPC client
builder.Services.AddSingleton(sp =>
{
    var grpcChannel = GrpcChannel.ForAddress("https://localhost:5001");
    return new Greeter.GreeterClient(grpcChannel);
});

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IWeatherService, WeatherService>();

var app = builder.Build();

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Weather REST API");
    });
}

// register the custom middleware
app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();
app.MapOpenApi();
app.Run();