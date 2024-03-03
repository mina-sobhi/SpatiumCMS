using AutoMapper;
using Domain.BlogsAggregate.Input;
using Domain.BlogsAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.CommentController.Response;
using Spatium_CMS.Controllers.CommentController.Request;
using Microsoft.AspNetCore.Identity;
using Domain.ApplicationUserAggregate;
using Utilities.Exceptions;
using Utilities.Results;
using Microsoft.AspNetCore.Authorization;
using Spatium_CMS.Filters;
using Utilities.Enums;

namespace Spatium_CMS.Controllers.CommentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : CmsControllerBase
    {
        public CommentController(UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentController> logger) : base(unitOfWork, mapper,logger,userManager)
        {
        }

        [HttpGet]
        [Route("{commentId}")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadComment)]
        public Task<IActionResult> GetCommentById(int commentId)
        {
            return TryCatchLogAsync(async () =>
            {
                var Comment = await unitOfWork.BlogRepository.GetCommentByIdAsync(commentId) ??
                        throw new SpatiumException("Comment Not Found");  
                
                var result = mapper.Map<CommentResponse>(Comment);
                return Ok(result);
            });
        }

        [HttpGet]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadComment)]
        public Task<IActionResult> GetPostComments(int postId,string FilterColumn=null, string FilterValue = null)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();

                var comments = await unitOfWork.BlogRepository.GetCommentsAsync(blogId, postId, FilterColumn, FilterValue)?? throw new SpatiumException("Post Dose Not Contain Comments");

                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var item in comments)
                {
                    commentResponses.Add(mapper.Map<CommentResponse>(item));
                }

                return Ok(commentResponses);
            });
        }

        [HttpPost]
        [Authorize]
        [Route("CreateComment")]
        [PermissionFilter(PermissionsEnum.CreateComment)]
        public Task<IActionResult> CreateComment([FromQuery]CommentRequest commentRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();

                var post=await unitOfWork.BlogRepository.GetPostByIdAsync(commentRequest.PostId, blogId)?? throw new SpatiumException(ResponseMessages.PostNotFound);

                if (!post.CommentsAllowed)
                    throw new SpatiumException("Post Dose not Allowed Comment");

                var commentinput = mapper.Map<CommentInput>(commentRequest);
                commentinput.CreatedById = GetUserId();
                var comment = new Comment(commentinput);

                var userPermissions = User.Claims.Where(x => x.Type.Equals("Permissions")).ToList();
                if (userPermissions.Any(p => p.Value == "401"))
                {
                    if (commentRequest.StatusId!=null)
                    {
                        comment.ChangeCommentStatus(commentRequest.StatusId.Value);
                    }
                }

                await unitOfWork.BlogRepository.CreateCommentAsync(comment);
                await unitOfWork.SaveChangesAsync();

                return Ok(new
                {
                    Message = $" Comment  Created Successfuly"
                });

            });

        }

        [HttpPut]
        [Route("ChangeCommentStatus")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.UpdateComment)]
        public Task<IActionResult> ChangeCommentStatus(int commentId,CommentStatusEnum commentStatus)
        {
            return TryCatchLogAsync(async () =>
            {
                var comment = await unitOfWork.BlogRepository.GetCommentByIdAsync(commentId) ??
                        throw new SpatiumException("Comment Not Found!!");
                comment.ChangeCommentStatus(commentStatus);
                await unitOfWork.SaveChangesAsync();
                return Ok(new
                {
                    Message = $"Comment Status Changed  Successfuly"
                });
            });
        }

        [HttpDelete]
        [Authorize]
        [PermissionFilter(PermissionsEnum.DeleteComment)]
        public Task<IActionResult> DeleteComment(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                var comment = await unitOfWork.BlogRepository.GetCommentByIdAsync(Id)?? 
                        throw new SpatiumException("Comment Not Found!!");
                
                comment.Delete();
                await unitOfWork.SaveChangesAsync();

                return Ok(new
                {
                    Message = $"Comment Deleted Successfuly"
                });
            });

        }

    }
}
