using FlowWing.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FlowWing.DataAccess
{
    public class FlowWingDbContext: DbContext
    {
        public FlowWingDbContext(DbContextOptions<FlowWingDbContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); 
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=flowwing;User Id=postgres;Password=1234;\r\n");
        }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<ScheduledEmail> ScheduledEmails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Roles> Roles { get; set; }

    }
}
