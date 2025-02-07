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

    }
}
