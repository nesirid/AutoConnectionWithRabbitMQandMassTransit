using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Cities
{
    public class CreateCityDto
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
