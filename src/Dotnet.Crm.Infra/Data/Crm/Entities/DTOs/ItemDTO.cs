using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Requests;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs
{
    [Table("Item", Schema = "Crm")]
    public record ItemDTO
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }


        public static ItemDTO ConvertFromItemRequest(ItemRequest item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "ItemRequest cannot be null.");
            }
            var dtoItem = new ItemDTO();
            dtoItem.UnitPrice = item.UnitPrice;
            dtoItem.Quantity = item.Quantity;
            dtoItem.Name = item.Name;
            return dtoItem;
        }


        public static List<ItemDTO> ConvertFromItemRequest(List<ItemRequest> items)
        {
            List<ItemDTO> ItemDtos = [];
            foreach (var item in items)
            {
                var dtoItem = ConvertFromItemRequest(item);
                ItemDtos.Add(dtoItem);
            }
            return ItemDtos;
        }



        public static ItemDTO ConvertFromItem(ItemResponse itemResponse)
        {
            return new ItemDTO
            {
                Id = itemResponse.Id,
                Name = itemResponse.Name,
                Quantity = itemResponse.Quantity,
                UnitPrice = itemResponse.UnitPrice
            };
        }

        public static List<ItemDTO> ConvertFromItem(List<ItemResponse> itemResponses)
        {
            return itemResponses.Select(ConvertFromItem).ToList();
        }

    }
}
