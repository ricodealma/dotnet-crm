using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs.Extensions
{
    public static class ItemExtensions
    {
        public static ItemDTO ToDto(this ItemModel item) => new()
        {
            Quantity = item.Quantity,
            Name = item.Name,
            UnitPrice = item.UnitPrice
        };

        public static List<ItemDTO> ToDto(this List<ItemModel> items) => items.Select(ToDto).ToList();

        public static ItemModel ToDomain(this ItemDTO dto) => new()
        {
            Id = dto.Id,
            ProposalId = dto.ProposalId,
            Name = dto.Name,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
        };

        public static List<ItemModel> ToDomain(this List<ItemDTO> dtos) => dtos.Select(ToDomain).ToList();

    }
}
