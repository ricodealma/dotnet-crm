using Dotnet.Crm.Domain.Aggregates.Aws;
using Dotnet.Crm.Domain.SeedWork.ErrorResult;
using Dotnet.Crm.Domain.SeedWork;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Requests;
using Dotnet.Crm.Domain.SeedWork.Mappers;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;
using Dotnet.Crm.Domain.SeedWork.EnumExtensions;

namespace Dotnet.Crm.Domain.Aggregates.Crm
{
    public sealed class CrmService(
        IProposalRepository proposalRepository,
        EnvironmentKey environmentKey,
        IAwsService awsService) : ICrmService
    {
        private readonly IProposalRepository _proposalRepository = proposalRepository;
        private readonly EnvironmentKey _environmentKey = environmentKey;
        private readonly IAwsService _awsService = awsService;

        public async Task<Tuple<ProposalResponse?, ErrorResult>> InsertProposalAsync(ProposalRequest request)
        {
            var proposal = ProposalMapper.ConvertFromRequest(request);

            var (insertedProposal, error) = await _proposalRepository.InsertProposalAsync(proposal);

            if (insertedProposal == null)
                return new(null, error);

            var proposalResponse = insertedProposal.ToResponse();

            return new(proposalResponse, error);
        }
        public async Task<Tuple<ProposalResponse?, ErrorResult>> SelectProposalByIdAsync(Guid id)
        {
            var (proposal, proposalError) = await _proposalRepository.SelectProposalByIdAsync(id);

            if (proposal is null)
                return new(null, proposalError);

            return new(proposal.ToResponse(), proposalError);
        }

        public async Task<Tuple<ProposalResponse?, ErrorResult>> SendProposalToSign(Guid id)
        {
            var (proposal, proposalError) = await _proposalRepository.SelectProposalByIdAsync(id);

            if (proposal is null)
                return new(null, proposalError);

            await Task.WhenAll(
                _awsService.PublishProposalSentToSignNotificationAsync(proposal),
                _awsService.PublishProposalSentToSignWebhookAsync(proposal)
            );

            return await UpdateStatusAsync(id, ProposalStatusEnum.SentForSignature);
        }
        public async Task<Tuple<ProposalResponse?, ErrorResult>> PostProposalSigned(Guid id)
        {
            var (proposal, proposalError) = await _proposalRepository.SelectProposalByIdAsync(id);

            if (proposal is null)
                return new(null, proposalError);

            return await UpdateStatusAsync(id, ProposalStatusEnum.Signed); ;
        }

        public async Task<Tuple<ProposalResponse?, ErrorResult>> UpdateStatusAsync(Guid id, ProposalStatusEnum request)
        {
            var (updateResult, updateError) = await _proposalRepository.UpdateProposalStatusAsync(id, request);
            if (updateResult is null)
                return new(null, updateError);

            var updateResponse = updateResult.ToResponse();

            return new(updateResponse, new());
        }
    }
}
