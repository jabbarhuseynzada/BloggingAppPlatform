using Core.Entities.Concrete;
using Entities.DTOs;

namespace BloggingAppPlatform.MVC.Areas.Admin.ViewModels
{
    public class OpClaimVM
    {
        public OpClaimDto opClaim {  get; set; } 
        public List<OperationClaim> OperationClaims { get; set; }
    }
}
