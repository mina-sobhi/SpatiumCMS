using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domian.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Security.Claims;
using Utilities.Exceptions;
using Utilities.Results;

namespace Spatium_CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CmsControllerBase : ControllerBase
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;
        protected readonly ILogger<CmsControllerBase> logger;
        protected readonly UserManager<ApplicationUser> userManager;

        public CmsControllerBase(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CmsControllerBase> logger, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.userManager = userManager;
        }
        protected async Task<IActionResult> TryCatchLogAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                var email = User?.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await userManager.FindUserByEmailIgnoreFilter(email);
                    if (user.RoleId != GetRoleId() || !user.IsAccountActive)
                        throw new SpatiumException(ResponseMessages.UnauthorizedAccessLoginFirst);
                    var tokenPermisons = User.Claims.Where(x => x.Type.Equals("Permissions")).Select(x => Convert.ToInt32(x.Value)).ToList();
                    if (!tokenPermisons.SequenceEqual(user.Role.RolePermission.Select(p => p.UserPermissionId).ToList()))
                    {                   
                        throw new SpatiumException(ResponseMessages.UnauthorizedAccessLoginFirst);
                    }
                }
                return await func.Invoke();
            }
            catch (SpatiumException spatException)
            {
                logger.LogInformation("Exception Message: {message}", spatException.Message);
                var response = new SpatiumErrorResponse()
                {
                    Message = spatException.Message,
                    Path = Request.Path
                };
                return StatusCode(400, response);
            }
            catch (AggregateException aggException)
            {
                if (aggException.InnerException is SpatiumException)
                {
                    logger.LogInformation("Exception Message: {message} \n Stack Trace:\n {stack}", aggException.Message, aggException.StackTrace);
                    var aggResponse = new SpatiumErrorResponse()
                    {
                        Message = aggException.InnerException.Message,
                        Path = Request.Path
                    };
                    return StatusCode(400, aggResponse);
                }
                var response = new SpatiumErrorResponse()
                {
                    Message = aggException.Message,
                    Path = Request.Path
                };
                logger.LogError("Exception Message: {message} \n Stack Trace:\n {stack}", aggException.Message, aggException.StackTrace);
                return StatusCode(400, response);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception Message: {message} \n Stack Trace:\n {stack}", ex.Message, ex.InnerException);
                var response = new SpatiumErrorResponse()
                {
                    Message = ex.Message,
                    Path = Request.Path
                };
                return StatusCode(400, response);
            }
        }

        protected int GetBlogId()
        {
            if (int.TryParse(User?.FindFirstValue("BlogId"), out int result))
            {
                return result;
            };
            throw new SpatiumException(ResponseMessages.UnauthorizedAccessLoginFirst);
        }
        protected string GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new SpatiumException(ResponseMessages.UnauthorizedAccessLoginFirst);
        }

        protected string GetRoleId()
        {
            return User?.FindFirstValue("RoleId") ?? throw new SpatiumException(ResponseMessages.UnauthorizedAccessLoginFirst);
        }
    }
}