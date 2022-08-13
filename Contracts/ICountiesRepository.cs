using HotelListing.API.Data;

namespace HotelListing.API.Contracts
{
    public interface ICountiesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetailsForCountry(int id);
    }
}
