using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs.Extensions
{
    public static class ClientExtensions
    {
        public static ClientDTO ToDto(this ClientModel client)
        {
            return new()
            {
                Email = client.Email,
                Name = client.Name,
                Company = client.Company,
            };
        }

        public static ClientModel ToDomain(this ClientDTO dto)
        {
            return new ClientModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Company = dto.Company
            };
        }
    }
}
