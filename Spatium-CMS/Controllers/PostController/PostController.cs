﻿using AutoMapper;
using Domain.BlogsAggregate.Input;
using Domain.BlogsAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.PostController.Response;
using Spatium_CMS.Controllers.PostController.Request;
using Microsoft.AspNetCore.Identity;
using Domain.ApplicationUserAggregate;
using Microsoft.AspNetCore.Authorization;
using Domain.Base;
using Utilities.Enums;
using Spatium_CMS.Filters;
namespace Spatium_CMS.Controllers.PostController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : CmsControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<PostController> logger;

        public PostController(ILogger<PostController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, mapper, logger)
        {
            this.userManager = userManager;
        }

        [HttpGet("{Id:int}")]
        [Authorize]
        public Task<IActionResult> GetPostById(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var FlagPost = await unitOfWork.BlogRepository.GetPostByIdAsync(Id,blogId);
                if (FlagPost == null)
                {
                    return NotFound();
                }
                var result = mapper.Map<PostRespone>(FlagPost);
                return Ok(result);
            });
        }

        [HttpGet]
        [Route("GetPostSnippetPreview")]
        [Authorize]
        public Task<IActionResult> GetPostSnippetPreview(int postId)
        {
            return TryCatchLogAsync(async () =>
            {
                var FlagPost = await unitOfWork.BlogRepository.PostSnippetPreview(postId);
                if (FlagPost == null)
                {
                    return NotFound();
                }
                var result = mapper.Map<PostSnippetPreviewResponse>(FlagPost);
                return Ok(result);
            });
        }

        [HttpGet]
        [Route("GetPosts")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.ReadPost)]
        public Task<IActionResult> GetPosts([FromQuery] GetEntitiyParams postParams)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest("User Not found");
                var posts = await unitOfWork.BlogRepository.GetPostsAsync(postParams, user.BlogId);
                if (posts.Count() <= 0)
                {
                    return BadRequest(" No post Yet ");
                }
                List<PostRespone> PostsResults = new List<PostRespone>();
                foreach (var item in posts)
                {
                    PostsResults.Add(mapper.Map<PostRespone>(item));
                }
                return Ok(PostsResults);
            });
        }

        [HttpPut]
        [Route("SchedulePost")]
        [Authorize]
        public Task<IActionResult> SchedulePost(int postId, string ScheduledPublishDateTime, string ScheduledUnpublishDateTime)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId=GetBlogId();
                var found = await unitOfWork.BlogRepository.GetPostByIdAsync(postId, blogId);
                if (found != null)
                {
                    DateTime scheduledPublishDateTime;
                    DateTime scheduledUnpublishDateTime;

                    if (!DateTime.TryParse(ScheduledPublishDateTime, out scheduledPublishDateTime) ||
                        !DateTime.TryParse(ScheduledUnpublishDateTime, out scheduledUnpublishDateTime))
                    {
                        return BadRequest("Invalid date format");
                    }
                    if (!(scheduledPublishDateTime < DateTime.UtcNow && scheduledUnpublishDateTime <= DateTime.UtcNow) && !(scheduledUnpublishDateTime <= scheduledPublishDateTime) && !(scheduledPublishDateTime >= scheduledUnpublishDateTime))
                    {
                        found.SchedualedPost(scheduledPublishDateTime, scheduledUnpublishDateTime);
                        await unitOfWork.SaveChangesAsync();
                        return Ok(mapper.Map<PostRespone>(found));
                    }
                    return BadRequest("Invalid Schedual DateTime in PublishDateTime or UnpublishDateTime ");
                }
                return NotFound();
            });
        }

        [HttpPut]
        [Route("PublishedPost")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.PublishPost)]
        public Task<IActionResult> PublishedPost(int postId)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var found = await unitOfWork.BlogRepository.GetPostByIdAsync(postId, blogId);
                if (found != null)
                {

                    found.ChangePostStatus(PostStatusEnum.Published);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(mapper.Map<PostRespone>(found));
                }
                return NotFound();
            });
        }
        [HttpPut]
        [Route("UnPublishedPost")]
        [PermissionFilter(PermissionsEnum.UnpublishPost)]
        public Task<IActionResult> UnPublishedPost(int postId)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId= GetBlogId();
                var found = await unitOfWork.BlogRepository.GetPostByIdAsync(postId, blogId);
                if (found != null)
                {
                    var post = await unitOfWork.BlogRepository.GetPostByIdAsync(postId, blogId);
                    post.ChangePostStatus(PostStatusEnum.Unpublished);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(mapper.Map<PostRespone>(found));
                }
                return NotFound();
            });
        }

        [HttpPost]
        [Authorize]
        [PermissionFilter(PermissionsEnum.CreatePost)]
        public Task<IActionResult> Create(CreatePostRequest createPostRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var Postinput = mapper.Map<PostInput>(createPostRequest);

                    foreach (var item in createPostRequest.TableOfContentRequests)
                    {
                        var tableOfContentInput = mapper.Map<TableOfContentInput>(item);
                        Postinput.TableOfContents.Add(tableOfContentInput);
                    }
                    Postinput.CreatedById = GetUserId();
                    var user = await userManager.FindByIdAsync(Postinput.CreatedById);
                    Postinput.BlogId = user.BlogId;
                    Postinput.CommentsAllowed = true;
                    var post = new Post(Postinput);
                    await unitOfWork.BlogRepository.CreatePostAsync(post);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new
                    {
                        Message = $"post {createPostRequest.Title} Created Successfuly"
                    });
                }
                return BadRequest(ModelState);
            });

        }

        [HttpPut]
        [Authorize]
        [PermissionFilter(PermissionsEnum.UpdatePost)]
        public Task<IActionResult> Update(UpdatePostRequest updatePostRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var userId = GetUserId();
                    //var blogId=GetBlogId();
                    var user = await userManager.FindByIdAsync(userId);
                    //if(user.ro)
                    var post = await unitOfWork.BlogRepository.GetPostByOwnerId(userId,updatePostRequest.Id);
                    if (post is null)
                    {
                        return NotFound();
                    }

                    var postInput = mapper.Map<UpdatePostInput>(updatePostRequest);

                    foreach (var tableOfContent in updatePostRequest.TableOfContentRequests)
                    {
                        var contentUpdate = mapper.Map<UpdateTableOfContentInput>(tableOfContent);
                        postInput.UpdateTableOfContentInput.Add(contentUpdate);
                    }
                    post.Update(postInput);

                    await unitOfWork.SaveChangesAsync();
                    var response = new UpdatePostResponse()
                    {
                        Message = $"Post {post.Title} Updated Successfully!",
                        PostId = post.Id,
                    };
                    return Ok(response);
                }
                return BadRequest(ModelState);
            });
        }

        [HttpDelete]
        [PermissionFilter(PermissionsEnum.DeletePost)]
        public Task<IActionResult> Remove(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var blogId=GetBlogId();
                    var postModel = await unitOfWork.BlogRepository.GetPostByIdAsync(Id, blogId);
                    if (postModel is null)
                    {
                        return NotFound();
                    }
                    postModel.Delete();
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new
                    {
                        Message = $" post {postModel.Title} Deleted Successfuly "
                    });
                }
                return BadRequest(ModelState);
            });

        }
    }
}

