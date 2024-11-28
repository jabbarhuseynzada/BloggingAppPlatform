using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class OpClaimDto : IDto
    {
        public string Username { get; set; }
        public string ClaimName { get; set; }
    }
}
