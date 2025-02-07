using Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DTOs.Categories;
using Repository.Data;
using AutoMapper;
using MassTransit;

namespace App.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;



        public CategoriesController(AppDbContext context,
                                    IMapper mapper,
                                    IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                                           .Where(c => !c.IsDeleted)
                                           .Select(c => new
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            IsDeleted = c.IsDeleted,
            FormattedCreatedDate = c.CreatedDate.ToString("dd MMMM yyyy HH:mm:ss"),
            FormattedUpdatedDate = c.UpdatedDate.ToString("dd MMMM yyyy HH:mm:ss"),
            FormattedDeletedDate = c.DeletedDate.HasValue
            ? c.DeletedDate.Value.ToString("dd MMMM yyyy HH:mm:ss") : "Not Deleted"
        })
                                           .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = $"İd {id} olan kateqoriyalar fond deyil" });

            var result = new
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsDeleted = category.IsDeleted,
            FormattedCreatedDate = category.CreatedDate.ToString("dd MMMM yyyy HH:mm:ss"),
            FormattedUpdatedDate = category.UpdatedDate.ToString("dd MMMM yyyy HH:mm:ss"),
            FormattedDeletedDate = category.DeletedDate.HasValue
            ? category.DeletedDate.Value.ToString("dd MMMM yyyy HH:mm:ss") : "Not Deleted"
        };

            return Ok(result);
        }

        [HttpPatch("{id}/description")]
        public async Task<IActionResult> UpdateCategoryDescription(int id, [FromBody] string description)
        {
            if (string.IsNullOrEmpty(description))
                return BadRequest(new { message = "Təsvir boş ola bilməz" });

            await _publishEndpoint.Publish(new UpdateCategoryMessage
            {
                Id = id,
                Description = description
            });

            return Ok(new { message = "Yeniləmə sorğusu növbəyə göndərildi" });
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedCategories()
        {
            var deletedCategories = await _context.Categories
                                                  .Where(c => c.IsDeleted)
                                                  .ToListAsync();

            var categoryDtos = _mapper.Map<List<CategoryDto>>(deletedCategories);
            return Ok(categoryDtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(dto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = $"ID {id} olan kateqoriya tapılmadı" });

            _mapper.Map(dto, category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = $"ID {id} olan kateqoriya tapılmadı" });

            category.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"ID {id} olan kateqoriya yumşaq silindi" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = $"ID {id} olan kateqoriya tapılmadı" });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"ID {id} olan kateqoriya silindi" });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = $"ID  {id}  olan kateqoriya tapılmadı" });

            if (!category.IsDeleted)
                return BadRequest(new { message = $"ID {id} olan kateqoriya silinməyib" });

            category.IsDeleted = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"ID {id} olan kateqoriya bərpa edildi" });
        }
    }
}
