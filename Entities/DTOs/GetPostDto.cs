using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class GetPostDto : IDto
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
    }
}
