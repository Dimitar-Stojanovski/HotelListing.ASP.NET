using HotelListing.API.Data;

namespace HotelListing.API.Contracts
{
    public interface IHotelRepository:IGenericRepository<Hotel>
    {
        Task<Hotel> IncludeHotelCountry(int id);
        Task<List<Hotel>> GetHotelsWithBiggerRatings(double rating);
    }
}
