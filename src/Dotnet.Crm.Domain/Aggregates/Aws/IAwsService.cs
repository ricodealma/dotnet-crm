using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Domain.Aggregates.Aws
{
    public interface IAwsService
    {
        Task<Dictionary<string, string>> SelectSecretAsync(string? secret = null, string? region = null);
        Task PublishProposalSentToSignNotificationAsync(ProposalModel proposal);
        Task PublishProposalSentToSignWebhookAsync(ProposalModel proposal);
        Task PublishStatusUpdatedNotificationAsync(ProposalModel proposal);
        Task PublishStatusUpdatedWebhookAsync(ProposalModel proposal);
    }
}
