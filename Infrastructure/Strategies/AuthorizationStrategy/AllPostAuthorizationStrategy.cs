using Domain.BlogsAggregate;
using System.Linq.Expressions;

namespace Infrastructure.Strategies.AuthorizationStrategy
{
    public class AllPostAuthorizationStrategy : IAuthorizationStrategy
    {
        private readonly int blogId;
        private readonly int postId;
        public AllPostAuthorizationStrategy(int blogId, int postId)
        {
            this.blogId = blogId;
            this.postId = postId;
        }

        public Expression<Func<Post, bool>> GetDeletePostExpression()
        {
            Expression<Func<Post,bool>> expression=f=>f.BlogId==blogId && f.Id==postId;
            return expression;
        }

        public Expression<Func<Post, bool>> GetUpdatePostExpression()
        {
            Expression<Func<Post, bool>> filter = f=>f.BlogId == blogId && f.Id == postId;
            return filter;
        }
    }
}
