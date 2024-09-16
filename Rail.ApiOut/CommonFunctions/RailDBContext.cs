using Microsoft.EntityFrameworkCore;
using Rail.BO.ApiOutModels;
using Rail.BO.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Rail.ApiOut.CommonFunctions
{
    public class RailDBContext : DbContext
    {
        public DbSet<BookingItemsModel> cartModel { get; set; }
        public DbSet<BookingModel> bookings { get; set; }
        public DbSet<PaxDetailModel> paxDetails { get; set; }
        public DbSet<BookingSectorsModel> sectorsModels { get; set; }
        public DbSet<CorrelationAgentMappingModel> correlationMappings { get; set; }
        public DbSet<ROEModel> rOEs { get; set; }
        public DbSet<PaymentGatewayModel> payments { get; set; }
        public DbSet<ErrorLogsModel> errorLogs { get; set; }
        public DbSet<RailLOGOs> LOGOs { get; set; }
        public DbSet<IsConfirmedModel> isConfirmed { get; set; }
        public DbSet<TokensModel> tokens { get; set; }
        public DbSet<AgentLoginModel> agents { get; set; }
        public DbSet<SearchHistoryModel> history { get; set; }
        public DbSet<ApiOutLogs> apiOutLogs { get; set; }
        public RailDBContext(DbContextOptions<RailDBContext> options) : base(options) { ChangeTracker.LazyLoadingEnabled = false; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentGatewayModel>().HasNoKey();
        }
    }
}
