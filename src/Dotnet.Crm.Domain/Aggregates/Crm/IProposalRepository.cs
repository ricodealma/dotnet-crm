using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;
using Dotnet.Crm.Domain.SeedWork.EnumExtensions;
using Dotnet.Crm.Domain.SeedWork.ErrorResult;

namespace Dotnet.Crm.Domain.Aggregates.Crm
{
    public interface IProposalRepository
    {
        Task<Tuple<ProposalModel?, ErrorResult>> InsertProposalAsync(ProposalModel proposal);
        Task<Tuple<ProposalModel?, ErrorResult>> SelectProposalByIdAsync(Guid id);
        Task<Tuple<ProposalModel?, ErrorResult>> UpdateProposalStatusAsync(Guid id, ProposalStatusEnum request);
    }
}
