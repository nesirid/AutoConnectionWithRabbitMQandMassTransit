using App.DTOs.Categories;
using MassTransit;
using Repository.Data;

namespace App.DTOs.Consumers
{
    public class UpdateCategoryConsumer : IConsumer<UpdateCategoryMessage>
    {
        private readonly AppDbContext _context;

        public UpdateCategoryConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UpdateCategoryMessage> context)
        {
            var message = context.Message;

            var category = await _context.Categories.FindAsync(message.Id);
            if (category == null)
            {
                Console.WriteLine($"Categoriya {message.Id} tapilmadi");
                return;
            }
            category.Description = message.Description;
            category.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            Console.WriteLine($"Categoriya {message.Id} yenilendi .");
        }
    }
}
