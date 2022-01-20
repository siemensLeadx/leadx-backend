using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class LookupResponseDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class LookupWithGuidResponseDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}
