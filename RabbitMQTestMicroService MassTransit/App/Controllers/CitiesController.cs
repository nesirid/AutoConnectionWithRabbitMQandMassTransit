using AutoMapper;
using Domain.DTOs.Cities;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace App.Controllers
{
    public class CitiesController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CitiesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _context.Cities.Include(c => c.Country).ToListAsync();
            var cityDtos = _mapper.Map<List<CityDto>>(cities);
            return Ok(cityDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id)
        {
            var city = await _context.Cities.Include(c => c.Country).FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
                return NotFound(new { message = $"ID {id} olan şəhər tapılmadı" });

            var cityDto = _mapper.Map<CityDto>(city);
            return Ok(cityDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity([FromBody] CreateCityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var city = _mapper.Map<City>(dto);
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            var cityDto = _mapper.Map<CityDto>(city);
            return CreatedAtAction(nameof(GetCity), new { id = cityDto.Id }, cityDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
                return NotFound(new { message = $"ID {id} olan şəhər tapılmadı" });

            _mapper.Map(dto, city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
                return NotFound(new { message = $"ID  {id}  olan şəhər tapılmadı" });

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"ID {id} olan şəhər silindi" });
        }
    }
}
