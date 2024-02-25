using Domain.BlogsAggregate;
using System.Linq.Expressions;

namespace Infrastructure.Strategies.AuthorizationStrategy
{
    public class OwnPostAuthorizationStrategy : IAuthorizationStrategy
    {
        private readonly string userId;
        private readonly int blogId;
        private readonly int postId;

        public OwnPostAuthorizationStrategy(string userId, int blogId, int postId)
        {
            this.userId = userId;
            this.blogId = blogId;
            this.postId = postId;
        }

        public Expression<Func<Post, bool>> GetDeletePostExpression()
        {
            Expression<Func<Post,bool>> filter= f=>f.CreatedById==userId && f.BlogId==blogId && f.Id==postId;
            return filter;
        }

        public Expression<Func<Post, bool>> GetPublishPostSelectExpression()
        {
            Expression<Func<Post, bool>> filter = f => f.CreatedById == userId && f.BlogId == blogId && f.Id == postId;
            return filter;
        }

        public Expression<Func<Post, bool>> GetUpdatePostExpression()
        {
            Expression<Func<Post,bool>> filter= f=>f.CreatedById==userId && f.BlogId==blogId && f.Id==postId;
            return filter;
        }
    }
}