using Domain.ApplicationUserAggregate;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Utilities.Exceptions;
using Utilities.Results;
using Infrastructure.Extensions;
using System.Text.Json;

namespace Spatium_CMS.Middlewares
{
    public class ValidateTokenMiddleware
    {
        private readonly RequestDelegate _next;
 

        public ValidateTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private async Task ResponsBody(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var res = new SpatiumErrorResponse
            {
                Message = ResponseMessages.UnauthorizedAccessLoginFirst,
                Path = context.Request.Path,
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize<SpatiumErrorResponse>(res));
        }
        public async Task InvokeAsync(HttpContext context , UserManager<ApplicationUser> userManager)
        {
            var email = context.User?.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var TokenRoleId = context.User?.FindFirstValue("RoleId");
                if (TokenRoleId == null)
                {
                    await ResponsBody(context);
                    return;
                }
                var user = await userManager.FindUserByEmailIgnoreFilter(email);
       
                if (user==null || user.RoleId != TokenRoleId || user.UserStatusId!=1)
                {
                    await ResponsBody(context);
                    return;
                }
                var tokenPermisons =context.User?.Claims.Where(x => x.Type.Equals("Permissions")).Select(x => Convert.ToInt32(x.Value)).ToList();
                if (tokenPermisons ==null || !tokenPermisons.SequenceEqual(user.Role.RolePermission.Where(x=>!x.IsDeleted).Select(p => p.UserPermissionId).ToList()))
                {
                   await ResponsBody(context);
                    return;
                }
            }
            await _next(context);
        }
    }
}
