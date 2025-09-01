using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Infra.External
{
    public interface IAwsDAO
    {
        Task<Dictionary<string, string>> GetSecretsFromSecretManagerAsync(string? secret = null, string? region = null);
        Task PublishProposalSentToSignNotificationAsync(ProposalModel proposal);
        Task PublishStatusUpdatedNotificationAsync(ProposalModel proposal);
    }
}
