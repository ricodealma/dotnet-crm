using System.Text.Json.Serialization;

namespace Dotnet.Crm.Domain.Aggregates.Crm.Entities.Requests;

public sealed class ProposalRequest
{
    [JsonPropertyName("items")]
    public List<ItemRequest> Items { get; set; } = [];

    [JsonPropertyName("client")]
    public ClientRequest Client { get; set; } = new();
}
