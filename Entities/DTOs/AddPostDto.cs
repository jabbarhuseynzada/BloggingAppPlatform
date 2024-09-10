using Core.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class AddPostDto : IDto
    {
        //add(postDto){post.title = postDto.PostTitle}
        public int UserId { get; set; }
        public string PostTitle { get; set; }
        public string PostContext { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
