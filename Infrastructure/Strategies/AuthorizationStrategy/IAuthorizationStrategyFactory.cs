using Domain.ApplicationUserAggregate;

namespace Infrastructure.Strategies.AuthorizationStrategy
{
    public interface IAuthorizationStrategyFactory
    {
        public IAuthorizationStrategy GetEditStrategy(UserRole role, int blogId, string userId, int postId);
        public IAuthorizationStrategy GetDeleteStartegy(UserRole role, int blogId, string userId, int postId);
    }
}
