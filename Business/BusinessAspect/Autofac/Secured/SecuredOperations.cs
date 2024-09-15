using Castle.DynamicProxy;
using Core.Extensions;
using Core.Helpers.Interceptors;
using Core.Helpers.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Business.BusinessAspect.Autofac.Secured
{
    public class SecuredOperation : MethodInterception
    {
        private readonly string[] _roles;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var context = _httpContextAccessor?.HttpContext;
            if (context == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            var roleClaims = context.User.ClaimRoles();



            foreach (var roleClaim in _roles)
            {
                if (roleClaims.Contains(roleClaim))
                {
                    
                    //burda ifle eger userdise id-si yoxlanilsin elave olunmalidi cunki onsuz rollar table-si yarananda default olaraq user, admin, mod elave etmisem 
                    return;
                }
            }

            throw new Exception("You do not have permission");
        }
    }
}
