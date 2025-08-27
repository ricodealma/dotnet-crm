using Microsoft.Extensions.DependencyInjection;
using Dotnet.Crm.Domain.Aggregates.Aws;
using Dotnet.Crm.Domain.SeedWork;
using Dotnet.Crm.Domain.Aggregates.Crm;

namespace Dotnet.Crm.Domain.Extensions
{
    public static class DomainServicesExtensions
    {
        private static void AddDomainServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAwsService, AwsService>();
            serviceCollection.AddScoped<ICrmService, CrmService>();
        }

        private static void AddSeedWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<EnvironmentKey>();
        }

        public static void AddDomain(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDomainServices();
            serviceCollection.AddSeedWork();
        }
    }
}
