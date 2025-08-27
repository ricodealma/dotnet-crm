namespace Dotnet.Crm.Domain.SeedWork;
public sealed class Constant
{
    public const string APP_ENV = "APP_ENV";
    public const string APP_ENV_DEV = "DEV";
    public const string APP_ENV_QA = "QA";
    public const string APP_ENV_PROD = "PROD";
    public const string APP_GATEWAY_REQUEST_HEADER_KEY = "Gateway-Authentication";
    public const string APP_STATUS_TO_NOTIFICATE = "APP_STATUS_TO_NOTIFICATE";
    public const string AWS_SECRET_MANAGER_GATEWAY_TOKEN = "gateway-token";

    public const string APP_FILTER_SORT_CRITERIA_ASC = "asc";
    public const string APP_FILTER_SORT_CRITERIA_DESC = "desc";
    public const string APP_FILTER_SORT_DATE = "date";

    public const string APP_REDIS_CACHE_ENTITY_BASE_NAME = "crm-receivable-microservice-api:";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_BASE_NAME = "crm-receivable-microservice-api:grouped-search:";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_MATCH = "MATCH";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_SCAN = "SCAN";
    public const string APP_REDIS_CACHE_GROUP_SEARCH_COUNT = "COUNT";

    public const string APP_REDIS_CACHE_GROUP_SEARCH_TOTAL_BASE_NAME = "grouped-search:total:";

    public const string AWS_SECRET_MANAGER_SQL_SERVER = "sql-server";
    public const string AWS_SECRET_MANAGER_SQL_USER = "sql-user";
    public const string AWS_SECRET_MANAGER_SQL_PASSWORD = "sql-password";
    public const string AWS_SECRET_MANAGER_SQL_DATABASE = "sql-database";
    public const string AWS_SNS_NOTIFICATION_TOPIC_ARN = "notification-topic-arn";
    public const string AWS_SNS_NOTIFICATION_WEBHOOK_TOPIC_ARN = "notification-webhook-topic-arn";

    public const string AWS_SECRET_MANAGER_NAME = "AWS_SECRET_MANAGER_NAME";
    public const string AWS_SECRET_MANAGER_REGION = "AWS_SECRET_MANAGER_REGION";

    public const string AWS_SECRET_MANAGER_REDIS_SERVER = "redis-server";
    public const string AWS_SECRET_MANAGER_REDIS_USER = "redis-user";
    public const string AWS_SECRET_MANAGER_REDIS_PASSWORD = "redis-password";

    public const string REDIS_CACHE_ENTITY_EXPIRATION_TIME = "REDIS_CACHE_ENTITY_EXPIRATION_TIME";
}
