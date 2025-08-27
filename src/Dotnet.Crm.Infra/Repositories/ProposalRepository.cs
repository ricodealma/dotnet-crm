using Dotnet.Crm.Domain.Aggregates.Crm;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Database;
using Dotnet.Crm.Domain.SeedWork;
using Dotnet.Crm.Domain.SeedWork.EnumExtensions;
using Dotnet.Crm.Domain.SeedWork.ErrorResult;
using Dotnet.Crm.Infra.Data.Crm.Entities.DAOs;
using Dotnet.Crm.Infra.Data.Crm.Entities.DTOs.Extensions;
using Dotnet.Crm.Infra.External;
using Newtonsoft.Json;

namespace Dotnet.Crm.Infra.Repositories
{
    public sealed class ProposalRepository(
        IProposalDAO proposalDAO,
        EnvironmentKey environmentKey,
        IDistributedMemoryCacheDAO distributedMemoryCacheDAO
        ) : IProposalRepository

    {
        private readonly IProposalDAO _proposalDAO = proposalDAO;
        private readonly EnvironmentKey _environmentKey = environmentKey;
        private readonly IDistributedMemoryCacheDAO _distributedMemoryCacheDAO = distributedMemoryCacheDAO;

        public async Task<Tuple<ProposalModel?, ErrorResult>> InsertProposalAsync(ProposalModel proposal)
        {
            var (result, error) = await _proposalDAO.InsertAsync(proposal.FromDomain());

            if (result is null)
                return new(null, error);

            _distributedMemoryCacheDAO.RemoveAllGroups();
            return new(result.ToDomain(), new());
        }


        public async Task<Tuple<ProposalModel?, ErrorResult>> SelectProposalByIdAsync(Guid id)
        {
            var isInCache = _distributedMemoryCacheDAO.TryGetValue<ProposalModel>(id.ToString(), out var cachedProposals);

            if (isInCache && cachedProposals is not null)
                return new(cachedProposals, new());

            var (proposalDTO, error) = await _proposalDAO.SelectByIdAsync(id);

            if (proposalDTO is null)
                return new(null, error);


            _distributedMemoryCacheDAO.SetValue(
                $"{proposalDTO.Id}",
                JsonConvert.SerializeObject(proposalDTO),
                                TimeSpan.FromMinutes(_environmentKey.RedisInformation.CacheExpirationTime));

            return new(proposalDTO.ToDomain(), new());
        }

        public async Task<Tuple<ProposalModel?, ErrorResult>> UpdateProposalStatusAsync(Guid id, ProposalStatusEnum request)
        {
            var (updatedProposal, updateError) = await _proposalDAO.PatchProposalStatusAsync(id, request);

            if (updatedProposal is null)
                return new(null, updateError);

            return new(updatedProposal.ToDomain(), new());
        }
    }
}
