using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.StorageController.Request;
using Domain.StorageAggregate.Input;
using Domain.StorageAggregate;
using Microsoft.AspNetCore.Authorization;
using Spatium_CMS.AttachmentService;
using Spatium_CMS.Filters;
using Utilities.Enums;
using Infrastructure.Extensions;
using Utilities.Exceptions;
using Utilities.Results;
using Spatium_CMS.Controllers.StorageController.Response;
using Domain.Base;
using Microsoft.AspNetCore.StaticFiles;

namespace Spatium_CMS.Controllers.StorageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : CmsControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IConfiguration _configration;
        public StorageController(ILogger<StorageController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IAttachmentService attachmentService, IConfiguration configration, IStorageRepository storageRepository) : base(unitOfWork, mapper, logger, userManager)
        {
            _attachmentService = attachmentService;
            _configration = configration;
        }

        #region FolderApis
        [HttpGet]
        [Route("DeleteFolders")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> Delete([FromQuery] int folderId)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId) ?? throw new SpatiumException(ResponseMessages.StorageNotFound);
                var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
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
        [Authorize]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> ConfirmDelete([FromQuery] int folderId)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId) ?? throw new SpatiumException(ResponseMessages.StorageNotFound);
                var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId) ?? throw new SpatiumException($"Invalid Folder Id ");
                folder.Delete();
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = $"Folder {folder.Name} Deleted Successfuly",
                    Success = true,
                };
                return Ok(response);
            });
        }

        [HttpDelete]
        [Route("DeleteBullk")]
        [Authorize]
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
                    var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
                    folder.Delete();
                }
                foreach (var fileId in deleteBulk.FilesIds)
                {
                    var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException(ResponseMessages.InvalidFileName);
                    file.Delete();
                }
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = ResponseMessages.FoldersAndFilesDeletedSuccessfully,
                    Success = true,
                };
                return Ok(response);
            });
        }

        [HttpPut]
        [Route("MoveBullk")]
        [Authorize]
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
                    var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
                    folder.MoveTo(moveBulk.DestinationId);
                }
                foreach (var fileId in moveBulk.FilesIds)
                {
                    var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
                    file.MoveToFolderId(moveBulk.DestinationId);
                }
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = ResponseMessages.FoldersAndFilesMovedSuccessfully,
                    Success = true,
                };
                return Ok(response);
            });
        }

        [HttpGet()]
        [Route("ShowAllFolders")]
        [Authorize]
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
                return Ok(respone);
            });
        }

        [HttpPost("Folder")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.CreateMedia)]
        public Task<IActionResult> Create(CreateFolderRequest Request)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var userId = GetUserId();
                    var blogId = GetBlogId();
                    var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                    if (await unitOfWork.StorageRepository.ChechNameExists(blogId, Request.ParentId, Request.Name.ToLower()))
                    {
                        throw new SpatiumException($"{Request.Name} Already Exist!");
                    }
                    var input = mapper.Map<AddFolderInput>(Request);
                    input.CreatedById = userId;
                    input.BlogId = blogId;

                    var folder = new Folder(input);

                    await unitOfWork.StorageRepository.CreateFolderAsync(folder);
                    await unitOfWork.SaveChangesAsync();
                    var response = new SpatiumResponse()
                    {
                        Message = ResponseMessages.FolderCreatedSuccessfully,
                        Success = true,
                    };
                    return Ok(folder);
                }
                return BadRequest(ModelState);
            });

        }

        [HttpPost]
        [Route("CreateFile")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.CreateMedia)]
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
                        throw new SpatiumException(ResponseMessages.InvalidFileName);
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
                    var response = new SpatiumResponse()
                    {
                        Message = ResponseMessages.FileAddedSuccessfully,
                        Success = true,
                    };
                    return Ok(response);
                }
                return BadRequest(ModelState);
            });
        }

        [HttpPut]
        [Route("RenameFolder")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.UpdateMedia)]
        public Task<IActionResult> RenameFolder(RenameFolderRequest Request)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
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
        public Task<IActionResult> GetAllFiles([FromQuery] GetEntitiyParams entityParams)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var files = await unitOfWork.StorageRepository.GetAllFilesAsync(entityParams, blogId);
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
                var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException(ResponseMessages.FileNotFound);
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
                var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException(ResponseMessages.FileNotFound);
                var resualt = mapper.Map<ViewFile>(file);
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
            var file = await unitOfWork.StorageRepository.GetFileAsync(fileId) ?? throw new SpatiumException(ResponseMessages.FileNotFound);

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
        [Authorize]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> DeleteFile(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    await unitOfWork.StorageRepository.DeleteFileAsync(Id);
                    await unitOfWork.SaveChangesAsync();
                    var response = new SpatiumResponse()
                    {
                        Message = ResponseMessages.FileDeletedSuccessfully,
                        Success = true
                    };
                    return Ok(response);
                }
                return BadRequest(ModelState);
            });
        }
        #endregion
    }
}
