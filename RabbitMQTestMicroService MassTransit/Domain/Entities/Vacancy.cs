using Domain.Common;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vacancy : BaseEntity
    {
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }

        public List<PhoneNumber> PhoneNumbers { get; set; }
    }
}
