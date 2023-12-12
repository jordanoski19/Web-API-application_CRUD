using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=webAPICRUD;Integrated Security=True;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Company)
                .WithMany()
                .HasForeignKey(c => c.CompanyId);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryId);
        }
    }
}
