using Microsoft.EntityFrameworkCore;
using Domain.Entites;
using Domain.Entities;

namespace Repository.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Cities)
                .WithOne(c => c.Country)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.Country)
                .WithMany()
                .HasForeignKey(v => v.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.City)
                .WithMany()
                .HasForeignKey(v => v.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhoneNumber>()
                .HasOne(p => p.Vacancy)
                .WithMany(v => v.PhoneNumbers)
                .HasForeignKey(p => p.VacancyId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
