using Dotnet.Crm.Domain.Aggregates.Aws;
using Dotnet.Crm.Domain.SeedWork;
using Newtonsoft.Json;

namespace Dotnet.Crm.App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static void FillEnvironmentKeys(EnvironmentKey environmentKey, IConfiguration configuration)
        {
            environmentKey.AwsInformation.SecretManagerInformation.SecretName =
                EnvironmentKey.GetVariable<string>(Constant.AWS_SECRET_MANAGER_NAME, configuration);

            environmentKey.AwsInformation.SecretManagerInformation.Region =
                 EnvironmentKey.GetVariable<string>(Constant.AWS_SECRET_MANAGER_REGION, configuration);

            environmentKey.RedisInformation.CacheExpirationHours =
                EnvironmentKey.GetVariable<int>(Constant.REDIS_CACHE_ENTITY_EXPIRATION_HOURS, configuration);
        }

        private static async Task FillSecretManagerInformation(EnvironmentKey environmentKey, IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            Dictionary<string, string> secrets = [];

            if (EnvironmentKey.TypeInformation != EnvironmentKey.Type.DEV)
            {
                using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();
                IAwsService awsService = scope.ServiceProvider.GetRequiredService<IAwsService>();

                secrets = await awsService.SelectSecretAsync(environmentKey.AwsInformation.SecretManagerInformation.SecretName,
                    environmentKey.AwsInformation.SecretManagerInformation.Region);
            }

            environmentKey.AppInformation.HeaderKey = EnvironmentKey.GetVariable<string>
                (Constant.AWS_SECRET_MANAGER_HEADER_TOKEN, configuration, secrets);

            environmentKey.MySqlInformation.Server = EnvironmentKey.GetVariable<string>
                (Constant.AWS_SECRET_MANAGER_SQL_SERVER, configuration, secrets);

            environmentKey.MySqlInformation.DataBase = EnvironmentKey.GetVariable<string>
                (Constant.AWS_SECRET_MANAGER_SQL_DATABASE, configuration, secrets);

            environmentKey.MySqlInformation.UserId = EnvironmentKey.GetVariable<string>
                (Constant.AWS_SECRET_MANAGER_SQL_USER, configuration, secrets);

            environmentKey.MySqlInformation.Password = EnvironmentKey.GetVariable<string>
                (Constant.AWS_SECRET_MANAGER_SQL_PASSWORD, configuration, secrets);

            environmentKey.AwsInformation.SNSInformation.NotificationWebhookTopicArn = EnvironmentKey.GetVariable<string>
                (Constant.AWS_SNS_NOTIFICATION_WEBHOOK_TOPIC_ARN, configuration, secrets);

            environmentKey.AwsInformation.SNSInformation.NotificationTopicArn = EnvironmentKey.GetVariable<string>
                (Constant.AWS_SNS_NOTIFICATION_TOPIC_ARN, configuration, secrets);

            environmentKey.RedisInformation.Password =
                EnvironmentKey.GetVariable<string>(Constant.AWS_SECRET_MANAGER_REDIS_PASSWORD, configuration, secrets);

            environmentKey.RedisInformation.User =
                EnvironmentKey.GetVariable<string>(Constant.AWS_SECRET_MANAGER_REDIS_USER, configuration, secrets);

            environmentKey.RedisInformation.Server =
                EnvironmentKey.GetVariable<string>(Constant.AWS_SECRET_MANAGER_REDIS_SERVER, configuration, secrets);

        }

        private static void AddMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GatewayAuthenticationMiddleware>();
        }
        public static async Task FillEnvironmentVariables(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            EnvironmentKey environmentKey = applicationBuilder.ApplicationServices.GetRequiredService<EnvironmentKey>();
            FillEnvironmentKeys(environmentKey, configuration);
            await FillSecretManagerInformation(environmentKey, applicationBuilder, configuration);
            ValidateConfigurationBeforeStart(environmentKey, applicationBuilder.ApplicationServices);
            applicationBuilder.AddMiddlewares();

        }

        private static void ValidateConfigurationBeforeStart(EnvironmentKey environmentKey, IServiceProvider serviceProvider)
        {
            if (!environmentKey.IsValid())
                throw new Exception(JsonConvert.SerializeObject(new { ErrorMessage = "Some environment variables are not configured", DetailedError = environmentKey }));
        }

    }
}
