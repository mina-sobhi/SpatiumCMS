using Domain.BlogsAggregate;
using System.Linq.Expressions;

namespace Infrastructure.Strategies.AuthorizationStrategy
{
    public interface IAuthorizationStrategy
    {
        public Expression<Func<Post, bool>> GetUpdatePostExpression();
        public Expression<Func<Post, bool>> GetDeletePostExpression();
    }
}
