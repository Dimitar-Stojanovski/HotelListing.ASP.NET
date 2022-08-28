using HotelListing.API.DTOs.Hotels;

namespace HotelListing.API.DTOs.Country;

public class CountryDto:BaseCountryDto
{
    public int Id { get; set; }
    public List<HotelDto> Hotels { get; set; }
}

