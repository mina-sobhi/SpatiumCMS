using Domain.ApplicationUserAggregate;
using Domain.Base;
using Domain.BlogsAggregate.Input;
using Domain.LookupsAggregate;
using System.ComponentModel.DataAnnotations;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Results;

namespace Domain.BlogsAggregate
{
    public class Post : EntityBase
    {
        #region Properties
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string FeaturedImagePath { get; private set; }
        public string Content { get; private set; }
        public string MetaDescription { get; private set; }
        public int ContentLineSpacing { get; private set; }
        public string Category { get; private set; }
        public string Tag { get; private set; }
        public DateTime? LastUpdate { get; private set; }
        public int LikeCount { get; private set; }
        public int ShareCount { get; private set; }
        public DateTime? PublishDate { get; private set; }
        public DateTime? UnPublishDate { get; private set; }
        public string CreatedById { get; private set; }
        public string AuthorId { get; private set; }
        public int BlogId { get; private set; }
        public int StatusId {get; private set; }
        public bool CommentsAllowed { get; private set; }  
        #endregion

        #region Navigational Properties
        [EnumDataType(typeof(PostStatus))]
        public virtual PostStatus Status { get; private set; }
        public virtual ApplicationUser CreatedBy { get; private set; }
        public virtual ApplicationUser Author { get; private set; }
        public virtual Blog Blog { get; private set; }
        #endregion

        #region Virtual Lists
        private readonly List<Comment> _comments = new List<Comment>();
        public virtual IReadOnlyList<Comment> Comments=> _comments.ToList();

        private readonly List<TableOfContent> _tableOfContents = new List<TableOfContent>();
        public virtual IReadOnlyList<TableOfContent> TableOfContents => _tableOfContents.ToList();
        #endregion

        #region CTOR
        public Post()
        {
            
        }
        public Post(PostInput postInput)
        {
            this.CreationDate = DateTime.UtcNow;
            this.IsDeleted = false;
            this.LikeCount = 0;
            this.ShareCount = 0;
            this.Title = postInput.Title;
            this.Slug = postInput.Slug;
            this.FeaturedImagePath = postInput.FeaturedImagePath;
            this.Content = postInput.Content;
            this.MetaDescription = postInput.MetaDescription;
            this.ContentLineSpacing = postInput.ContentLineSpacing;
            this.Category = postInput.Category;
            this.Tag = postInput.Tag;
            this.PublishDate = postInput.PublishDate == null ? null : postInput.PublishDate;
            this.UnPublishDate = postInput.UnPublishDate == null ? null : postInput.UnPublishDate;
            this.CreatedById = postInput.CreatedById;
            this.AuthorId = postInput.AuthorId;
            this.BlogId = postInput.BlogId;
            this.CommentsAllowed = postInput.CommentsAllowed;
            this.StatusId = (int)PostStatusEnum.Draft;

            foreach (var item in postInput.TableOfContents)
            {
                this._tableOfContents.Add(new TableOfContent(item));
            }
        }
        #endregion

        public void Update(UpdatePostInput postInput)
        {
            this.Title = postInput.Title;
            this.Slug = postInput.Slug;
            this.FeaturedImagePath = postInput.FeaturedImagePath;
            this.Content = postInput.Content;
            this.MetaDescription = postInput.MetaDescription;
            this.ContentLineSpacing = postInput.ContentLineSpacing;
            this.Category = postInput.Category;
            this.Tag = postInput.Tag;
            this.AuthorId = postInput.AuthorId;
            this.LastUpdate = DateTime.UtcNow;
            foreach (var tableOfContent in postInput.UpdateTableOfContentInput)
            {
                var toc = _tableOfContents.SingleOrDefault(x => x.Id == tableOfContent.Id) ?? throw new SpatiumException(ResponseMessages.TocNotFound);
                toc.Update(tableOfContent);
            }
        }

        public void Delete()
        {
            this.IsDeleted = true;
        }

        public void ChangePostStatus(PostStatusEnum postStatus)
        {
            if (postStatus == PostStatusEnum.Published)
            {
                this.PublishDate = DateTime.UtcNow;
                this.UnPublishDate = null;
                this.StatusId = (int)PostStatusEnum.Published;
            } 
            else if (postStatus == PostStatusEnum.Unpublished)
            {
                this.UnPublishDate = DateTime.UtcNow;
                this.PublishDate = null;
                this.StatusId = (int)PostStatusEnum.Unpublished;
            }
        }

        public void SchedualedPost(DateTime publishDate,DateTime unPublishedDate) {
            this.PublishDate = publishDate;
            this.UnPublishDate= unPublishedDate;
            this.StatusId = (int)PostStatusEnum.Scheduled;
        }
    }
}
