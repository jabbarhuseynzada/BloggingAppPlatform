using Core.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ReportDto : IDto
    {
        public string ReportedBy { get; set; }
        public string ReportedUser { get; set; }
        public IFormFile Photo { get; set; }
    }
}
