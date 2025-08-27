using Microsoft.Extensions.Configuration;

namespace Dotnet.Crm.Domain.SeedWork
{
    public class EnvironmentKey() : ObjectValidator
    {
        public Aws AwsInformation { get; } = new();
        public MySql MySqlInformation { get; } = new();
        public App AppInformation { get; } = new();
        public static Type TypeInformation { get; set; } = GetEnvironment();
        public Redis RedisInformation { get; } = new();
        public sealed class Aws
        {
            public SecretManager SecretManagerInformation { get; set; } = new();

            public SimpleNotificationService SNSInformation { get; set; } = new();

            public sealed class SimpleNotificationService
            {
                public string NotificationTopicArn { get; set; } = string.Empty;
                public string NotificationWebhookTopicArn { get; set; } = string.Empty;
            }

            public sealed class SecretManager
            {
                public string SecretName { get; set; } = string.Empty;
                public string Region { get; set; } = string.Empty;
            }
        }

        public sealed class MySql
        {
            public string Server { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string DataBase { get; set; } = string.Empty;

            public string ConnectionString
            {
                get
                {
                    return $"SERVER={Server};UID={UserId};PWD={Password};DATABASE={DataBase};TrustServerCertificate=True;";
                }
            }
        }

        public enum Type
        {
            DEV,
            QA,
            PROD
        }

        public sealed class App
        {
            public string GatewayToken { get; set; } = string.Empty;
            public string StatusToNotificate { get; set; } = string.Empty;
        }

        public sealed class Redis
        {
            public string Server { get; set; } = string.Empty;
            public string User { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public int CacheExpirationTime { get; set; } = default;

            public string ConnectionString
            {
                get
                {
                    return $"{Server}:6379,user={User},password={Password},ssl=True";
                }
            }
        }
        public bool IsValid() => AllPropertiesAreFilled(this);

        private static Type GetEnvironment()
        {
            string env = Environment.GetEnvironmentVariable(Constant.APP_ENV)!;
            return env switch
            {
                Constant.APP_ENV_QA => Type.QA,
                Constant.APP_ENV_PROD => Type.PROD,
                _ => Type.DEV,
            };
        }

        public static T GetVariable<T>(string? constant, IConfiguration configuration)
        {
            try
            {
                if (constant is null) throw new ArgumentNullException(constant);

                if (TypeInformation == Type.DEV)
                {
                    return (T)Convert.ChangeType(configuration[constant]! ?? string.Empty, typeof(T));
                }

                return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(constant)! ?? string.Empty, typeof(T));
            }
            catch (Exception)
            {
                throw new NotImplementedException($"Missing an environment variable: {constant}");
            }
        }

        public static T GetVariable<T>(string constant, IConfiguration configuration, Dictionary<string, string> secrets)
        {
            try
            {
                if (TypeInformation != Type.DEV)
                {
                    return (T)Convert.ChangeType(secrets.GetValueOrDefault(constant) ?? string.Empty, typeof(T));
                }
                else
                {
                    return (T)Convert.ChangeType(configuration[constant] ?? string.Empty, typeof(T));
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException($"Missing an environment variable: {constant}");
            }
        }
    }
}
