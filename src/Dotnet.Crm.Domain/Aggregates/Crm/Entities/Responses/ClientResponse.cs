namespace Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;
public sealed class ClientResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
}
