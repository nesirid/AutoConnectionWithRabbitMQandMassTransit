
namespace Domain.DTOs.Vacancies
{
    public class CreateVacancyDto
    {
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public IFormFile CompanyLogo { get; set; }
        public int CategoryId { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
