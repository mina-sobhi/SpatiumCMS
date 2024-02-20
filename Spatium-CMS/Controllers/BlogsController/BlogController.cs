using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.BlogsAggregate.Input;
using Domian.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spatium_CMS.Controllers.BlogsController.Request;
using Spatium_CMS.Controllers.BlogsController.Response;
using Spatium_CMS.Filters;
using Utilities.Enums;

namespace Spatium_CMS.Controllers.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : CmsControllerBase
    {
        public BlogController(ILogger<BlogController> logger,IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager) 
            : base(unitOfWork, mapper, logger, userManager)
        {
        }

        //[HttpGet("{Id:int}")]
        //public Task<IActionResult> GetBlogById(int Id)
        //{
        //    return TryCatchLogAsync(async () =>
        //    {
        //      var FlagBlog =await unitOfWork.BlogRepository.GetByIdAsync(Id);
        //        if(FlagBlog == null)
        //        {
        //            return NotFound();
        //        }
        //        var result = mapper.Map<BlogResult>(FlagBlog);
        //        return Ok(result);    
        //    });
        //}
        //[HttpGet]
        //[Route("getPaginations")]
        //[Authorize]
        //public Task<IActionResult> GetBlogs([FromQuery]PaginnationRequest paginnationRequest)
        //{
        //    return TryCatchLogAsync(async () =>
        //    {
        //        var userId = GetUserId();
        //        var user = await userManager.FindByIdAsync(userId);



        //        var blogs = await unitOfWork.BlogRepository.GetBlogsAsync();
        //        var totalBlogs = blogs.Count();
        //        if (totalBlogs== 0)
        //        {
        //            return BadRequest(" No Blog Yet ");
        //        }

        //        if(totalBlogs < paginnationRequest.NumberOfRecord)
        //        {
        //            return BadRequest();
        //        }
        //        int nuberOfPage = (int)Math.Floor(totalBlogs / (double)paginnationRequest.NumberOfRecord);
        //        var result =await unitOfWork.BlogRepository.FindAllAsync(paginnationRequest.LastPageID, paginnationRequest.NumberOfRecord);


        //        List<BlogResult> blogResults = new List<BlogResult>();

        //        foreach (var item in result)
        //        {
        //            blogResults.Add(mapper.Map<BlogResult>(item));

        //        }
        //        return Ok(blogResults);
        //    });
        //}
        //[HttpPost]
        //public Task<IActionResult> Create(CreateBlogRequest createBlogRequest)
        //{
        //    return TryCatchLogAsync(async () =>
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var bloginput = mapper.Map<BlogInput>(createBlogRequest);
        //            var blog = new Domain.BlogsAggregate.Blog(bloginput);
        //            await unitOfWork.BlogRepository.CreateAsync(blog);
        //            await  unitOfWork.SaveChangesAsync();
        //            return Ok(new BlogCreatedResponse
        //            {
        //                Message = $"Blog {createBlogRequest.Name} Created Successfuly"
        //            });
        //        }
        //        return BadRequest(ModelState);
        //    });

        //}
        ////[HttpPut]
        ////public Task<IActionResult> Update([FromQuery]int Id , UpdateBlogRequest updateBlogRequest)
        ////{
        ////    return TryCatchLogAsync(async () =>
        ////    {
        ////        if (ModelState.IsValid)
        ////        {
        ////            if(Id != updateBlogRequest.Id)
        ////            {
        ////                return BadRequest("Invalid Id");
        ////            }
        ////            if(await unitOfWork.BlogRepository.GetByIdAsync(updateBlogRequest.Id) is null)
        ////            {
        ////                return NotFound();
        ////            }
        ////            var bloginput = mapper.Map<BlogInput>(updateBlogRequest);
        ////            var blog = new Blog(bloginput);
        ////            await unitOfWork.BlogRepository.UpdateAsync(blog);
        ////            await unitOfWork.SaveChangesAsync();
        ////            return Ok(new BlogCreatedResponse
        ////            {
        ////                Message = $"Blog {updateBlogRequest.Name} Updated Successfuly"
        ////            });
        ////        }
        ////        return BadRequest(ModelState);
        ////    });
        ////}

        //[HttpDelete]
        //public Task<IActionResult> Remove(int Id)
        //{
        //    return TryCatchLogAsync(async () =>
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var blogModel = await unitOfWork.BlogRepository.GetByIdAsync(Id);
        //            if (blogModel is null)
        //            {
        //                return NotFound();
        //            }

        //            await unitOfWork.BlogRepository.DeleteAsync(blogModel.Id);
        //            await unitOfWork.SaveChangesAsync();
        //            return Ok(new BlogCreatedResponse
        //            {
        //                Message = $"Blog {blogModel.Name} Deleted Successfuly"
        //            });
        //        }
        //        return BadRequest(ModelState);
        //    });

        //}
    }
}
