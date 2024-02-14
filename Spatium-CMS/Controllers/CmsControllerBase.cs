using AutoMapper;
using Domian.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utilities.Exceptions;

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
            catch (SpatiumException svuScholarshipException)
            {
                _logger.LogInformation("Exception Message: {message}", svuScholarshipException.Message);
                return StatusCode(402, svuScholarshipException.Message);
            }
            catch (AggregateException aggException)
            {
                if (aggException.InnerException is SpatiumException)
                {
                    _logger.LogInformation("Exception Message: {message} \n Stack Trace:\n {stack}", aggException.Message, aggException.StackTrace); ;
                    return StatusCode(402, (aggException.InnerException as SpatiumException).Message);
                }
                _logger.LogError("Exception Message: {message} \n Stack Trace:\n {stack}", aggException.Message, aggException.StackTrace);
                return StatusCode(402, "Error");
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Message: {message} \n Stack Trace:\n {stack}", ex.Message, ex.InnerException);
                return StatusCode(402, "Error");
            }
        }

        protected string GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);

        }
    }
}