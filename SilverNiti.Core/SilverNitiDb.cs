using Microsoft.EntityFrameworkCore;
using Ozzy.Server;
using Ozzy.Server.EntityFramework;
using SilverNiti.Core.Domain;

namespace SilverNiti.Core
{
    public class SilverNitiDb : AggregateDbContext
    {
        public SilverNitiDb() 
            : base(new DbContextOptionsBuilder<SilverNitiDb>().UseSqlServer("Server=.\\SQLEXPRESS;Database=silverniti-test;Integrated Security=true").Options)
        {
        }

        public SilverNitiDb(IExtensibleOptions<SilverNitiDb> options) : base(options)
        {
        }

        public DbSet<ContactFormMessage> ContactFormMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactFormMessage>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
