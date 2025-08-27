using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs
{
    [Table("Proposal", Schema = "Crm")]
    public record ProposalDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public int StatusId { get; set; }
        public ClientDTO? Client { get; set; }
        public StatusDTO? Status { get; set; }
        public List<ItemDTO>? Items { get; set; }
    }
}
