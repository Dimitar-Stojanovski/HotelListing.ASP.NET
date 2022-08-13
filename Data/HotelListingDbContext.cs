using HotelListing.API.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext:IdentityDbContext<ApiUser>
        //previously was DBContext
    {
        public HotelListingDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country>Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //Initating configuration
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //Making configuration for Countries with the class CountryConfiguration
            modelBuilder.ApplyConfiguration(new CountryConfiguration());

            //Making configuration for Hotels with HotelConfiguration
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
        }

    }
}
