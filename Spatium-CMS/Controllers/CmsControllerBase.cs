using AutoMapper;
using Domian.Interfaces;
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
        private readonly ILogger<CmsControllerBase> _logger;

        public CmsControllerBase(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CmsControllerBase> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _logger = logger;
        }
        protected async Task<IActionResult> TryCatchLogAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                return await func.Invoke();
            }
            catch (SpatiumException spatException)
            {
                _logger.LogInformation("Exception Message: {message}", spatException.Message);
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
                    _logger.LogInformation("Exception Message: {message} \n Stack Trace:\n {stack}", aggException.Message, aggException.StackTrace);
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
                _logger.LogError("Exception Message: {message} \n Stack Trace:\n {stack}", aggException.Message, aggException.StackTrace);
                return StatusCode(400, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Message: {message} \n Stack Trace:\n {stack}", ex.Message, ex.InnerException);
                var response = new SpatiumErrorResponse()
                {
                    Message = ex.Message,
                    Path = Request.Path
                };
                return StatusCode(402, response);
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
            return User?.FindFirstValue("RoleId")?? throw new SpatiumException(ResponseMessages.UnauthorizedAccessLoginFirst);
        }
    }
}