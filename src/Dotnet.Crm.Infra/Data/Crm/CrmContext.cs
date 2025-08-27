using Dotnet.Crm.Domain.SeedWork;
using Dotnet.Crm.Infra.Data.Crm.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Crm.Infra.Data.Crm
{
    public class CrmContext(DbContextOptions<CrmContext> options, EnvironmentKey environmentKey, bool test = false) : DbContext(options), ICrmContext
    {
        private readonly EnvironmentKey _environmentKey = environmentKey;
        public DbSet<ProposalDTO> Proposal { get; set; }
        public DbSet<ClientDTO> Client { get; set; }
        public DbSet<StatusDTO> Status { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!test)
            {
                MySqlServerVersion serverVersion = new(new Version(8, 0, 29));
                if (EnvironmentKey.TypeInformation != EnvironmentKey.Type.DEV)
                {
                    dbContextOptionsBuilder
                    .UseMySql(_environmentKey.MySqlInformation.ConnectionString, serverVersion);
                }
                else
                {
                    dbContextOptionsBuilder
                    .UseMySql(_environmentKey.MySqlInformation.ConnectionString, serverVersion)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
                }
            }
            else
            {
                dbContextOptionsBuilder
                    .UseInMemoryDatabase($"test_db_{Guid.NewGuid()}")
                    .EnableDetailedErrors();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProposalDTO>()
                .Property(o => o.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<ProposalDTO>()
                .HasOne(o => o.Status)
                .WithMany()
                .HasForeignKey(o => o.StatusId)
                .IsRequired();

            modelBuilder.Entity<ProposalDTO>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .IsRequired();

            modelBuilder.Entity<ProposalDTO>()
                .HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.ProposalId)
                .IsRequired();
        }
    }
}
