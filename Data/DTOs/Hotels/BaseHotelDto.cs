using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DTOs.Hotels
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Rating { get; set; }
    }
}
