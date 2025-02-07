namespace App.DTOs.Vacancies
{
    public class VacancyDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string CategoryName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
