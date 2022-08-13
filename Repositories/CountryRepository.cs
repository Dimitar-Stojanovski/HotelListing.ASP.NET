using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repositories
{
    public class CountryRepository:GenericRepository<Country>, ICountiesRepository
    {
        private readonly HotelListingDbContext _context;
        public CountryRepository(HotelListingDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Country> GetDetailsForCountry(int id)
        {
            var entity= await _context.Countries.Include(q => q.Hotels)
                .FirstOrDefaultAsync(q => q.Id == id);
            
            return entity;
        }
    }
}
