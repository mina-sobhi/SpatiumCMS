using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.StorageController.Request;
using Domain.StorageAggregate.Input;
using Domain.StorageAggregate;
using Microsoft.AspNetCore.Authorization;
using Spatium_CMS.Filters;
using Utilities.Enums;
using Infrastructure.Extensions;
using Utilities.Exceptions;
using Utilities.Results;
using Spatium_CMS.Controllers.StorageController.Response;
using Org.BouncyCastle.Asn1.X509;

namespace Spatium_CMS.Controllers.StorageController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : CmsControllerBase
    {

        public StorageController(ILogger<StorageController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, mapper, logger, userManager)
        {}
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
                var folder =await unitOfWork.StorageRepository.GetFolderAndFileByStorageIdAndFolderId(storag.Id, folderId);
                if(folder == null)
                {
                    throw new SpatiumException($"Invalid Folder Id ");
                }
                var deleteResponse = new DeleteResponse() { 
                    Id= folder.Id,
                    FolderName  = folder.Name,
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
        public  Task<IActionResult> ShowAllFolders([FromQuery] int? FolderId)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId= GetUserId();
                var blogId=GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId,userId)??throw new SpatiumException(ResponseMessages.UserNotFound);
                var storage = await unitOfWork.StorageRepository.GetStorageByBlogId(blogId)??throw new  SpatiumException("Invalid Blog Id");
               
                List<ViewFolderResponse> respone =  new List<ViewFolderResponse>();
                foreach (var folder in storage.Folders.Where(f=> f.ParentId == FolderId && f.IsDeleted == false))
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
                if( respone.Count == 0 )
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

                    var response =  new SpatiumResponse()
                    {
                        Message = $"F older Renamed Successfuly from {Request.OldName} To {folder.Name} ",
                        Success = true
                    };
                    return Ok(response);

                }
                return BadRequest(ModelState);
            });

        }
    }
}
