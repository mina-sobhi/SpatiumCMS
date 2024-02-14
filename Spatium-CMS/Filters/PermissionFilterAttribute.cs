using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Utilities.Enums;

namespace Spatium_CMS.Filters
{
    public class PermissionFilterAttribute:ActionFilterAttribute
    {
        private readonly int PermissionValue;
        private readonly int PermissionValue2;
        
        public PermissionsEnum PermissionsEnum { get; private set; }
        public PermissionsEnum PermissionsEnum2 { get; private set; }

        public PermissionFilterAttribute(PermissionsEnum PermissionsEnum, PermissionsEnum PermissionsEnum2=0)
        {
            PermissionValue=(int) PermissionsEnum;
            PermissionValue2 = (int)PermissionsEnum2;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var identityClaims=context.HttpContext.User.Identity as ClaimsIdentity;
            var claimsList = identityClaims.Claims.Where(x=>x.Type.Equals(ClaimTypes.Role)).ToList();
            if(identityClaims !=null)
            {
                if(!claimsList.Any(x=>x.Value==PermissionValue.ToString() || x.Value==PermissionValue2.ToString())) {
                    context.Result = new UnauthorizedObjectResult(new
                    {
                        status = false,
                        Message = "You don't have permission to Access!"
                    });
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    status = false,
                    Message = "You don't have permission to Access!"
                });
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}