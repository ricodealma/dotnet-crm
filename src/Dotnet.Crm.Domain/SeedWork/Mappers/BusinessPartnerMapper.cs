using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;

namespace Dotnet.Crm.Domain.SeedWork.Mappers
{
    public static class ClientMapper
    {
        public static ClientResponse ToResponse(this ClientModel client)
        {
            return new ClientResponse
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Company = client.Company
            };
        }
    }
}
