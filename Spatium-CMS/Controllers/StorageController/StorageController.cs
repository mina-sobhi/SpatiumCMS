using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.StorageController.Request;
using Domain.StorageAggregate.Input;
using Domain.StorageAggregate;
using Microsoft.AspNetCore.Authorization;

namespace Spatium_CMS.Controllers.StorageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : CmsControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<StorageController> logger;

        public StorageController(ILogger<StorageController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, mapper, logger)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles ="Super Admin")]
        public Task<IActionResult> Create(CreateFolderRequest Request)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var userId = GetUserId();
                    var user = userManager.FindByIdAsync(userId);
                    var blogId = GetBlogId();
                    var input = mapper.Map<AddFolderInput>(Request);
                    input.CreatedById = userId;
                    input.BlogId = blogId;
                    var folder = new Folder(input);
                    await unitOfWork.StorageRepository.CreateFolderAsync(folder);
                    await unitOfWork.SaveChangesAsync();

                }
                return BadRequest(ModelState);
            });

        }

    }
}
