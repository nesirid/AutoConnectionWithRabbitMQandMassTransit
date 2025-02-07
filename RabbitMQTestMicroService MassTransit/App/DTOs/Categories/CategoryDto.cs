namespace App.DTOs.Categories
{
    public class CategoryDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; } 
        public DateTime? DeletedDate { get; set; }
    }
}
