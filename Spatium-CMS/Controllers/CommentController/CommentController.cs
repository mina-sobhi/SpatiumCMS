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
using Spatium_CMS.Controllers.PostController.Response;

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

        [HttpGet]
        [Authorize]
        [Route("GetTopPostsCommented")]
        [PermissionFilter(PermissionsEnum.ReadComment)]
        public Task<IActionResult> GetTopPostsCommented()
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();

                var posts = await unitOfWork.BlogRepository.GetTotalComments(blogId)?? throw new SpatiumException("Post Dose Not Contain Comments");

                List<TopPostsCommentedResponse> postsResponses = new List<TopPostsCommentedResponse>();
                foreach (var item in posts)
                {
                    postsResponses.Add(mapper.Map<TopPostsCommentedResponse>(item));
                }
                return Ok(postsResponses);
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
                if (commentRequest.StatusId != null)
                {
                    if (userPermissions.Any(p => p.Value == "401"))
                        comment.ChangeCommentStatus(commentRequest.StatusId.Value);
                    else
                        throw new SpatiumException("You Don't Have Permission To Add Status ");
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
