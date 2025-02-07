using Domain.DTOs.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Countries
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CityDto> Cities { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
