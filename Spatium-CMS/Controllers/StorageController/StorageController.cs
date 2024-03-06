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

using static System.Net.Mime.MediaTypeNames;

using Spatium_CMS.Controllers.UserRoleController.Request;

namespace Spatium_CMS.Controllers.StorageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : CmsControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        private readonly IConfiguration _configration;
        public StorageController(ILogger<StorageController> logger, IMapper mapper, 
                                IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
                                IAttachmentService attachmentService, IConfiguration configration)
                                : base(unitOfWork, mapper, logger, userManager)
        {
            _attachmentService = attachmentService;
            _configration = configration;
        }

        #region FolderApis
        [HttpGet]
        [Route("FoldersAndFileCount")]
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
                var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId,blogId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
                var deleteResponse = new DeleteResponse()
                {
                    Id = folder.Id,
                    FolderName = folder.Name,
                    FileCount = folder.Files.Count(),
                    FoldersCount=folder.Folders.Count(),
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
                var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId, blogId) ?? throw new SpatiumException($"Invalid Folder Id ");
                folder.Delete();
                foreach (var file in folder.Files)
                {
                    await unitOfWork.StorageRepository.DeleteFileAsync(file.Id, blogId);
                }
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
        [Route("DeleteFolderBullk")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> DeleteFolderBullk(DeleteBulkRequest deleteBulk)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId);
                bool isApplied = false;
                foreach (var folderId in deleteBulk.FolderIds)
                {
                    var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId, blogId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
                    folder.Delete();
                    isApplied = true;
                }
                foreach (var fileId in deleteBulk.FilesIds)
                {
                    var file = await unitOfWork.StorageRepository.GetFileAsync(fileId,blogId) ?? throw new SpatiumException(ResponseMessages.InvalidFileName);
                    file.Delete();
                    isApplied = true;
                }
                if (isApplied)
                {
                    await unitOfWork.SaveChangesAsync();
                    var response = new SpatiumResponse()
                    {
                        Message = ResponseMessages.FoldersAndFilesDeletedSuccessfully,
                        Success = true,
                    };
                    return Ok(response);
                }
                throw new SpatiumException("Empty List!");
            });
        }

        [HttpPut]
        [Route("MoveFolderBullk")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> MoveFolderBullk(MoveBulkRequest moveBulk)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var storag = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId);
                var destinationFolder = await unitOfWork.StorageRepository.GetFolderAsync(moveBulk.DestinationId.Value, blogId) ?? throw new SpatiumException("Invalid Folder Destination");
                
                if(moveBulk.DestinationId!=null && moveBulk.FolderIds.Contains(moveBulk.DestinationId.Value)) 
                    throw new SpatiumException("Invalid Destination!");
                foreach (var folderId in moveBulk.FolderIds)
                {
                    var folder = await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId, blogId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
                    if(folder.ParentId == null && moveBulk.DestinationId==null) throw new SpatiumException("Invalid Destination!");
                    folder.MoveTo(moveBulk.DestinationId);
                }
                foreach (var fileId in moveBulk.FilesIds)
                {
                    var file = await unitOfWork.StorageRepository.GetFileAsync(fileId, blogId) ?? throw new SpatiumException(ResponseMessages.InvalidFolder);
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
                foreach (var folder in storage.Folders.Where(f => f.ParentId == FolderId))
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
                    if (Request.ParentId !=null && Request.ParentId > 0)
                    {
                        var fo = await unitOfWork.StorageRepository.GetFolderAsync(Request.ParentId.Value,blogId)?? throw new SpatiumException(ResponseMessages.InvalidFolder);
                    }
                    else
                        Request.ParentId = null;
                    var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                    var storage = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId);
                    if (await unitOfWork.StorageRepository.ChechNameExists(blogId, Request.ParentId, Request.Name.ToLower()))
                    {
                        throw new SpatiumException($"{Request.Name} Already Exist!");
                    }
                    var input = mapper.Map<AddFolderInput>(Request);
                    input.CreatedById = userId;
                    input.BlogId = blogId;
                    input.StorageId = storage.Id;

                    var folder = new Folder(input);

                    await unitOfWork.StorageRepository.CreateFolderAsync(folder);
                    await unitOfWork.SaveChangesAsync();

                    var response = new SpatiumResponse()
                    {
                        Message = ResponseMessages.FolderCreatedSuccessfully,
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

                    if (FileRequest.FolderId!=null)
                    {
                        var folder = await unitOfWork.StorageRepository.GetFolderAsync(FileRequest.FolderId.Value, blogId) ?? throw new SpatiumException($"Folder Not Found");
                    }

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
                    if (await unitOfWork.StorageRepository.ChechFileNameExists(fileName,FileRequest.FolderId))
                    {
                        throw new SpatiumException($"{fileName} Already Exist!");
                    }

                    _attachmentService.ValidateFileSize(FileRequest.file);
                    string imageUrl = $"{blogId}/{newFileName}";
                    var InputFile = mapper.Map<FileInput>(FileRequest);
                    InputFile.CreatedById = UserId;
                    InputFile.BlogId = blogId;
                    InputFile.UrlPath = imageUrl;
                    InputFile.FileSize = filesize;
                    InputFile.FolderId = FileRequest.FolderId;
                    InputFile.Extention = _attachmentService.GetFileExtention(FileRequest.file);
                    var file = new StaticFile(InputFile);
                    string fullfilePath = Path.Combine(uploadPath, newFileName);
                    using (var stream = new FileStream(fullfilePath, FileMode.Create))
                    {
                        await FileRequest.file.CopyToAsync(stream);
                    }
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
        [Route("UpdateFile")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.UpdateMedia)]
        public Task<IActionResult> UpdateFile(UpdateFileRequest Request)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var blogId = GetBlogId();
                    var OldFile = await unitOfWork.StorageRepository.GetFileAsync(Request.Id, blogId);
                    if (OldFile != null)
                    {
                        string fileName = Request.Name;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            throw new SpatiumException(ResponseMessages.InvalidFileName);
                        }
                        if (await unitOfWork.StorageRepository.CheckFileName(Request.Name, Request.Id, OldFile.FolderId))
                        {
                            throw new SpatiumException($"{fileName} Already Exists in the same folder!");
                        }
                        if (Request.File is not null)
                        {
                            _attachmentService.CheckFileExtension(Request.File);
                            _attachmentService.ValidateFileSize(Request.File);
                            string newFileName = _attachmentService.GetDesireFileName(Request.File, fileName);
                            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", OldFile.BlogId.ToString(), OldFile.Name + OldFile.Extention);
                            string imageUrl = $"{blogId}/{newFileName}";
                            string FileExtention=_attachmentService.GetFileExtention(Request.File);
                            var NewFilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", OldFile.BlogId.ToString(), Request.Name + FileExtention);                        
                            string filesize = Request.File.Length.ToString();
                            var UserId = GetUserId();
                            var UpdateFile = mapper.Map<UpdateFileInput>(Request);
                            UpdateFile.Url = imageUrl;
                            UpdateFile.LastUpdate = DateTime.Now;
                            UpdateFile.BlogId = blogId;
                            UpdateFile.Createdby = UserId;
                            UpdateFile.FileSize = filesize;
                            UpdateFile.Extention = _attachmentService.GetFileExtention(Request.File);
                            OldFile.Update(UpdateFile);

                            if (System.IO.File.Exists(uploadPath))
                            {
                                System.IO.File.Delete(uploadPath);
                            }
                            using (var stream = new FileStream(NewFilepath, FileMode.OpenOrCreate))
                            {
                              await Request.File.CopyToAsync(stream);
                            }
                                await unitOfWork.SaveChangesAsync();
                        }
                        else
                        {
                           string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", OldFile.BlogId.ToString(), OldFile.Name + OldFile.Extention);
                           var NewFilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", OldFile.BlogId.ToString(), Request.Name + OldFile.Extention);  
                           string imageUrl = $"{blogId}/{Request.Name}{OldFile.Extention}";
                           var UserId = GetUserId();
                            var UpdateFile = mapper.Map<UpdateFileInput>(Request);
                            UpdateFile.Url = imageUrl;
                            UpdateFile.LastUpdate = DateTime.Now;
                            UpdateFile.BlogId = blogId;
                            UpdateFile.Extention = OldFile.Extention;
                            UpdateFile.Createdby = UserId;
                            if (OldFile.Name != Request.Name)
                            {
                                OldFile.Update(UpdateFile);
                                System.IO.File.Copy(uploadPath, NewFilepath);
                                if (System.IO.File.Exists(uploadPath))
                                {
                                    System.IO.File.Delete(uploadPath);
                                }
                            }
                            await unitOfWork.SaveChangesAsync();
                        }
                        var response = new SpatiumResponse()
                        {
                            Message = ResponseMessages.FileUpdatedSuccessfully,
                            Success = true,
                        };
                        return Ok(response);
                    }
                    else
                    {
                        throw new SpatiumException("File Not Found");
                    }
                }
                return BadRequest(ModelState);
            });

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
                    var blogId = GetBlogId();
                    await unitOfWork.StorageRepository.DeleteFileAsync(Id,blogId);
                    await unitOfWork.SaveChangesAsync();
                    return Ok("Deleted");
                }
                return BadRequest(ModelState);
            });
        }


        [HttpPut]
        [Route("MoveFilesBulk")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.UpdateMedia)]
        public Task<IActionResult> MoveFilesBulk(List<int> filesIds, int? folderIdDestination = null)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();

                if (folderIdDestination != null)
                {
                    var folder = await unitOfWork.StorageRepository.GetFolderAsync(folderIdDestination.Value, blogId) ?? throw new SpatiumException($"Folder Not Found");
                }

                foreach (var file in filesIds)
                {
                    var fileToMove = await unitOfWork.StorageRepository.GetFileAsync(file, blogId) ?? throw new SpatiumException("File Not Existed To Move");
                    fileToMove.MoveToFolderId(folderIdDestination);
                    await unitOfWork.SaveChangesAsync();
                }
                return Ok("Files Moved Succsseded...");
            });
        }

        [HttpDelete]
        [Route("DeleteFilesBulk")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.DeleteMedia)]
        public Task<IActionResult> DeleteFilesBulk(List<int> filesIds)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                foreach (var file in filesIds)
                {
                    var fileToDelete = await unitOfWork.StorageRepository.GetFileAsync(file,blogId) ?? throw new SpatiumException("File Not Existed To Deleted");
                    await unitOfWork.StorageRepository.DeleteFileAsync(fileToDelete.Id,blogId);
                    await unitOfWork.SaveChangesAsync();
                }
                return Ok("Files Deleted Succsseded...");
            });
        }


        [HttpGet]
        [Route("GetAllFiles")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadMedia)]
        public Task<IActionResult> GetAllFiles([FromQuery] GetEntitiyParams entityParams)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var files = await unitOfWork.StorageRepository.GetAllFilesAsync(entityParams, blogId)??throw new SpatiumException("there are not files found !!");
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
                var blogId= GetBlogId();
                var file = await unitOfWork.StorageRepository.GetFileAsync(fileId, blogId) ?? throw new SpatiumException(ResponseMessages.FileNotFound);
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
                var blogId= GetBlogId();
                var file = await unitOfWork.StorageRepository.GetFileAsync(fileId, blogId) ?? throw new SpatiumException(ResponseMessages.FileNotFound);
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
            var blogId=GetBlogId();
            var currentDirectory = Directory.GetCurrentDirectory();
            var file = await unitOfWork.StorageRepository.GetFileAsync(fileId, blogId);

            if (file == null)
                return BadRequest(ResponseMessages.FileNotFound);
            
            var path = Path.Combine(currentDirectory, @"wwwroot\", file.UrlPath);
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(file.UrlPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return File(bytes, contentType, Path.GetFileName(path));
        }


        [HttpGet]
        [Route("ExtractFiles")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ExportMedia)]
        public Task<IActionResult> ExtractFiles(int? folderId)
        {
            return TryCatchLogAsync(async () =>
            {

                var blogId = GetBlogId();
                if (folderId!=null)
                {
                    var folder = unitOfWork.StorageRepository.GetFolderAsync(folderId.Value, blogId) ??throw new SpatiumException("folder not found !!");
                }
                var files = await unitOfWork.StorageRepository.GetFilesToExtract(blogId, folderId) ?? throw new SpatiumException("There are not files !!");

                var filesToZip = _attachmentService.FilesToExtract(files);
                var Identifire = new Random();
                var zipArchivePath = Path.Combine(Path.GetTempPath(),"Spatium_Cms_"+DateTime.Now.ToString("M")+"_"+DateTime.Now.ToString("t") + Identifire.Next(1, 100000).ToString() + ".zip");
                await _attachmentService.CreateZipArchive(filesToZip, zipArchivePath);

                var fileStreamToReturn = new FileStream(zipArchivePath, FileMode.Open);
                return File(fileStreamToReturn, "application/zip", "Spatium_Cms_" + DateTime.Now.ToString("M") +"_" + DateTime.Now.ToString("t") + Identifire.Next(1, 100000).ToString()+".zip");
            });
        }
       
        #endregion
    }
}
