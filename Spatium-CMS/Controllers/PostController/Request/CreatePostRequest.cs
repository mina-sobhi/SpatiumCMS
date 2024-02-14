using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.PostController.Request
{
    public class CreatePostRequest
    {
        [Required(ErrorMessage ="Title is Required")]
        [MinLength(3,ErrorMessage ="the length must mbe max 3 charachters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Slug is Required")]
        [MinLength(3, ErrorMessage = "the length must mbe max 3 charachters")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "FeaturedImagePath is Required")]
        [MinLength(3, ErrorMessage = "the length must mbe max 3 charachters")]
        public string FeaturedImagePath { get; set; }

        [Required(ErrorMessage = "Content is Required")]
        [MinLength(3, ErrorMessage = "the length must mbe max 3 charachters")]
        public string Content { get; set; }
        [Required(ErrorMessage = "MetaDescription is Required")]
        [MinLength(3, ErrorMessage = "the length must mbe max 3 charachters")]
        public string MetaDescription { get; set; }
        public int ContentLineSpacing { get; set; }
        [Required(ErrorMessage = "Category is Required")]
        [MinLength(3, ErrorMessage = "the length must mbe max 3 charachters")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Tag is Required")]
        [MinLength(3, ErrorMessage = "the length must mbe max 3 charachters")]
        public string Tag { get; set; }

        //public string PublishDate { get; set; }
        //public string UnPublishDate { get; set; }
        public string AuthorId { get; set; }
        public List<TableOfContentRequest> TableOfContentRequests { get; set; } = new List<TableOfContentRequest>();
    }
}
