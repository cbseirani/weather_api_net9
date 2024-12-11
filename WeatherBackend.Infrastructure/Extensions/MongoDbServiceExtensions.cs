using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;

namespace WeatherBackend.Infrastructure.Extensions;

public static class MongoDbServiceExtensions
{
    public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration["MONGODB_CONN_STRING"];
        var dbName = configuration["MONGODB_DB_NAME"];
        
        if(string.IsNullOrWhiteSpace(connString) || string.IsNullOrWhiteSpace(dbName))
            Log.Logger.Warning($"MongoDB connString {connString} or dbName {dbName} missing!");
            
        // register MongoDB client
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connString));

        // register MongoDB database
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(dbName);
        });
    }
}