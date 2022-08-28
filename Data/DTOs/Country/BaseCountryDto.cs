using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DTOs.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(3)]
        public string ShortName { get; set; }
    }
}
