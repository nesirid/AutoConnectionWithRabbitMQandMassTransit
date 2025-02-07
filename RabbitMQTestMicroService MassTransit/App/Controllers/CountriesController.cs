using AutoMapper;
using Domain.DTOs.Countries;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace App.Controllers
{
    public class CountriesController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CountriesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _context.Countries.Include(c => c.Cities).ToListAsync();
            var countryDtos = _mapper.Map<List<CountryDto>>(countries);
            return Ok(countryDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _context.Countries.Include(c => c.Cities).FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
                return NotFound(new { message = $"ID {id} olan ölkə tapılmadı" });

            var countryDto = _mapper.Map<CountryDto>(country);
            return Ok(countryDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry([FromBody] CreateCountryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _mapper.Map<Country>(dto);
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            var countryDto = _mapper.Map<CountryDto>(country);
            return CreatedAtAction(nameof(GetCountry), new { id = countryDto.Id }, countryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
                return NotFound(new { message = $"ID {id} olan ölkə tapılmadı" });

            _mapper.Map(dto, country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
                return NotFound(new { message = $"ID  {id}  olan ölkə tapılmadı" });

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"ID {id} olan ölkə silindi" });
        }
    }

}