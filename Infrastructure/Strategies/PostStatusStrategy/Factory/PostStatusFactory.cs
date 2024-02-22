using Utilities.Enums;
using Utilities.Extensions;

namespace Infrastructure.Strategies.PostStatusStrategy.Factory
{
    public class PostStatusFactory : IPostStatusFactory
    {
        public IPostStutsStrategy GetStrategy(string roleId)
        {

            if (MainRolesIdsEnum.ArticleCreator.GetDescription().Equals(roleId))
                return new  OwenPostStrategy();
            return new  AllPostStrategy();
        }
    }
}
