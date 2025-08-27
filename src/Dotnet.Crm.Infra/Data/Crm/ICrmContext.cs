using Dotnet.Crm.Infra.Data.Crm.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dotnet.Crm.Infra.Data.Crm
{
    public interface ICrmContext
    {
        DbSet<ProposalDTO> Proposal { get; set; }
        DbSet<ClientDTO> Client { get; set; }
        DbSet<StatusDTO> Status { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
