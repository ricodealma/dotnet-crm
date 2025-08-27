namespace Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;
public sealed class ItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
}
