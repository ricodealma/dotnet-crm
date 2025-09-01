using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Requests;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;
using Dotnet.Crm.Domain.SeedWork.ErrorResult;

namespace Dotnet.Crm.Domain.Aggregates.Crm
{
    public interface ICrmService
    {
        Task<Tuple<ProposalResponse?, ErrorResult>> InsertProposalAsync(ProposalRequest proposal);
        Task<Tuple<ProposalResponse?, ErrorResult>> PostProposalSigned(Guid id);
        Task<Tuple<ProposalResponse?, ErrorResult>> SelectProposalByIdAsync(Guid id);
        Task<Tuple<ProposalResponse?, ErrorResult>> SendProposalToSign(Guid id);
    }
}
