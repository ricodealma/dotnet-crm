using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Domain.Aggregates.Aws
{
    public interface IAwsRepository
    {
        Task<Dictionary<string, string>> SelectSecretAsync(string? secret = null, string? region = null);
        Task PublishProposalSentToSignNotificationAsync(ProposalModel proposal);
        Task PublishStatusUpdatedNotificationAsync(ProposalModel proposal);
    }
}
