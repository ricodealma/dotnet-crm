using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;
using Dotnet.Crm.Domain.SeedWork;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Infra.External
{
    public class AwsDAO(ILogger<AwsDAO> logger, IAmazonSimpleNotificationService snsClient, IAmazonSecretsManager secretsManager, EnvironmentKey environmentKey) : IAwsDAO
    {
        private readonly ILogger<AwsDAO> _logger = logger;
        private readonly EnvironmentKey _environmentKey = environmentKey;
        private readonly IAmazonSimpleNotificationService _snsClient = snsClient;
        private readonly IAmazonSecretsManager _secretsManager = secretsManager;

        public async Task<Dictionary<string, string>> GetSecretsFromSecretManagerAsync(string? secret = null, string? region = null)
        {
            try
            {
                var request = new GetSecretValueRequest
                {
                    SecretId = secret ?? _environmentKey.AwsInformation.SecretManagerInformation.SecretName
                };

                var response = await _secretsManager.GetSecretValueAsync(request);

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.SecretString) ?? [];
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return [];
            }
        }

        public async Task PublishProposalSentToSignNotificationAsync(ProposalModel proposal)
        {
            if (EnvironmentKey.TypeInformation == EnvironmentKey.Type.DEV)
                return;
            try
            {
                var message = JsonConvert.SerializeObject(new
                {
                    EventType = "ProposalSentToSign",
                    Data = proposal,
                    Timestamp = DateTime.UtcNow
                });

                var notificationRequest = new PublishRequest()
                {
                    TopicArn = _environmentKey.AwsInformation.SNSInformation.NotificationTopicArn,
                    Message = message,
                    MessageGroupId = proposal.Id.ToString(),
                };

                await _snsClient.PublishAsync(notificationRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
            }
        }

        public async Task PublishProposalSentToSignWebhookAsync(ProposalModel proposal)
        {
            if (EnvironmentKey.TypeInformation == EnvironmentKey.Type.DEV)
                return;
            try
            {
                var message = JsonConvert.SerializeObject(new
                {
                    EventType = "ProposalSentToSign",
                    Data = proposal,
                    Timestamp = DateTime.UtcNow
                });

                var webhookRequest = new PublishRequest
                {
                    TopicArn = _environmentKey.AwsInformation.SNSInformation.NotificationWebhookTopicArn,
                    Message = message,
                    MessageGroupId = proposal.Id.ToString(),
                };

                await _snsClient.PublishAsync(webhookRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
            }
        }

        public async Task PublishStatusUpdatedNotificationAsync(ProposalModel proposal)
        {
            if (EnvironmentKey.TypeInformation == EnvironmentKey.Type.DEV)
                return;
            try
            {
                var message = JsonConvert.SerializeObject(new
                {
                    EventType = "StatusUpdate",
                    Data = proposal,
                    Timestamp = DateTime.UtcNow
                });

                var notificationRequest = new PublishRequest()
                {
                    TopicArn = _environmentKey.AwsInformation.SNSInformation.NotificationTopicArn,
                    Message = message,
                    MessageGroupId = proposal.Id.ToString()
                };

                await _snsClient.PublishAsync(notificationRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
            }
        }

        public async Task PublishStatusUpdatedWebhookAsync(ProposalModel proposal)
        {
            if (EnvironmentKey.TypeInformation == EnvironmentKey.Type.DEV)
                return;
            try
            {
                var message = JsonConvert.SerializeObject(new
                {
                    EventType = "StatusUpdate",
                    Data = proposal,
                    Timestamp = DateTime.UtcNow
                });

                var webhookRequest = new PublishRequest
                {
                    TopicArn = _environmentKey.AwsInformation.SNSInformation.NotificationWebhookTopicArn,
                    Message = message,
                    MessageGroupId = proposal.Id.ToString(),
                };

                await _snsClient.PublishAsync(webhookRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
            }
        }
    }
}
