using Dotnet.Crm.Domain.SeedWork.EnumExtensions;
using Dotnet.Crm.Domain.SeedWork.ErrorResult;
using Dotnet.Crm.Infra.Data.Crm.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dotnet.Crm.Infra.Data.Crm.Entities.DAOs
{
    public class ProposalDAO(ILogger<ProposalDAO> logger, ICrmContext crmContext) : IProposalDAO
    {
        private readonly ILogger<ProposalDAO> _logger = logger;
        private readonly ICrmContext _crmContext = crmContext;
        public async Task<Tuple<ProposalDTO?, ErrorResult>> SelectByIdAsync(Guid id)
        {
            try
            {
                var proposalDTO = await _crmContext.Proposal
                    .Include(proposal => proposal.Status)
                    .Include(proposal => proposal.Items)
                    .Include(proposal => proposal.Client)
                    .FirstOrDefaultAsync(proposal => proposal.Id == id);

                if (proposalDTO == null)
                {
                    return new(null, new ErrorResult
                    {
                        Error = true,
                        StatusCode = ErrorCode.NotFound,
                        Id = id.ToString(),
                        Message = "Proposal not found for the given ID"
                    });
                }

                return new(proposalDTO, new());
            }
            catch (Exception e)
            {
                string error = JsonConvert.SerializeObject(e);
                _logger.LogError(error);

                return new(null, new ErrorResult
                {
                    Error = true,
                    Message = error,
                    StatusCode = ErrorCode.InternalServerError,
                    Id = id.ToString()
                });
            }
        }

        public async Task<Tuple<ProposalDTO?, ErrorResult>> InsertAsync(ProposalDTO proposal)
        {
            await using var transaction = await _crmContext.Database.BeginTransactionAsync();
            try
            {
                var result = await _crmContext.Proposal.AddAsync(proposal);
                await _crmContext.SaveChangesAsync();

                if (result.Entity.Id == default)
                {
                    await transaction.RollbackAsync();
                    return new(null, new()
                    {
                        Error = true,
                        StatusCode = ErrorCode.InternalServerError,
                        Message = $"Unexpected Error While inserting proposal: {JsonConvert.SerializeObject(proposal)}"
                    });
                }

                await transaction.CommitAsync();
                return new(result.Entity, new());

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Unexpected error: {ex.Message} - {JsonConvert.SerializeObject(proposal)}");
                return new(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"{JsonConvert.SerializeObject(proposal)}"
                });
            }
        }

        public async Task<Tuple<ProposalDTO?, ErrorResult>> PatchProposalStatusAsync(Guid proposalId, ProposalStatusEnum status)
        {
            try
            {
                var proposal = await _crmContext.Proposal.FindAsync(proposalId);

                if (proposal == null)
                    return Tuple.Create<ProposalDTO?, ErrorResult>(null, new()
                    {
                        Error = true,
                        Id = proposalId.ToString(),
                        Message = "Couldn't find proposal for that id",
                        StatusCode = ErrorCode.NotFound
                    });

                proposal.StatusId = (int)status;

                await _crmContext.SaveChangesAsync();

                return Tuple.Create<ProposalDTO?, ErrorResult>(proposal, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return Tuple.Create<ProposalDTO?, ErrorResult>(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to update proposal status with error: {JsonConvert.SerializeObject(e)}"
                });
            }
        }
    }
}
