using App.DTOs.Vacancies;
using Domain.DTOs.Vacancies;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace App.Controllers
{
    public class VacanciesController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public VacanciesController(AppDbContext context,
                                   IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVacancy([FromForm] CreateVacancyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId))
                return BadRequest(new { message = $"ID {dto.CategoryId} olan kateqoriya mövcud deyil" });

            if (!await _context.Countries.AnyAsync(c => c.Id == dto.CountryId))
                return BadRequest(new { message = $"D {dto.CountryId} olan ölkə mövcud deyil" });

            if (!await _context.Cities.AnyAsync(c => c.Id == dto.CityId && c.CountryId == dto.CountryId))
                return BadRequest(new { message = $"{dto.CityId} ID-li şəhər {dto.CountryId} ID-li Ölkəyə aid deyil." });

            var vacancy = new Vacancy
            {
                Title = dto.Title,
                CompanyName = dto.CompanyName,
                CategoryId = dto.CategoryId,
                CountryId = dto.CountryId,
                CityId = dto.CityId,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumbers = dto.PhoneNumbers.Select(p => new PhoneNumber { Number = p }).ToList()
            };

            if (dto.CompanyLogo != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.CompanyLogo.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.CompanyLogo.CopyToAsync(stream);
                }

                vacancy.CompanyLogo = $"/uploads/{uniqueFileName}";
            }

            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVacancy), new { id = vacancy.Id }, new { message = "Vakansiya uğurla yaradıldı" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVacancy(int id)
        {
            var vacancy = await _context.Vacancies
                .Include(v => v.Category)
                .Include(v => v.Country)
                .Include(v => v.City)
                .Include(v => v.PhoneNumbers)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vacancy == null)
                return NotFound(new { message = $"ID {id} olan vakansiya tapılmadı" });

            var vacancyDto = new VacancyDto
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                CompanyName = vacancy.CompanyName,
                CompanyLogo = vacancy.CompanyLogo,
                CategoryName = vacancy.Category?.Name,
                CountryName = vacancy.Country?.Name,
                CityName = vacancy.City?.Name,
                Address = vacancy.Address,
                Email = vacancy.Email,
                PhoneNumbers = vacancy.PhoneNumbers.Select(p => p.Number).ToList()
            };

            return Ok(vacancyDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVacancies()
        {
            var vacancies = await _context.Vacancies
                .Where(v => !v.IsDeleted)
                .Include(v => v.Category)
                .Include(v => v.Country)
                .Include(v => v.City)
                .Include(v => v.PhoneNumbers)
                .ToListAsync();

            if (!vacancies.Any())
                return NotFound(new { message = "Heç bir vakansiya tapılmadı" });

            var vacancyDtos = vacancies.Select(v => new VacancyDto
            {
                Id = v.Id,
                Title = v.Title,
                CompanyName = v.CompanyName,
                CompanyLogo = v.CompanyLogo,
                CategoryName = v.Category?.Name,
                CountryName = v.Country?.Name,
                CityName = v.City?.Name,
                Address = v.Address,
                Email = v.Email,
                PhoneNumbers = v.PhoneNumbers.Select(p => p.Number).ToList()
            }).ToList();

            return Ok(vacancyDtos);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedVacancies()
        {
            var deletedVacancies = await _context.Vacancies
                .Where(v => v.IsDeleted)
                .Include(v => v.Category)
                .Include(v => v.Country)
                .Include(v => v.City)
                .Include(v => v.PhoneNumbers)
                .ToListAsync();

            if (!deletedVacancies.Any())
                return NotFound(new { message = "Silinmiş vakansiyalar tapılmadı" });

            var result = deletedVacancies.Select(v => new
            {
                v.Id,
                v.Title,
                v.CompanyName,
                v.CompanyLogo,
                CategoryName = v.Category?.Name,
                CountryName = v.Country?.Name,
                CityName = v.City?.Name,
                v.Address,
                v.Email,
                PhoneNumbers = v.PhoneNumbers.Select(p => p.Number).ToList(),
                DeletedDate = v.DeletedDate?.ToString("dd MMMM yyyy HH:mm:ss") ?? "Mövcud deyil"
            }).ToList();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVacancy(int id, [FromForm] CreateVacancyDto dto)
        {
            var vacancy = await _context.Vacancies.Include(v => v.PhoneNumbers).FirstOrDefaultAsync(v => v.Id == id);
            if (vacancy == null)
                return NotFound(new { message = $"ID {id} olan vakansiya tapılmadı" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId))
                return BadRequest(new { message = $"ID {dto.CategoryId} olan kateqoriya mövcud deyil" });

            if (!await _context.Countries.AnyAsync(c => c.Id == dto.CountryId))
                return BadRequest(new { message = $"ID {dto.CountryId} olan ölkə mövcud deyil" });

            if (!await _context.Cities.AnyAsync(c => c.Id == dto.CityId && c.CountryId == dto.CountryId))
                return BadRequest(new { message = $"{dto.CityId} ID-li şəhər {dto.CountryId} ID-li Ölkəyə aid deyil" });

            vacancy.Title = dto.Title;
            vacancy.CompanyName = dto.CompanyName;
            vacancy.CategoryId = dto.CategoryId;
            vacancy.CountryId = dto.CountryId;
            vacancy.CityId = dto.CityId;
            vacancy.Address = dto.Address;
            vacancy.Email = dto.Email;

            vacancy.PhoneNumbers.Clear();
            vacancy.PhoneNumbers.AddRange(dto.PhoneNumbers.Select(p => new PhoneNumber { Number = p }));

            if (dto.CompanyLogo != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.CompanyLogo.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.CompanyLogo.CopyToAsync(stream);
                }

                vacancy.CompanyLogo = $"/uploads/{uniqueFileName}";
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"ID {id} olan vakansiya uğurla yeniləndi" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
                return NotFound(new { message = $"ID  {id}  olan vakansiya tapılmadı" });

            vacancy.IsDeleted = true;
            vacancy.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = $"ID {id} olan vakansiya silindi" });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreVacancy(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null || !vacancy.IsDeleted)
                return NotFound(new { message = $"ID {id} olan vakansiya tapılmadı və ya silinmədi" });

            vacancy.IsDeleted = false;
            vacancy.DeletedDate = null;

            await _context.SaveChangesAsync();
            return Ok(new { message = $"ID {id} olan vakansiya bərpa edildi" });
        }
    }
}
