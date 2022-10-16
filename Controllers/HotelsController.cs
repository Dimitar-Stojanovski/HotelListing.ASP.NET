using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Repositories;
using AutoMapper;
using HotelListing.API.DTOs.Hotels;
using HotelListing.API.Contracts;
using HotelListing.API.Exceptions;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            var records = _mapper.Map<List<HotelDto>>(hotels);
            return records;
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync(id);
            var record = _mapper.Map<HotelDto>(hotel);

            if (hotel == null)
            {
                throw new NotFoundException(nameof(GetHotel), id);
            }

            return record;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDto _updateHotel)
        {
           

            var hotel= await _hotelRepository.GetAsync(id);

            if (hotel == null)
            {
                throw new NotFoundException(nameof(PutHotel), id);
            }

            _mapper.Map(_updateHotel, hotel);

            try
            {
                await _hotelRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    throw new NotFoundException(nameof(PutHotel), id);
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
        {
            var _hotel = _mapper.Map<Hotel>(hotelDto);

            await _hotelRepository.AddAsync(_hotel);

            return CreatedAtAction("GetHotel", new { id = _hotel.Id }, _hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync(id);
            if (hotel == null)
            {
                throw new NotFoundException(nameof(DeleteHotel), id);
            }

           await _hotelRepository.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("ratings/{ratings}")]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetRatings(double ratings)
        {
            var hotels = await _hotelRepository.GetHotelsWithBiggerRatings(ratings);
            var records = _mapper.Map<List<HotelDto>>(hotels);
            return records;
        }



        private async Task <bool> HotelExists(int id)
        {
            return await _hotelRepository.Exist(id);
        }
    }
}
