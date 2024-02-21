using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.StorageController.Request;
using Domain.StorageAggregate.Input;
using Domain.StorageAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Spatium_CMS.AttachmentService;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Spatium_CMS.Controllers.StorageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : CmsControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<StorageController> logger;
        private readonly IAttachmentService _attachmentService;
        private readonly IConfiguration _configration;

       

        public StorageController(ILogger<StorageController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,  IAttachmentService attachmentService, IConfiguration configration) : base(unitOfWork, mapper, logger)
        {
            this.userManager = userManager;
            _attachmentService = attachmentService;
            _configration = configration;
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
                    var user = await userManager.FindByIdAsync(userId);
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

        [HttpPost]
        [Route("CreateFile")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> CreateFile(AddFileRequest FileRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var blogId = GetBlogId();
                    var UserId = GetUserId();
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blogId.ToString());

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    string fileName = FileRequest.Name;
                    string filepath = FileRequest.Extention;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        return BadRequest("File name is empty or null.");
                    }
                  string newFileName = _attachmentService.GetDesireFileName(FileRequest.file, fileName);
                    _attachmentService.CheckFileExtension(FileRequest.file,filepath);
                  string filePath = Path.Combine(uploadPath, newFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FileRequest.file.CopyToAsync(stream);
                    }

                    string baseUrl = _configration["ApiBaseUrl"];

                    if (string.IsNullOrEmpty(baseUrl))
                    {
                        return StatusCode(500, "Base URL is not configured.");
                    }

                    string imageUrl = $"{baseUrl}/{blogId}/{newFileName}";
                    var InputFile = mapper.Map<FileInput>(FileRequest);
                    InputFile.CreatedById = UserId;
                    InputFile.BlogId = blogId;
                    InputFile.UrlPath = imageUrl;
                    var file = new StaticFile(InputFile);
                    await unitOfWork.StorageRepository.CreateFileAsync(file);
                    await unitOfWork.SaveChangesAsync();
                    return Ok("Added Successfully");
                }
                return BadRequest(ModelState);
            });
        }

    }
}
