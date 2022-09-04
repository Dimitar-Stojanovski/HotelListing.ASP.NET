using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using AutoMapper;
using HotelListing.API.DTOs.Country;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CountriesController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly ICountiesRepository _countiesRepository;

        public CountriesController( IMapper mapper, ICountiesRepository countiesRepository)
        {
            
            _mapper = mapper;
            _countiesRepository = countiesRepository;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {

            var _countries = await _countiesRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(_countries);


            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
          
            var country = await _countiesRepository.GetDetailsForCountry(id);

            var record = _mapper.Map<CountryDto>(country);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
        {
            

            var country = await _countiesRepository.GetAsync(id);
            
            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountry, country);


            try
            {
                await _countiesRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
        {
           

            var country = _mapper.Map<Country>(createCountry);

           await _countiesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {

            var country = await _countiesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _countiesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task <bool> CountryExists(int id)
        {
            return await _countiesRepository.Exist(id);
        }
    }
}
