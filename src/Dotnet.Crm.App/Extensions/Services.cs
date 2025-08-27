using Dotnet.Crm.App.Factory;
using Dotnet.Crm.Domain.Extensions;
using Dotnet.Crm.Domain.SeedWork.HTTP;
using Dotnet.Crm.Infra.Extensions;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

namespace Dotnet.Crm.App.Extensions
{
    public static class ServicesExtensions
    {
        private static void AddApp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<IRequest, Request>();
            serviceCollection.AddSingleton(ConnectionMultiplexerFactory.Create);

            serviceCollection.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            serviceCollection.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
        public static void AddCustomServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDomain();
            serviceCollection.AddInfra();
            serviceCollection.AddApp();
        }
    }
}
