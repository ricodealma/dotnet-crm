using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Requests;
using Dotnet.Crm.Domain.SeedWork.EnumExtensions;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;

namespace Dotnet.Crm.Domain.SeedWork.Mappers
{
    public static class ProposalMapper
    {
        public static ProposalModel ConvertFromRequest(ProposalRequest proposal)
        {
            return new()
            {
                Id = Guid.CreateVersion7(),
                StatusId = (int)StatusEnum.DRAFT,
                Client = new()
                {
                    Company = proposal.Client.Company,
                    Email = proposal.Client.Email,
                    Name = proposal.Client.Name,
                },
                Items = proposal.Items.Select(item => new ItemModel
                {
                    Quantity = item.Quantity,
                    Name = item.Name,
                    UnitPrice = item.UnitPrice
                }).ToList(),
            };
        }

        public static ProposalResponse ToResponse(this ProposalModel proposal)
        {
            var proposalResponse = new ProposalResponse
            {
                Id = proposal.Id,
                Client = proposal.Client.ToResponse(),

                Status = $"{(StatusEnum)proposal.StatusId}",

                Items = proposal.Items is not null ? proposal.Items.ToResponse() : [],

            };

            return proposalResponse;
        }
    }
}
