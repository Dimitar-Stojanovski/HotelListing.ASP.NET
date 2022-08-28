using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Data.DTOs.Users;
using HotelListing.API.DTOs.Country;
using HotelListing.API.DTOs.Hotels;
using HotelListing.API.DTOs.Users;

namespace HotelListing.API.Configurations
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            
            //Hotel Maps
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            
            CreateMap<ApiUserDto, ApiUser>().ReverseMap();
            CreateMap<LoginDto, ApiUser>().ReverseMap();
            
        }
    }
}
