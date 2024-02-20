using Domain.ApplicationUserAggregate;

namespace Infrastructure.Strategies.AuthorizationStrategy
{
    public class AuthorizationStrategyFactory : IAuthorizationStrategyFactory
    {
        public AuthorizationStrategyFactory()
        {
        }

        public IAuthorizationStrategy GetDeleteStartegy(UserRole role, int blogId, string userId,int postId)
        {
            return role.Name switch
            {
                "Article Creator" => new OwnPostAuthorizationStrategy(userId, blogId,postId),
                _ => new AllPostAuthorizationStrategy(blogId, postId),
            };
        }

        public IAuthorizationStrategy GetEditStrategy(UserRole role, int blogId, string userId, int postId)
        {
            return role.Name switch
            {
                "Article Creator" => new OwnPostAuthorizationStrategy(userId,blogId,postId),
                _ => new AllPostAuthorizationStrategy(blogId,postId),
            };
        }
    }
}
