using CRM_Sales_Core.Entites;
using Microsoft.EntityFrameworkCore;

namespace CRM_Sales_Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<SalesAgent> SalesAgents { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // SalesAgent Self Reference
            modelBuilder.Entity<SalesAgent>()
                .HasOne(a => a.Leader)
                .WithMany(a => a.Agents)
                .HasForeignKey(a => a.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SalesAgent>()
                .HasOne(a => a.Team)
                .WithMany(t => t.SalesAgents)
                .HasForeignKey(a => a.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Client - Agent
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Agent)
                .WithMany(a => a.Clients)
                .HasForeignKey(c => c.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Client - PreviousAgent
            modelBuilder.Entity<Client>()
                .HasOne(c => c.PreviousAgent)
                .WithMany()
                .HasForeignKey(c => c.PreviousAgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .Property(t => t.Floor)
                .HasConversion<string>();
        }
    }
}
