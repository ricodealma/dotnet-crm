using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs
{
    [Table("ProposalStatus")]
    public record StatusDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
