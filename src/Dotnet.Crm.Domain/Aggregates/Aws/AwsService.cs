using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Domain.Aggregates.Aws
{
    public sealed class AwsService(IAwsRepository awsRepository) : IAwsService
    {
        private readonly IAwsRepository _awsRepository = awsRepository;

        public async Task<Dictionary<string, string>> SelectSecretAsync(string? secret = null, string? region = null) => await _awsRepository.SelectSecretAsync(secret, region);
        public async Task PublishProposalSentToSignNotificationAsync(ProposalModel proposal) => await _awsRepository.PublishProposalSentToSignNotificationAsync(proposal);
        public async Task PublishStatusUpdatedNotificationAsync(ProposalModel proposal) => await _awsRepository.PublishStatusUpdatedNotificationAsync(proposal);
    }
}
