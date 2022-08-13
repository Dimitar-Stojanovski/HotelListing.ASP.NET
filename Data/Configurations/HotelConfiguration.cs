using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class HotelConfiguration:IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(

                 new Hotel
                 {
                     Id = 1,
                     Name = "Kingston Hotel",
                     Address = "Kingston street 3/33",
                     Rating = 4.5,
                     CountryId = 1,

                 },
                new Hotel
                {
                    Id = 2,
                    Name = "Hotel Drim",
                    Address = "Street 32",
                    Rating = 5.0,
                    CountryId = 2,
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Hotel Moskva",
                    Address = "Knez Mihajlova",
                    Rating = 3.0,
                    CountryId = 3

                }

                );
        }
    }
}
