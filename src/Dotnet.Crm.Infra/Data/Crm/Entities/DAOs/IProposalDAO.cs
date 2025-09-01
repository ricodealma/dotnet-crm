using Dotnet.Crm.Domain.SeedWork.EnumExtensions;
using Dotnet.Crm.Domain.SeedWork.ErrorResult;
using Dotnet.Crm.Infra.Data.Crm.Entities.DTOs;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DAOs
{
    public interface IProposalDAO
    {
        Task<Tuple<ProposalDTO?, ErrorResult>> InsertAsync(ProposalDTO proposalDTO);
        Task<Tuple<ProposalDTO?, ErrorResult>> SelectByIdAsync(Guid id);
        Task<Tuple<ProposalDTO?, ErrorResult>> PatchProposalStatusAsync(Guid proposalId, ProposalStatusEnum status);

    }

}
