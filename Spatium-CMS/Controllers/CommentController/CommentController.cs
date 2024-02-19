using AutoMapper;
using Domain.BlogsAggregate.Input;
using Domain.BlogsAggregate;
using Domian.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.BlogsController.Request;
using Spatium_CMS.Controllers.BlogsController.Response;
using Spatium_CMS.Controllers.CommentController.Response;
using Spatium_CMS.Controllers.CommentController.Request;

namespace Spatium_CMS.Controllers.CommentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : CmsControllerBase
    {
        private readonly ILogger<CommentController> logger;
        public CommentController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentController> logger) : base(unitOfWork, mapper,logger)
        {
            this.logger = logger;
        }
        [HttpGet("{Id:int}")]
        public Task<IActionResult> GetcommentById(int Id)
        {
            return TryCatchLogAsync(async () =>
            {
                var FlagComment = await unitOfWork.BlogRepository.GetCommentByIdAsync(Id);
                if (FlagComment == null)
                {
                    return NotFound();
                }
                var result = mapper.Map<CommentResponse>(FlagComment);
                return Ok(result);
            });
        }
        [HttpGet()]
        public Task<IActionResult> Getcomment()
        {
            return TryCatchLogAsync(async () =>
            {
                var comments = await unitOfWork.BlogRepository.GetCommentsAsync();
                if (comments.Count() == 0)
                {
                    return BadRequest(" No comment Yet ");
                }
                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var item in comments)
                {
                    commentResponses.Add(mapper.Map<CommentResponse>(item));
                }

                return Ok(commentResponses);
            });
        }
        [HttpPost]
        public Task<IActionResult> Create(CommentRequest commentRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var commentinput = mapper.Map<CommentInput>(commentRequest);
                    var comment = new Comment(commentinput);
                    await unitOfWork.BlogRepository.CreateCommentAsync(comment);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new 
                    {
                        Message = $" Comment  Created Successfuly"
                    });
                }
                return BadRequest(ModelState);
            });

        }
        [HttpPut]
        public Task<IActionResult> Update([FromQuery] int Id, UpdateCommentRequest updateCommentRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    if (Id != updateCommentRequest.Id)
                    {
                        return BadRequest("Invalid Id");
                    }
                    var fLagComment = await unitOfWork.BlogRepository.GetCommentByIdAsync(updateCommentRequest.Id);
                    if (fLagComment is null)
                    {
                        return NotFound();
                    }
                    var commentinput = mapper.Map<CommentUpdateInput>(updateCommentRequest);
                    fLagComment.Update(commentinput);
                    //   await unitOfWork.CommentRepository.UpdateAsync(fLagComment);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new 
                    {
                        Message = $"comment Updated Successfuly"
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
                    var commentModel = await unitOfWork.BlogRepository.GetCommentByIdAsync(Id);
                    if (commentModel is null)
                    {
                        return NotFound();
                    }
                    commentModel.Delete();
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new BlogCreatedResponse
                    {
                        Message = $"Post Deleted Successfuly"
                    });
                }
                return BadRequest(ModelState);
            });

        }

    }
}
