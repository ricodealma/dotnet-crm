using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;

namespace Dotnet.Crm.Domain.SeedWork.Mappers
{
    public static class ItemMapper
    {
        public static ItemResponse ToResponse(this ItemModel item) => new()
        {
            Id = item.Id,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            Name = item.Name
        };

        public static List<ItemResponse> ToResponse(this List<ItemModel> items) => items.Select(ToResponse).ToList();
    }
}
