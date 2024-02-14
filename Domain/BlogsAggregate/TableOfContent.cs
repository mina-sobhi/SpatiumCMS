using Domain.Base;
using Domain.BlogsAggregate.Input;

namespace Domain.BlogsAggregate
{
    public class TableOfContent:EntityBase
    {
        #region Properties
        public int PostId { get; private set; }
        public int? ParentTableOfContentId { get; private set; }
        public string Content { get; private set; }
        #endregion

        #region Virtual Properties
        public virtual Post Post { get; private set; }
        public virtual TableOfContent ParentTableOfContent { get; private set; }
        #endregion

        #region Virtual List
        private readonly List<TableOfContent> _childTableOfContents = new List<TableOfContent>();
        public virtual IReadOnlyList<TableOfContent> ChildTableOfContents => _childTableOfContents.ToList();
        #endregion

        #region Ctor
        public TableOfContent()
        {
            
        }

        public TableOfContent(TableOfContentInput tableOfContentInput)
        {

            this.Content = tableOfContentInput.Content;
        }

        public void Update(UpdateTableOfContentInput updateTableOfContent)
        {
            this.Content = updateTableOfContent.Content;
        }
        #endregion
    }
}
