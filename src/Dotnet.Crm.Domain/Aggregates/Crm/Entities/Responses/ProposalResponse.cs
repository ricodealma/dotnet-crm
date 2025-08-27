namespace Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;

public sealed class ProposalResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<ItemResponse> Items { get; set; } = [];
    public ClientResponse Client { get; set; } = new();
}
