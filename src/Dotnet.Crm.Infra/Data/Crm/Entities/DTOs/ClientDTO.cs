using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs
{
    [Table("Client", Schema = "Crm")]
    public record ClientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
