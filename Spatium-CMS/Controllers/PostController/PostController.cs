using AutoMapper;
using Domain.BlogsAggregate.Input;
using Domain.BlogsAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.BlogsController.Response;
using Spatium_CMS.Controllers.PostController.Response;
using Spatium_CMS.Controllers.PostController.Request;
using Microsoft.AspNetCore.Identity;
using Domain.ApplicationUserAggregate;
using Microsoft.AspNetCore.Authorization;
using Domain.Base;
using Utilities.Enums;
using Spatium_CMS.Filters;
using System.Security.Claims;
namespace Spatium_CMS.Controllers.PostController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : CmsControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PostController(IMapper mapper,IUnitOfWork unitOfWork ,UserManager<ApplicationUser> userManager):base(unitOfWork, mapper)
        {
            this.userManager = userManager;
        }

        [HttpGet("FilterPosts")]
        public Task<IActionResult> FilterPosts([FromForm]int StatusValue , [FromForm] string postTitle = "" )
        {
            return TryCatchLogAsync(async () =>
            {
                return Ok();
            });
        }

        [HttpGet("{Id:int}")]
        public Task<IActionResult> GetPostById(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                var FlagPost = await unitOfWork.PostRepository.GetByIdAsync(Id);
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
        public Task<IActionResult> GetPostSnippetPreview(int postId)
        {
            return TryCatchLogAsync(async () =>
            {
                var FlagPost = await unitOfWork.PostRepository.PostSnippetPreview(postId);
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
        public Task<IActionResult> GetPosts([FromQuery] GetEntitiyParams postParams)
        {
            return TryCatchLogAsync(async () =>
            {
                var posts = await unitOfWork.PostRepository.GetPostsAsync(postParams);
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
        [Route("ChangePostStatus")]
        public Task<IActionResult> ChangePostStatus(int postId,PostStatusEnum postStatus) {

            return TryCatchLogAsync(async () =>
            {
                var found = await unitOfWork.PostRepository.GetByIdAsync(postId);
                if (found!=null)
                {
                    var post=await unitOfWork.PostRepository.GetByIdAsync(postId);
                    post.ChangePostStatus(postStatus);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(mapper.Map<PostRespone>(found));
                }
                return NotFound();
            });
        }

        [HttpPost]
        [Authorize] 
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
                    var post = new Post(Postinput);
                    await unitOfWork.PostRepository.CreateAsync(post);
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
        [PermissionFilter(PermissionsEnum.UpdatePost)]
        public Task<IActionResult> Update([FromQuery] int Id, UpdatePostRequest updatePostRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    if (Id != updatePostRequest.Id)
                    {
                        return BadRequest("Invalid Id");
                    }
                    var userId = GetUserId();
                    var user= await userManager.FindByIdAsync(userId);
                    var post = await unitOfWork.PostRepository.GetByIdAsync(updatePostRequest.Id);
                    if (user.BlogId != post.BlogId)
                    {
                        return BadRequest("Invalid Post");
                    }
                    if (post is null)
                    {
                        return NotFound();
                    }

                    var postinput = mapper.Map<UpdatePostInput>(updatePostRequest);
                    post.Update(postinput);

                    foreach (var tableOfContent in post.TableOfContents)
                    {
                        tableOfContent.Update(mapper.Map<TableOfContentInput>(tableOfContent));
                    }
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new BlogCreatedResponse
                    {
                        Message = $"post {post.Title} Updated Successfuly"
                    });
                }
                return BadRequest(ModelState);
            });
        }

        [HttpDelete]
        public Task<IActionResult> Remove(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var postModel = await unitOfWork.PostRepository.GetByIdAsync(Id);
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

