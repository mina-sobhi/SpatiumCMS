using AutoMapper;
using Domian.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Spatium_CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CmsControllerBase : ControllerBase
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        public CmsControllerBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        protected async Task<IActionResult> TryCatchLogAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                return await func.Invoke();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected string GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);

        }
    }
}