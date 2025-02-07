using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Cities
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
