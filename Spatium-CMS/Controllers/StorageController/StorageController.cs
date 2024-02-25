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
using Domain.BlogsAggregate;
using Microsoft.AspNetCore.Hosting;

using Spatium_CMS.Filters;
using Utilities.Enums;
using Infrastructure.Extensions;
using Utilities.Exceptions;
using Utilities.Results;
using Spatium_CMS.Controllers.StorageController.Response;
using Domain.Base;
using Org.BouncyCastle.Utilities;
using System.IO;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.StaticFiles;


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
        private readonly IStorageRepository storageRepository;



        public StorageController(ILogger<StorageController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IAttachmentService attachmentService, IConfiguration configration, IStorageRepository storageRepository) : base(unitOfWork, mapper, logger)
        {
            this.userManager = userManager;
            _attachmentService = attachmentService;
            _configration = configration;
            this.storageRepository = storageRepository;
        }

        #region FolderApis
        [HttpGet]
        [Route("DeleteFolders")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> Delete([FromQuery] int folderId)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId);
                var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId);
                if (folder == null)
                {
                    throw new SpatiumException($"Invalid Folder Id ");
                }
                var deleteResponse = new DeleteResponse()
                {
                    Id = folder.Id,
                    FolderName = folder.Name,
                    FileCount = folder.Files.Count()

                };
                return Ok(deleteResponse);
            });
        }
        [HttpDelete]
        [Route("ConfirmDeleteFolders")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> ConfirmDelete([FromQuery] int folderId)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId);
                var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId);
               
                if (folder == null)
                {
                    throw new SpatiumException($"Invalid Folder Id ");
                }
                folder.Delete();
                await unitOfWork.SaveChangesAsync();

                return Ok($"Folder {folder.Name} Deleted Successfuly");
            });
        }

        [HttpDelete]
        [Route("DeleteBullk")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> DeleteBullk(DeleteBulkRequest deleteBulk)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId);

                foreach (var folderId in deleteBulk.FolderIds)
                {
                    var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId);

                    if (folder == null)
                    {
                        throw new SpatiumException($"Invalid Folder Id ");
                    }
                    folder.Delete();
                }
                foreach (var fileId in deleteBulk.FilesIds)
                {
                    var file = await unitOfWork.StorageRepository.GetFileAsync(fileId);
                    if (file == null)
                    {
                        throw new SpatiumException($"Invalid File Id ");
                    }
                    file.Delete();
                }
                await unitOfWork.SaveChangesAsync();

                return Ok($"Folders and Files  Deleted Successfuly");
            });
        }

        [HttpPut]
        [Route("MoveBullk")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> MoveBullk(MoveBulkRequest moveBulk)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId);

                foreach (var folderId in moveBulk.FolderIds)
                {
                    var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId);

                    if (folder == null)
                    {
                        throw new SpatiumException($"Invalid Folder Id ");
                    }
                    folder.MoveTo(moveBulk.DestinationId);
                }
                foreach (var fileId in moveBulk.FilesIds)
                {
                    var file = await unitOfWork.StorageRepository.GetFileAsync(fileId);
                    if (file == null)
                    {
                        throw new SpatiumException($"Invalid File Id ");
                    }
                    file.MoveToFolderId(moveBulk.DestinationId);
                }
                await unitOfWork.SaveChangesAsync();

                return Ok($"Folders and Files  moved Successfuly");
            });
        }


        [HttpGet()]
        [Route("ShowAllFolders")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.ReadMedia)]
        public Task<IActionResult> ShowAllFolders([FromQuery] int? FolderId)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storage = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId) ?? throw new SpatiumException("Invalid Blog Id");

                List<ViewFolderResponse> respone = new List<ViewFolderResponse>();
                foreach (var folder in storage.Folders.Where(f => f.ParentId == FolderId && f.IsDeleted == false))
                {
                    ViewFolderResponse responseitem = new ViewFolderResponse()
                    {
                        Id = folder.Id,
                        FolderName = folder.Name,
                        CreatedBy = user.FullName,
                        ProfileImage = user.ProfileImagePath
                    };
                    responseitem.NumberOfFolders = folder.Folders.Count();
                    responseitem.NumberOfFiles = folder.Files.Count();
                    respone.Add(responseitem);
                }
                if (respone.Count == 0)
                {
                    throw new SpatiumException("There Is No Folder Here ");
                }

                return Ok(respone);
            });

        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]

        [PermissionFilter(PermissionsEnum.CreateMedia)]

        public Task<IActionResult> Create(CreateFolderRequest Request)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var userId = GetUserId();

                    var user = await userManager.FindByIdAsync(userId);

                    var blogId = GetBlogId();
                    var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound); ;
                    if (await unitOfWork.StorageRepository.ChechNameExists(blogId, Request.ParentId, Request.Name.ToLower()))
                    {
                        throw new SpatiumException($"Name {Request.Name} Already Exist!");
                    }
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
                    _attachmentService.CheckFileExtension(FileRequest.file);
                    string fileName = FileRequest.Name;
                    string filesize = FileRequest.file.Length.ToString();
                    if (string.IsNullOrEmpty(fileName))
                    {
                        return BadRequest("File name is empty or null.");
                    }
                    string newFileName = _attachmentService.GetDesireFileName(FileRequest.file, fileName);
                    _attachmentService.ValidateFileSize(FileRequest.file);
                    string fullfilePath = Path.Combine(uploadPath, newFileName);
                    using (var stream = new FileStream(fullfilePath, FileMode.Create))
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
                    InputFile.FileSize = filesize;
                    InputFile.Extention = _attachmentService.GetFileExtention(FileRequest.file);
                    var file = new StaticFile(InputFile);
                    await unitOfWork.StorageRepository.CreateFileAsync(file);
                    await unitOfWork.SaveChangesAsync();
                    return Ok("File Added Successfully");
                }
                return BadRequest(ModelState);
            });
        }

        [HttpPut]
        [Route("UpdateFile")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> UpdateFile(UpdateFileRequest Request)

                    var response = new SpatiumResponse()
                    {
                        Message = "Folder Created Successfuly ",
                        Success = true
                    };
                    return Ok(response);

                }
                return BadRequest(ModelState);
            });

        }

        [HttpPut]
        [Route("RenameFolder")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.EditMediaMetaInformation)]
        public Task<IActionResult> RenameFolder(RenameFolderRequest Request)

        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {

                    var OldFile = await storageRepository.GetFileAsync(Request.Id);
                    if (OldFile != null)
                    {
                        var UserId = GetUserId();
                        var UpdateFile = mapper.Map<UpdateFileInput>(Request);
                        UpdateFile.LastUpdate = DateTime.Now;
                        UpdateFile.Createdby = UserId;
                        OldFile.Update(UpdateFile);
                        await unitOfWork.SaveChangesAsync();
                        return Ok("UPdated");
                    }

                    var userId = GetUserId();
                    var blogId = GetBlogId();
                    var user = await userManager.FindUserInBlogAsync(blogId, userId);

                    if (await unitOfWork.StorageRepository.ChechNameExists(blogId, Request.ParentId, Request.NewName.ToLower()))
                    {

                        throw new SpatiumException($"Name {Request.NewName} Already Exist!");
                    }
                    var folder = await unitOfWork.StorageRepository.GetFolderByName(Request.OldName.ToLower(), blogId, Request.ParentId);
                    if (folder is null)
                    {
                        throw new SpatiumException($"folder {Request.OldName} Not Exist!");
                    }

                    folder.Rename(Request.NewName);
                    await unitOfWork.SaveChangesAsync();

                    var response = new SpatiumResponse()
                    {
                        Message = $"Folder Renamed Successfuly from {Request.OldName} To {folder.Name} ",
                        Success = true
                    };
                    return Ok(response);


                }
                return BadRequest(ModelState);
            });

        }
        #endregion

        #region FileApis
        [HttpGet]
        [Route("GetAllFiles")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadMedia)]
        public  Task<IActionResult> GetAllFiles([FromQuery]GetEntitiyParams entityParams)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var files =await unitOfWork.StorageRepository.GetAllFilesAsync(entityParams, blogId) ?? throw new SpatiumException("There are not files");
                return Ok(mapper.Map<List<ViewFile>>(files));
            });
        }

        [HttpGet]
        [Route("FilePreview")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadMedia)]
        public Task<IActionResult> FilePreview(int fileId)
        {
            return TryCatchLogAsync(async () =>
            {
                var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException("File Not Found!!");
                return Ok(mapper.Map<ViewFile>(file));
            });
        }

        [HttpGet]
        [Route("CopyURL")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadMedia)]
        public Task<IActionResult> CopyURL(int fileId)
        {
            return TryCatchLogAsync(async () =>
            {
                var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException("File Not Found!!");
                var resualt=mapper.Map<ViewFile>(file);
                var fileUrl = resualt.UrlPath;
                return Ok(fileUrl);
            });
        }

        [HttpGet]
        [Route("DownloadFile")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadMedia)]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException("File Not Found!!");

            var path = Path.Combine(currentDirectory, @"wwwroot\", file.UrlPath);
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(file.UrlPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return File(bytes, contentType, Path.GetFileName(path));
        }


        [HttpDelete]
        [Route("Delete")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> DeleteFile(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    await storageRepository.DeleteFileAsync(Id);
                    await unitOfWork.SaveChangesAsync();
                    return Ok("Deleted");
                }
                return BadRequest(ModelState);
            });
        }

        #endregion

}
}
