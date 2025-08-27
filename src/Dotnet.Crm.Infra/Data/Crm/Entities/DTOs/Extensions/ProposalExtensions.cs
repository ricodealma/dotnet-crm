using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DTOs.Extensions
{
    public static class ProposalExtensions
    {
        public static ProposalDTO FromDomain(this ProposalModel proposal)
        {
            return new()
            {
                Id = proposal.Id,
                StatusId = proposal.StatusId,
                Client = proposal.Client.ToDto(),
                Items = proposal.Items.ToDto(),
            };
        }

        public static ProposalModel ToDomain(this ProposalDTO dto)
        {
            return new ProposalModel
            {
                Id = dto.Id,
                StatusId = dto.StatusId,
                ClientId = dto.ClientId,
                Status = dto.Status?.ToDomain(),
                Items = dto.Items?.ToDomain() ?? [],
                Client = dto.Client?.ToDomain() ?? new(),
            };
        }

        public static List<ProposalModel> ToDomain(this List<ProposalDTO> proposalDTOs) => proposalDTOs.Select(ToDomain).ToList();
    }
}
