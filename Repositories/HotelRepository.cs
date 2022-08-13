using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repositories
{
    public class HotelRepository:GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HotelListingDbContext _context;

        public HotelRepository(HotelListingDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetHotelsWithBiggerRatings(double rating)
        {
            var hotels = await _context.Hotels.
                Where(a=>a.Rating>rating).ToListAsync();
            return hotels;
        }

        public async Task<Hotel> IncludeHotelCountry(int id)
        {
            /*var hotel = await _context.Hotels.Include(a => a.Country)
                .FirstOrDefaultAsync();*/

            var hotel = await _context.Hotels.Include(a=>a.Country.Name)
                .FirstOrDefaultAsync(q=>q.Id==id);
            return hotel;
        }
    }
}
