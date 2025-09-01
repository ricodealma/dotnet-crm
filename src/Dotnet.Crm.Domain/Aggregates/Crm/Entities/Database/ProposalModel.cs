namespace Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database
{
    public record ProposalModel
    {
        public Guid Id { get; set; }
        public int StatusId { get; set; }
        public Guid? ClientId { get; set; }
        public StatusModel? Status { get; set; }
        public ClientModel Client { get; set; } = new();
        public List<ItemModel> Items { get; set; } = [];

    }


    public record ItemModel
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }

    public record ClientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }

    public record StatusModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
