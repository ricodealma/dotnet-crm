using Amazon.SimpleNotificationService;
using Dotnet.Crm.Domain.Aggregates.Aws;
using Dotnet.Crm.Domain.Aggregates.Crm;
using Dotnet.Crm.Infra.Data.Crm.Entities.DAOs;
using Dotnet.Crm.Infra.Data.Crm;
using Dotnet.Crm.Infra.External;
using Dotnet.Crm.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Amazon.SecretsManager;

namespace Dotnet.Crm.Infra.Extensions
{
    public static class InfraServicesExtensions
    {
        private static void AddDAOs(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IProposalDAO, ProposalDAO>();
            serviceCollection.AddScoped<IAwsDAO, AwsDAO>();
        }

        private static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IProposalRepository, ProposalRepository>();
            serviceCollection.AddScoped<IAwsRepository, AwsRepository>();
        }

        private static void AddPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDistributedMemoryCacheDAO, DistributedMemoryCacheDAO>();
            serviceCollection.AddDbContext<ICrmContext, CrmContext>();
        }

        private static void AddNotification(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAWSService<IAmazonSimpleNotificationService>();
            serviceCollection.AddAWSService<IAmazonSecretsManager>();
        }

        public static void AddInfra(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDAOs();
            serviceCollection.AddRepositories();
            serviceCollection.AddPersistence();
            serviceCollection.AddNotification();
        }
    }
}
