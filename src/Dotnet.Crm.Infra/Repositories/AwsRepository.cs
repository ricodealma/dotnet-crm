using Dotnet.Crm.Domain.Aggregates.Aws;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;
using Dotnet.Crm.Infra.External;

namespace Dotnet.Crm.Infra.Repositories
{
    public sealed class AwsRepository(IAwsDAO aws) : IAwsRepository
    {
        private readonly IAwsDAO _awsDao = aws;

        public async Task<Dictionary<string, string>> SelectSecretAsync(string? secret = null, string? region = null) => await _awsDao.GetSecretsFromSecretManagerAsync(secret, region);
        public async Task PublishProposalSentToSignNotificationAsync(ProposalModel proposal) => await _awsDao.PublishProposalSentToSignNotificationAsync(proposal);
        public async Task PublishProposalSentToSignWebhookAsync(ProposalModel proposal) => await _awsDao.PublishProposalSentToSignWebhookAsync(proposal);
        public async Task PublishStatusUpdatedNotificationAsync(ProposalModel proposal) => await _awsDao.PublishStatusUpdatedNotificationAsync(proposal);
        public async Task PublishStatusUpdatedWebhookAsync(ProposalModel proposal) => await _awsDao.PublishStatusUpdatedWebhookAsync(proposal);
    }
}

