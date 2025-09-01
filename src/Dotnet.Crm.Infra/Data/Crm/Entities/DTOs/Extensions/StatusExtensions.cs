using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs.Extensions
{
    public static class StatusExtensions
    {
        public static StatusModel? ToDomain(this StatusDTO? dto) => dto is null ? null : new() { Id = dto.Id, Description = dto.Description };
    }
}
