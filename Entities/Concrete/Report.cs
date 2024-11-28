using Core.Entities.Abstract;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Report : BaseEntity
    {
        public string ReporetedBy { get; set; } //Username of who reports the post
        public string ReportedUser { get; set; } //Username of the reported user
        public string PhotoUrl { get; set; }
    }
}
