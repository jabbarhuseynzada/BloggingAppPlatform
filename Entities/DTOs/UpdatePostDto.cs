using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class UpdatePostDto : IDto
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; } 
        public string PostContext { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
